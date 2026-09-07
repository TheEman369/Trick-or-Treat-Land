using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameExit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // This method will be called when the Exit button is clicked
    public void ExitGame()
    {
        Debug.Log("Exiting game..."); // Logs a message in the editor (useful for testing in the editor)
        Application.Quit(); // Exits the application

        // Note: Application.Quit() won't work in the editor. It only works in a built application.
    }

}
