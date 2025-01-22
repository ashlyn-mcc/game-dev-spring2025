using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class buildinPaddle : MonoBehaviour
{

    private float stepSpeed = 0.025f;

    private float rightEdge;

    private float leftEdge;

    private float totalGameWidth = 11f;

    private float paddleWidth;

    void Start()
    {
        calculateEdges();
        
    }

    void Update()
    {  

        paddleWidth = transform.localScale.x;
        
        // Set the default x to be the current
        float updatedX;

        // Adjust the x position of the paddle based on arrow keys
        if (Input.GetKey(KeyCode.D)){
            updatedX = transform.position.x + stepSpeed;
        } else if (Input.GetKey(KeyCode.A)){
            updatedX = transform.position.x - stepSpeed;
        }else {
            updatedX = transform.position.x;
        }

        updatedX = Math.Clamp(updatedX,leftEdge,rightEdge);


        // Update the whole position vector
        transform.position = new Vector3(updatedX, transform.position.y, transform.position.z);
       
    }

    void calculateEdges(){

        paddleWidth = transform.localScale.x;

        rightEdge = totalGameWidth / 2f - paddleWidth / 2f;
        leftEdge = totalGameWidth / -2f + paddleWidth / 2f;
    }

    


}
