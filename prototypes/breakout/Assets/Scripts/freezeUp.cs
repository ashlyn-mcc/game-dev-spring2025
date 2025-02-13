using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class freezeUp : MonoBehaviour
{

    public bool goingUp;

    private float speed;

    public freezeGenUp generator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (goingUp){
            speed = -0.001f;
        } else {
             speed = 0.001f;
        }

        float y = transform.position.y + speed;
        transform.position = new Vector3(transform.position.x, y, transform.position.z);

    }


    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("OutOfBounds")){
            Destroy(gameObject);
        } else if (collision.gameObject.CompareTag("Ball")){

            Debug.Log("Collided!!");
            generator.freezeBuild = true;

        }
    }   

}
