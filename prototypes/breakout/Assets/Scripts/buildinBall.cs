using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildinBall : MonoBehaviour
{

    private Vector3 velocity = new Vector3(.007f,-.007f,0f);

    private bool outOfBounds = false;

    private float newBallTimer = 0;

    private float respawnTime = 2f;

    private bool showBall = false;

    private int counter = 0;

    public bool builtABrick = false;
    
    void Start()
    {
        
    }

    
    void Update()
    {

       // Debug.Log(brokenABrick);

        if (!outOfBounds){

            transform.position = transform.position + velocity;

        } else {

            newBallTimer += Time.deltaTime;
            transform.position = new Vector3(0,4.5f,-0.28f);
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
        if (collision.gameObject.GetComponent<buildBrick>() == null){

            
            if (collision.gameObject.CompareTag("Vertical") || collision.gameObject.CompareTag("Paddle")){
                
                velocity = Vector3.Scale(velocity,new Vector3(1,-1,0));

                if (collision.gameObject.CompareTag("Paddle")){
                   builtABrick = false; 
                }

            } else if (collision.gameObject.CompareTag("Horizontal")){

                velocity = Vector3.Scale(velocity,new Vector3(-1,1,0));

            } else if (collision.gameObject.CompareTag("OutOfBounds")){

                gameObject.GetComponent<MeshRenderer>().enabled = false;
                outOfBounds = true;
                transform.position = new Vector3(0,4.5f,-0.28f);
                respawnTime += 1f;
            }

        } else { // Assume it is a brick

          Debug.Log("GETTING TO ELSE");

            if (collision.gameObject.GetComponent<buildBrick>().broken == false){

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
