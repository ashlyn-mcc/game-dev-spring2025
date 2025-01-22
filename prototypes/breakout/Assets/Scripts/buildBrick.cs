using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buildBrick : MonoBehaviour
{

    public bool broken = true;

    public int column;
    public int row;

    public buildinGenerator generator;
    
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
        if (collision.gameObject.CompareTag("Ball") && broken && !collision.gameObject.GetComponent<buildinBall>().builtABrick && generator.areBricksBuiltBeneath(row,column)){
            collision.gameObject.GetComponent<buildinBall>().builtABrick = true;
            broken = false;
            gameObject.GetComponent<MeshRenderer>().enabled = true;
            //Debug.Log(gameObject.GetComponent<MeshRenderer>().enabled);
        }

    }
}
