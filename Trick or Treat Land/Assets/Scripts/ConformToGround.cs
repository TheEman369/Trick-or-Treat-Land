using UnityEngine;

public class ConformToGround : MonoBehaviour
{
    [Tooltip("What counts as ground. Include Terrain and your board ground mesh layers.")]
    public LayerMask groundMask = ~0;

    [Tooltip("Start height for the downward ray.")]
    public float castHeight = 1000f;

    [Tooltip("Lift to avoid z-fighting with ground.")]
    public float yOffset = 0.02f;

    [Tooltip("Tilt the tile to the ground's normal (slope).")]
    public bool alignToNormal = true;

    void Start() => ConformNow();

    [ContextMenu("Conform Now")]
    public void ConformNow()
    {
        Vector3 p = transform.position;
        Vector3 start = new Vector3(p.x, p.y + castHeight, p.z);
        if (Physics.Raycast(start, Vector3.down, out RaycastHit hit, castHeight * 2f, groundMask))
        {
            transform.position = hit.point + Vector3.up * yOffset;
            if (alignToNormal) transform.up = hit.normal;
        }
    }
}
