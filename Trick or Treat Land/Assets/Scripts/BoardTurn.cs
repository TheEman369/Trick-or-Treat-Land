using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class BoardTurn : MonoBehaviour
{
    [Header("Board Turn Properties")]
    [Tooltip("Current turn of the game.")] public int turn = 1;
    [Tooltip("Array of player pieces.")] public BoardPieces[] players; // Put players here
    [Tooltip("Index of the current player.")] public int playerIndex = 0;
    [Tooltip("Number of finished players.")] public int finishCtr = 0;
    [Tooltip("Indicates if the game has finished.")] public bool finished = false;

    [Tooltip("Queue for the player order in the turn.")]
    public Queue<BoardPieces> playerSequence = new(); // Order of who will roll the dice

    [Tooltip("Indicates if the turn is finished.")] public bool turnFinished = false;

    [Header("Object References")]
    [Tooltip("Script for the dice roll mechanic.")] public DiceRoll rollScript;

    [Tooltip("Script for the Camera manager.")] public CameraManager camScript;

    [Tooltip("Text showing current player.")]
    public TextMeshProUGUI currentPlayerText;

    [Tooltip("Text showing turn number.")]
    public TextMeshProUGUI turnNumberText;

    [Tooltip("Text showing hints.")]
    public TextMeshProUGUI candyText;

    [Tooltip("Text showing hints.")]
    public TextMeshProUGUI hintText;

    [Tooltip("Text showing game winner.")] public TextMeshProUGUI winnerText;
    [Tooltip("GameObject for Win Screen UI.")] public GameObject gameFinUI;

    [Tooltip("Game object for player portrait.")]
    public GameObject[] playerPortraits;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetQueue();
        ShowTurnInfo();
        rollScript = FindAnyObjectByType<DiceRoll>();
        camScript = FindAnyObjectByType<CameraManager>();
    }

    // Resets the state of the board to turn 1
    public void ResetBoardState()
    {
        playerIndex = 0; // Make sure it starts at the first player in order
        turn = 1;
        ResetQueue();
    }

    // Returns the index of the player with the highest number of candies
    public int GetHighestCandyIndex()
    {
        int max = 0;
        for (int i = 1; i < players.Count(); i++)
        {
            if (players[i].candies > players[max].candies)
                max = i;
        }

        return max;
    }

    // Shows the turn info to reflect the current board state
    public void ShowTurnInfo()
    {
        currentPlayerText.text = "Current Player: " + (playerIndex + 1);
        turnNumberText.text = "Turn #" + turn;
        hintText.text = "Hint: \"It's Player " + (playerIndex + 1) + "'s turn. Roll the dice.\"";
        candyText.text = "Candies: " + players[playerIndex].candies;

        for (int i = 0; i < players.Count(); i++)
        {
            if (i != playerIndex)
                playerPortraits[i].SetActive(false);
            else
                playerPortraits[i].SetActive(true);
        }
    }

    // Resets the turn order for inside of the queue
    public void ResetQueue()
    {
        playerIndex = 0; // Make sure it starts at the first player in order
        playerSequence.Clear(); // Make sure there are no players inside the queue

        foreach (BoardPieces player in players)
        {
            if (!player.finished)
                playerSequence.Enqueue(player);
            else 
                finishCtr++;
        }
    }

    // Play's the current player's turn in the queue
    public void PlayTurn()
    {
        rollScript.RollDice();
        camScript.SwapCam(CameraManager.CurrentCamera.Dice);
        playerIndex++;
    }

    // Update is called once per frame
    void Update()
    {
        // End of turn
        if (playerSequence.Count <= 0)
        {
            ResetQueue();
            turn++;
        }

        // Dice has stopped rolling
        if (rollScript.rolled)
        {
            playerSequence.Dequeue().Move(rollScript.GetTotalRoll());
            camScript.ChangeCamTarget(playerIndex - 1);
            rollScript.rolled = false;
        }

        if (finishCtr >= players.Count() && !finished)
        {
            finished = true;
            gameFinUI.SetActive(true);
            winnerText.text = "Player " + (GetHighestCandyIndex() + 1) + " wins!";
        }
    }
}
