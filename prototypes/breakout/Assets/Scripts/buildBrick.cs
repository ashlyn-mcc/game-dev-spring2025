using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildBrick : MonoBehaviour
{

    // Wheher or not the brick has beeen broken
    public bool broken = true;

    // The indeces of the brick in the 2D array
    public int column;
    public int row;

    public buildinGenerator generator;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

     void OnTriggerEnter(Collider collision)
    {

        // Enter on four conditions
        // 1. The object that collided with the brick was a ball
        // 2. The brick is currently broken
        // 3. The ball hasn't already built a brick
        // 4. The brick doesn't have unbuilt brikcs beneath it
        if (collision.gameObject.CompareTag("Ball") && broken && !collision.gameObject.GetComponent<buildinBall>().builtABrick && generator.areBricksBuiltBeneath(row,column)){
            
            collision.gameObject.GetComponent<buildinBall>().builtABrick = true;
            broken = false;

            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.Play();

            // Make the brick visible
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            GetComponent<Collider>().isTrigger = false;
        }

    }
}
