using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambience : MonoBehaviour
{

    public Collider Area;            // Area of the sound
    public GameObject Player;        // The object to track


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Locate the closet point on the collider to the player
        Vector3 closetPoint = Area.ClosestPoint(Player.transform.position);

        // Set position to the closet point to the player
        transform.position = closetPoint;

    }
}
