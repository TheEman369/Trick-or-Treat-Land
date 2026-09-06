using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class SplineRibbonGround : MonoBehaviour
{
    
    [Header("Inputs")]
    public SplineContainer spline;      // Drag your SplineContainer here
    public float width = 2.5f;          // Visual path width
    public int samples = 300;           // More = smoother

    [Header("Ground Conform")]
    public bool conformToGround = true;
    public Terrain terrain;             // Assign if you use a Terrain
    public LayerMask groundMask = ~0;   // Used if no Terrain, or as fallback
    public float castHeight = 1000f;    // Raycast start height
    public float yOffset = 0.02f;       // Lift to avoid z-fighting
    public bool alignToSlope = false;   // If true, tilt edges to ground normals

    [Header("UV Tiling")]
    public float tileLength = 10f;     // one board space in world units
    public int stripesPerRepeat = 6;   // your PNG has 6 color bands
    public float uvStartOffset = 0f;   // 0..1: slide where the first stripe starts
    public bool flipV = false;

    Mesh _mesh;

    void OnValidate()
    {
        samples = Mathf.Max(16, samples);
        width = Mathf.Max(0.01f, width);
        tileLength = Mathf.Max(0.001f, tileLength);
    }

    [ContextMenu("Rebuild Ribbon")]
    public void Rebuild()
    {
        if (!spline)
        {
            Debug.LogWarning("SplineRibbonGround: Assign a SplineContainer.");
            return;
        }

        if (!_mesh)
        {
            var mf = GetComponent<MeshFilter>();
            _mesh = mf.sharedMesh ?? (mf.sharedMesh = new Mesh { name = "RibbonMesh" });
        }
        _mesh.Clear();

        int rows = samples + 1;
        var verts = new List<Vector3>(rows * 2);
        var uvs = new List<Vector2>(rows * 2);
        var tris = new List<int>((rows - 1) * 6);
        var norms = new List<Vector3>(rows * 2);

        // Accumulated distance along the curve drives UV.v so stripes repeat every tileLength.
        float totalDist = 0f;
        Vector3 prevP = spline.EvaluatePosition(0f);

        for (int i = 0; i < rows; i++)
        {
            float t = i / (float)samples;

            // Base position & tangent from spline
            Vector3 p = spline.EvaluatePosition(t);
            float3 tanF3 = spline.EvaluateTangent(t);
            Vector3 tan = ((Vector3)math.normalize(tanF3));

            // Distance accumulation for UVs
            if (i > 0) totalDist += Vector3.Distance(prevP, p);
            prevP = p;

            // Ground projection
            Vector3 n = Vector3.up;
            if (conformToGround)
            {
                if (terrain)
                {
                    float ty = terrain.SampleHeight(p) + terrain.GetPosition().y;
                    p.y = ty;
                    // better normal via raycast (optional)
                    RaycastHit hit;
                    if (Physics.Raycast(new Vector3(p.x, p.y + 2f, p.z), Vector3.down, out hit, 5f, groundMask))
                        n = hit.normal;
                }
                else
                {
                    // Raycast against meshes/colliders
                    Vector3 start = new Vector3(p.x, p.y + castHeight, p.z);
                    if (Physics.Raycast(start, Vector3.down, out RaycastHit hit, castHeight * 2f, groundMask))
                    {
                        p = hit.point;
                        n = hit.normal;
                    }
                }
                p += Vector3.up * yOffset;
            }

            // Compute flat "left" vector (keeps ribbon flat even on slopes)
            Vector3 left = new Vector3(-tan.z, 0f, tan.x).normalized;
            if (left.sqrMagnitude < 1e-6f) left = Vector3.left;

            // Optional slope alignment (tilt edges to ground)
            if (alignToSlope) left = Vector3.Cross(tan, n).normalized;

            Vector3 center = p;
            Vector3 vL = center + left * (width * 0.5f);
            Vector3 vR = center - left * (width * 0.5f);

            // UVs: U across width, V along length
            float v = uvStartOffset + totalDist / (tileLength * Mathf.Max(1, stripesPerRepeat));
            if (flipV) v = -v;
            verts.Add(vL); uvs.Add(new Vector2(0f, v));
            verts.Add(vR); uvs.Add(new Vector2(1f, v));
            norms.Add(n);
            norms.Add(n);
        }

        // Triangles
        for (int i = 0; i < rows - 1; i++)
        {
            int i0 = i * 2;
            int i1 = i0 + 1;
            int i2 = i0 + 2;
            int i3 = i0 + 3;
            tris.Add(i0); tris.Add(i2); tris.Add(i1);
            tris.Add(i2); tris.Add(i3); tris.Add(i1);
        }

        _mesh.SetVertices(verts);
        _mesh.SetUVs(0, uvs);
        _mesh.SetTriangles(tris, 0);
        _mesh.SetNormals(norms);
        _mesh.RecalculateBounds();
    }

    // Auto-create mesh filter/renderer if dropped onto a plain GameObject
    void Reset()
    {
        if (!TryGetComponent<MeshFilter>(out _)) gameObject.AddComponent<MeshFilter>();
        if (!TryGetComponent<MeshRenderer>(out _)) gameObject.AddComponent<MeshRenderer>();
        Rebuild();
    }

    // Rebuild when you tweak values in the Inspector (Editor-only convenience)
#if UNITY_EDITOR
    void OnValidateDelayed() => Rebuild();
#endif
}
