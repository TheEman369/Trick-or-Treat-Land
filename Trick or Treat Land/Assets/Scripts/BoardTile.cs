using UnityEngine;

public class BoardTile : MonoBehaviour
{
    // Enumeration for the tile colors
    public enum TileColorKey
    {
        None, // Enums at default start at zero
        PumpkinOrange,
        WitchPurple,
        SlimeGreen,
        GhostWhite,
        MidnightBlack,
        BloodRed
    }

    [Header("Tile Properties")]
    [Tooltip("Index of this tile.")] public int index = -1;                           

    [Tooltip("Color of the tile. Assigned automatically.")] 
    public TileColorKey color = TileColorKey.None;  

    [Tooltip("Marks if a tile is special.")] public bool isSpecial = false;                  
    [Tooltip("Marks if a tile is the starting one.")] public bool isStartTile = false;

    [Tooltip("Candy values based on color sequence (Black, Red, White, Green, Purple, Orange).")]
    public int[] candyValues = { -20, 5, -10, 10, -5, 20 };

    [Header("Connections")]
    public BoardTile nextTile = null;

    // Static repeating sequence for Halloween order
    private static readonly TileColorKey[] colorSequence = new TileColorKey[]
    {
        TileColorKey.MidnightBlack,
        TileColorKey.BloodRed,
        TileColorKey.GhostWhite,
        TileColorKey.SlimeGreen,
        TileColorKey.WitchPurple,
        TileColorKey.PumpkinOrange
    };

    // Assign index + color when the tile is created/initialized
    public void Initialize()
    {
        if (isStartTile)
        {
            color = TileColorKey.None;
            index = -1;
        }
        else
            color = colorSequence[index % colorSequence.Length];
    }

    // Add a tile that follows this one
    public void AddNextTile(BoardTile tile)
    {
        if (tile == null) return;

        // Increment index of the next tile and initialize it
        tile.index = index + 1; // Same as this.index + 1 
        tile.Initialize();

        nextTile = tile;
    }
}
