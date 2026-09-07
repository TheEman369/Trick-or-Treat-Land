using UnityEngine;

// Class used to handle logic for a single die
public class Die : MonoBehaviour
{
    [Tooltip("Array holding the face detectors of the die")]
    public GameObject[] faces;

    [Tooltip("Holds the rigid body of the die.")]
    public Rigidbody rb;

    private void Awake() 
    {
        rb = GetComponent<Rigidbody>(); 
    }

    // Returns the value of the top face as an integer
    public int GetTopFaceValue()
    {
        // Get the index of the face with the highest y position
        int maxIndex = 0; 
        for (int i = 0; i < faces.Length; i++)
        {
            if (faces[i].transform.position.y > faces[maxIndex].transform.position.y)
                maxIndex = i;
        }

        return maxIndex + 1; // Index starts at 0; Die value usually starts at one
    }

    // Checks if the rigidbody of the current object has stopped moving
    public bool StoppedMoving()
    {
        if (rb.linearVelocity == Vector3.zero && 
            rb.angularVelocity == Vector3.zero)
            return true;
        else return false;
    }

    // Throws the die with random rotation, force, and torque 
    public void ThrowDice() 
    {
       // Set random rotation for the die
       int x = Random.Range(0, 360);
       int y = Random.Range(0, 360);
       int z = Random.Range(0, 360);
       Quaternion rotation = Quaternion.Euler(x,y,z); 

       // Set random amount of linear force to the die
       x = Random.Range(0, 155);       
       y = Random.Range(50, 155);       
       z = Random.Range(0, 155);       
       Vector3 force = new(x, -y, z);  

       // Set random amount of torque to the die
       x = Random.Range(0, 55);       
       y = Random.Range(0, 55);       
       z = Random.Range(0, 55);       
       Vector3 torque = new(x, y, z);  

       // Apply rotation, velocity, and torque to the die
       transform.rotation = rotation;
       rb.linearVelocity = force; 
       rb.maxAngularVelocity = 1000; // Max angular velocity is capped by 7 by default.
       rb.AddTorque(torque, ForceMode.VelocityChange);
    }  
}
