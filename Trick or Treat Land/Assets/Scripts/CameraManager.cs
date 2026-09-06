using System.Linq;
using Unity.Cinemachine;
using UnityEngine;

// Class for managing the in-game cameras
public class CameraManager : MonoBehaviour
{
    // Enumeration for the current camera used
    public enum CurrentCamera
    {
        Overworld,
        Dice,
        Player
    }

    [Tooltip("Cameras used in the game.")] public GameObject[] cameras;
    [Tooltip("Transforms of the player pieces.")] public Transform[] players;
    [Tooltip("CinemachineCamera component of the player camera.")] public CinemachineCamera playerCam;

    void Start()
    {
        SwapCam(CurrentCamera.Overworld);
    }

    // Shows appropriate camera given enum
    public void SwapCam(CurrentCamera cam)
    {
        int camIndex = 0; // Index of the appropriate camera

        switch (cam) // Assuming cameras are in the following order
        {
            case CurrentCamera.Overworld:
                camIndex = 0; 
                break;
            case CurrentCamera.Dice:
                camIndex = 1;
                break;
            case CurrentCamera.Player:
                camIndex = 2;
                break;
        }

        // Enable / Disable the cameras
        for (int i = 0; i < cameras.Count(); i++)
        {
            if (i == camIndex)
            {
                cameras[i].SetActive(true);
            }
            else 
                cameras[i].SetActive(false);
        }
    }

    // Changes the target of the player camera
    public void ChangeCamTarget(int playerIndex)
    {
        playerCam.Follow = players[playerIndex];
        playerCam.LookAt = players[playerIndex]; 
    }
}
