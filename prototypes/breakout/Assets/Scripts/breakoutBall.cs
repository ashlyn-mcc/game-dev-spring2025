using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class breakoutBall : MonoBehaviour
{

    // Velocity of the ball
    private Vector3 velocity = new Vector3 (0.05f, 0.05f, 0f);

    // Whether or not the ball has gone behind the paddle
    private bool outOfBounds = false;

    // The timer to track respawn and the amount of time before respawn allowed
    private float newBallTimer = 0;
    private float respawnTime = 2f;

    // Whether or not the ball is shown (disappears if out of bounds)
    private bool showBall = false;

    // Counter to help modulate the ball's appearance when it is respawning
    private int counter = 0;

    // Whether the ball has broken a brick since it last hit the paddle
    public bool brokenABrick = false;
    
    void Start()
    {
        
  
    }

    
    void Update()
    {

        // As long as the ball is in bounds, move it according to its velocity
        if (!outOfBounds){

            transform.position = transform.position + velocity;

        } else {

            // if the ball is out of bounds, start the timer for a new ball
            newBallTimer += Time.deltaTime;
            transform.position = new Vector3(0,-2.5f,-0.28f);
            counter++;

            // during the last 1.5 seconds, flash the ball so the player knows they are about to get another one
            if (newBallTimer > respawnTime - 1.5 && counter % 50 == 0){
                gameObject.GetComponent<MeshRenderer>().enabled = showBall;
                showBall = !showBall;
            }

            // Once time has elapsed, make the ball visible and resume gameplay
            if (respawnTime < newBallTimer){
                outOfBounds = false;
                gameObject.GetComponent<MeshRenderer>().enabled = true;
                newBallTimer = 0;
            }

        }

        
    }

    void OnTriggerEnter(Collider collision)
    {

        // If the ball hasn't collided with a brick
        if (collision.gameObject.GetComponent<breakBrick>() == null){


            // Test if it was a vertical, horizontal, paddle, or out of bounds surface hit.
            // Change the velocity accordingly
            if (collision.gameObject.CompareTag("Vertical") || collision.gameObject.CompareTag("Paddle")){
                
                velocity = Vector3.Scale(velocity,new Vector3(1,-1,0));

                if (collision.gameObject.CompareTag("Paddle")){
                   brokenABrick = false; 
                }

            } else if (collision.gameObject.CompareTag("Horizontal")){

                velocity = Vector3.Scale(velocity,new Vector3(-1,1,0));

            } else if (collision.gameObject.CompareTag("OutOfBounds")){

                gameObject.GetComponent<MeshRenderer>().enabled = false;
                outOfBounds = true;
                transform.position = new Vector3(0,-2.5f,-0.28f);
                respawnTime += 1f;
            }

        } else { 

            // If the ball did collide with a brick, and the brick has not yet broken another brick
            if (collision.gameObject.GetComponent<breakBrick>().broken == false){

                // I used AI for the code inside this if statement:

                // Convert the collision point to the cube's local space
                Vector3 localCollisionPoint = transform.InverseTransformPoint(collision.transform.position);

                // Get the absolute values of the local collision point coordinates
                float x = Mathf.Abs(localCollisionPoint.x);
                float y = Mathf.Abs(localCollisionPoint.y);
                float z = Mathf.Abs(localCollisionPoint.z);

                // Check which axis has the greatest magnitude to determine the primary direction of collision
                if (y > z && y > x) {
                    // Collision on the top or bottom face of the cube
                    velocity = Vector3.Scale(velocity, new Vector3(1, -1, 1));
                    Debug.Log("hit top/bottom");
                }
                if (x > y && x > z) {
                    // Collision on the left or right face of the cube
                    velocity = Vector3.Scale(velocity, new Vector3(-1, 1, 1));
                    Debug.Log("hit left/right");
                }


            } 

        }


    }


}
