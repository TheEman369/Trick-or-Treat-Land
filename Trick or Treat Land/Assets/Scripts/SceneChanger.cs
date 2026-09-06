using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Ensure Time.timeScale is reset in case the game was paused before entering the scene
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangingScenes(int intBuildIndex)
    {

        // Reset Time.timeScale before changing the scene
        Time.timeScale = 1f;

        SceneManager.LoadScene(intBuildIndex);
        Debug.Log("I changed the scene!"); // remove before building game
        Debug.Log($"Scene changed to index: {intBuildIndex}");
    }
}
