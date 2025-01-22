using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class breakoutBall : MonoBehaviour
{

    // Velocity of the ball
    private Vector3 velocity;

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
        // I used AI for this bit. I wasn't sure how to get random floats in C#

        // Create Random object and define the min and max possible velocities
        System.Random random = new System.Random();
        float minValue = 0.1f;
        float maxValue = 0.2f;
        
        // Generate a random double between 0 and 1, scale it between the min and max, 
        // then make sure it is within the range, and then cast as float
        float randomFloat = (float)(random.NextDouble() * (maxValue - minValue) + minValue);

        // Set the ball's velocity
        velocity = new Vector3 (minValue, maxValue, 0f);

    }

    
    void Update()
    {

        if (!outOfBounds){

            transform.position = transform.position + velocity;

        } else {

            newBallTimer += Time.deltaTime;
            transform.position = new Vector3(0,-3.3f,-0.28f);
            counter++;

            if (newBallTimer > respawnTime - 1.5 && counter % 50 == 0){
                gameObject.GetComponent<MeshRenderer>().enabled = showBall;
                showBall = !showBall;
            }

            if (respawnTime < newBallTimer){
                outOfBounds = false;
                gameObject.GetComponent<MeshRenderer>().enabled = true;
                newBallTimer = 0;
            }

        }

       // Debug.Log(newBallTimer);

        
    }

    void OnTriggerEnter(Collider collision)
    {

        // Assume it isn't a brick (proceed as usual)
        if (collision.gameObject.GetComponent<breakBrick>() == null){


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
                transform.position = new Vector3(0,-3.3f,-0.28f);
                respawnTime += 1f;
            }

        } else { // Assume it is a brick

        

            if (collision.gameObject.GetComponent<breakBrick>().broken == false){

                //Vector3 collisionPoint = GetComponent<Collider>().ClosestPoint(transform.position);

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


            } else {
                
                Debug.Log("broken brick");
            }

        }


    }


}
