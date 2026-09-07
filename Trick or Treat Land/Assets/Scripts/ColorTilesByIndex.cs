using UnityEngine;

// Class used to color board tiles by their indices
public class ColorTilesByIndex : MonoBehaviour
{
    [Tooltip("Put color materials here to be used. Use 6 for the Halloween palette.")]
    public Material[] palette; // size = 6

    [Tooltip("Cycle length. Usually 6 for the Halloween palette.")]
    public int cycleLength = 6;

    [Tooltip("Shift the starting color along the path. 0 = start with palette[0].")]
    public int startIndexOffset = 0;

    void Start() => ApplyColor();

    [ContextMenu("Apply Color Now")]
    public void ApplyColor()
    {
        // Guard clause; Do not continue if there is no pallete
        if (palette == null || palette.Length == 0) return;

        // Use sibling order under the parent (works great if you parent all tiles under a single object)
        int order = transform.GetSiblingIndex();

        int index = (startIndexOffset + order) % Mathf.Max(1, cycleLength);
        var renderer = GetComponentInChildren<Renderer>();
        if (renderer)
            renderer.sharedMaterial = palette[index % palette.Length];
    }
}
