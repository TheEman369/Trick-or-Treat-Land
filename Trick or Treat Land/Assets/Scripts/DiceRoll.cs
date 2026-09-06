using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

// Class used for handling dice roll logic
public class DiceRoll : MonoBehaviour
{
    /* DICE ROLL PROPERTIES */
    [Header("Dice Roll Properties")]
    [Tooltip("Array holding the dice.")]
    [SerializeField] private Die[] dice;

    [Tooltip("Distance between camera and dice.")]
    [SerializeField] private float cameraToDiceDistance = 10.0f;

    [Tooltip("Max speed for animation when showing dice.")]
    [SerializeField] private float maxShowSpeed = 15.0f;

    [Tooltip("Smooths the dice show animation.")]
    [SerializeField] private float smoothTime = 0.1f;

    /* COMPONENT REFERENCES */
    [Header("Component References")]
    [Tooltip("Text holding the result of the dice.")]
    [SerializeField] private TextMeshProUGUI totalText;

    [Tooltip("Main camera for the game.")]
    [SerializeField] private Camera camera; 
    
    [Tooltip("Invisible Dicebox for the dice roll.")]
    [SerializeField] private GameObject diceBox; 
    
    // Holds the value for the total value of the current dice roll
    private int totalRoll = 0;

    public bool rolled = false; // Bool to check if dice has been rolled
    private Vector3 currentVelocity; // Used for showdice animation

    public void Update()
    {
        if (rolled)
            ShowDice();
    }

    // Returns the total value of the dice roll
    public int GetTotalRoll()
    {
        return totalRoll;
    }

    // Rolls the dice inside the dice array and outputs the total value of
    // all the top face values of the dice combined
    public async void RollDice()
    {
        ResetDiceState();

        // Create tasks for checking the value of each die
        var tasks = new List<Task>();
        foreach(Die die in dice) {
            die.rb.useGravity = true;
            die.ThrowDice();
            tasks.Add(CheckForValue(die));
        }

        // Wait for calculations to finish before giving total
        await Task.WhenAll(tasks); 

        // Set total roll to appropriate UI element
        totalText.text = totalRoll.ToString(); 
        rolled = true;
    }

    // Shows the dice in front of the main camera
    public void ShowDice()
    {
        diceBox.SetActive(false);
        foreach(Die die in dice)
        {
            die.rb.useGravity = false; // We don't want the dice to fall when showing it to the camera
            die.transform.position = Vector3.SmoothDamp(die.transform.position,
                                                        camera.transform.position
                                                        + camera.transform.forward 
                                                        * cameraToDiceDistance,
                                                        ref currentVelocity,
                                                        smoothTime,
                                                        maxShowSpeed);
            die.faces[1].transform.LookAt(camera.transform.position);
        }
    }

    // Reset's all of the dice's state
    public void ResetDiceState()
    {
        totalRoll = 0; // Reset total roll number just in case it has a value
        diceBox.SetActive(true);

        // Reset each dice's velocity, gravity, and position
        foreach(Die die in dice)
        { 
            die.rb.linearVelocity = Vector3.zero;
            die.rb.angularVelocity = Vector3.zero;
        }
    }

    // Asynchronous task that checks for the value of a given die
    // when the die stops moving.
    private async Task CheckForValue(Die die) 
    {
        // Wait for the die to stop moving 
        await Task.Yield(); 
        while (!die.StoppedMoving())
        {
            await Task.Yield();
        }

        // Only add the face value when die has stopped moving
        totalRoll += die.GetTopFaceValue();
    }
}
