using System.Collections.Generic;
using UnityEngine;

// Class for handling the logic of the board's path
public class BoardPath : MonoBehaviour
{
    [Tooltip("List of tiles/nodes for the path")]
    public List<BoardTile> tiles = new();

    // Returns the tile count of the board
    public int GetTileCount() => tiles.Count;

    void Awake() 
    {
        InitializeTiles(); // Initialize tiles at the game load
    }

    // Returns the board tile given its index
    public BoardTile GetTile(int index)  
    {
        if (index < 0 || index > GetTileCount())
            return null; // Invalid index 
        return tiles[index];
    }

    // Initializes all of the tiles in the board
    public void InitializeTiles()
    {
        if (tiles == null || tiles.Count <= 0) return;

        // Initialize the first tile
        tiles[0].isStartTile = true;
        tiles[0].Initialize();

        // Initialize the rest of the tiles 
        for (int i = 0; i < tiles.Count - 1; i++)
        {
            tiles[i].AddNextTile(tiles[i+1]);
        }
    }
}

