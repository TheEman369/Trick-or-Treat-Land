using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BoardPieces : MonoBehaviour
{
    [Header("Board Piece Properties")]
    [Tooltip("Number of candies the piece has.")] public int candies = 0;
    [Tooltip("Number of moves the piece has left.")] public int movesLeft = 0;
    [Tooltip("Indicates if the piece can move.")] public bool canMove = false;
    [Tooltip("Indicates that the player has finished.")] public bool finished = false;

    [Header("Object References")]
    [Tooltip("Current tile the player is in.")] public BoardTile currentTile;
    [Tooltip("NavMeshAgent for this board piece.")] public NavMeshAgent pathing;
    [Tooltip("Script for the Camera manager.")] public CameraManager camScript;
    [Tooltip("Script for the board turn logic")] public BoardTurn turnScript;
    [Tooltip("Button used for rolling dice")] public Button rollButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pathing = GetComponent<NavMeshAgent>();
        camScript = FindAnyObjectByType<CameraManager>();
        turnScript = FindAnyObjectByType<BoardTurn>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
            MoveToTile();
    }

    // Moves the player within the given number of moves 
    public void Move(int moves)
    {
        canMove = true;
        movesLeft = moves;
        camScript.SwapCam(CameraManager.CurrentCamera.Player);
    }

    // Updates the player's candy value given the amount 
    public void UpdatePlayerCandy(int amount)
    {
        candies += amount; 

        // Clamp min value to 0. There are no negative candies
        if (candies < 0)
            candies = 0;
    }

    // Moves piece to the next tile if the piece has moves left
    public void MoveToTile()
    {
        if (pathing.remainingDistance <= pathing.stoppingDistance &&
                    !pathing.pathPending)
        {
            if (!currentTile.nextTile) // If the current tile is the goal/last tile
            {
                finished = true;
                return;
            }
            if (movesLeft > 0) // If there are moves left
            {
                movesLeft--;
                currentTile = currentTile.nextTile; // Go to the next tile
                // Set the destination the to the waypoint
                pathing.SetDestination(currentTile.transform.position);
            }
            else // No moves left
            {
                camScript.SwapCam(CameraManager.CurrentCamera.Overworld);
                UpdatePlayerCandy(currentTile.candyValues[currentTile.index % 
                                  currentTile.candyValues.Count()]);
                turnScript.ShowTurnInfo();
                canMove = false;
            }
        }

    }
}
