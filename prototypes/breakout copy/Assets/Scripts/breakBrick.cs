using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breakBrick : MonoBehaviour
{

    // Whether or not the brick has been broken by the ball
    public bool broken = false;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

     void OnTriggerEnter(Collider collision)
    {

        // Enter on three conditions
        // 1. The object that hit the brick was the ball
        // 2. The brick has not yet already been broken
        // 3. The ball hasn't already broken a brick

        if (collision.gameObject.CompareTag("Ball") && !broken && !collision.gameObject.GetComponent<breakoutBall>().brokenABrick){
            collision.gameObject.GetComponent<breakoutBall>().brokenABrick = true;
            broken = true;

            // Make the brick invisible
            gameObject.GetComponent<MeshRenderer>().enabled = false;

        }

    }
}
