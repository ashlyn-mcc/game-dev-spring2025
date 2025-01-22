using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breakBrick : MonoBehaviour
{

    public bool broken = false;

    public

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     void OnTriggerEnter(Collider collision)
    {

        // if the brick collided with a ball
        if (collision.gameObject.CompareTag("Ball") && !broken && !collision.gameObject.GetComponent<breakoutBall>().brokenABrick){
            collision.gameObject.GetComponent<breakoutBall>().brokenABrick = true;
            broken = true;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            //Debug.Log(gameObject.GetComponent<MeshRenderer>().enabled);
        }

    }
}
