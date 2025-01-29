using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildinBall : MonoBehaviour
{

    // The velocity of the ball
    private Vector3 velocity = new Vector3 (0.05f, 0.05f, 0f);

    // Whether it's gone out of bounds
    private bool outOfBounds = false;

    // Timer and amount of time for getting a new ball
    private float newBallTimer = 0;
    private float respawnTime = 2f;

    // Whether the ball is visible
    private bool showBall = false;

    private int counter = 0;

    // Whether the ball has already built a brick since last hitting the paddle
    public bool builtABrick = false;
    
    void Start()
    {
    
    }

    
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Tab)){
            velocity = new Vector3(0.01f,0.05f,0f);
        }

        // If ball is in bounds, move its position based on velocity
        if (!outOfBounds){

            transform.position = transform.position + velocity;

        } else { // If ball is out of bounds

            // Start the timer for a new ball
            newBallTimer += Time.deltaTime;
            transform.position = new Vector3(0,4.5f,-0.28f);
            counter++;

            // Flash the ball on and off to indicate it is about to respawn
            if (newBallTimer > respawnTime - 1.5 && counter % 50 == 0){
                gameObject.GetComponent<MeshRenderer>().enabled = showBall;
                showBall = !showBall;
            }

            // Resume gameplay after elapsed time
            if (respawnTime < newBallTimer){
                outOfBounds = false;
                gameObject.GetComponent<MeshRenderer>().enabled = true;
                newBallTimer = 0;
            }

        }

        
    }

    void OnCollisionEnter(Collision collision)
    {   

        // If the collided object wasn't a brick enter and adjust the velocity based on what time of edge the ball hit
        if (collision.gameObject.GetComponent<buildBrick>() == null){

            
            if (collision.gameObject.CompareTag("Vertical") || collision.gameObject.CompareTag("Paddle")){
                
                // Get contact point of collider
                Collider paddleCollider = collision.gameObject.GetComponent<Collider>();
                ContactPoint contact = collision.contacts[0];

                // Calculate where on paddle the hit occured
                Vector3 paddleCenter = paddleCollider.bounds.center;
                float hitPosition = contact.point.x - paddleCenter.x;

                // Normalize it between 1 and -1 and then multiply by max angle (60 degrees)
                float normalizedHitPosition = hitPosition / paddleCollider.bounds.extents.x;
                float angle = normalizedHitPosition * 60f; 

                // Figure out the new direction.
                Vector3 newDirection = new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle), 0f);

                // Needs to move in the negative direction, because the build game is inversed
                newDirection.y = newDirection.y * -1;

                // Normalize it
                newDirection.Normalize();

                // Apply the original speed
                velocity = newDirection * velocity.magnitude;

                if (collision.gameObject.CompareTag("Paddle")){
                   builtABrick = false; 
                }

            } else if (collision.gameObject.CompareTag("Back")){
                
                velocity = Vector3.Scale(velocity,new Vector3(0,-1,0));

            } else if (collision.gameObject.CompareTag("Horizontal")){

                velocity = Vector3.Scale(velocity,new Vector3(-1,1,0));

            } else if (collision.gameObject.CompareTag("OutOfBounds")){

                gameObject.GetComponent<MeshRenderer>().enabled = false;
                outOfBounds = true;
                transform.position = new Vector3(0,4.5f,-0.28f);
                respawnTime += 1f;
            }

        Debug.Log(velocity);

        } else { // Enter if the collided object was a brick

          //Debug.Log("GETTING TO ELSE");

            if (collision.gameObject.GetComponent<buildBrick>().broken == false){

               // Convert the collision point to the cube's local space
                    Vector3 localCollisionPoint = transform.InverseTransformPoint(collision.contacts[0].point);

                    // Get the absolute values of the local collision point coordinates
                    float x = Mathf.Abs(localCollisionPoint.x);
                    float y = Mathf.Abs(localCollisionPoint.y);
                    float z = Mathf.Abs(localCollisionPoint.z);

                    // Check which axis has the greatest magnitude to determine the primary direction of collision
                    if (y > z && y > x) 
                    {
                        // Collision on the top or bottom face of the cube
                        velocity = Vector3.Scale(velocity, new Vector3(1, -1, 1));
                        Debug.Log("Hit top/bottom");
                    }
                    else if (x > y && x > z) 
                    {
                        // Collision on the left or right face of the cube
                        velocity = Vector3.Scale(velocity, new Vector3(-1, 1, 1));
                        Debug.Log("Hit left/right");
                    }


            }

        }


    }


}
