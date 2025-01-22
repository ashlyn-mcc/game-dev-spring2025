using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class breakoutPaddle : MonoBehaviour
{

    // Speed the paddle moves
    private float stepSpeed = 0.1f;

    // Variables to store the bounds the paddle can move
    private float rightEdge;
    private float leftEdge;

    // The total width of the gamespace
    private float totalGameWidth = 11f;

    // The width of the paddle
    private float paddleWidth;

    void Start()
    {
        calculateEdges();
        
    }

    void Update()
    {  

       // paddleWidth = transform.localScale.x;
    
        float updatedX;

        // Adjust the x position of the paddle based on arrow keys
        if (Input.GetKey(KeyCode.RightArrow)){
            updatedX = transform.position.x + stepSpeed;
        } else if (Input.GetKey(KeyCode.LeftArrow)){
            updatedX = transform.position.x - stepSpeed;
        }else {
            updatedX = transform.position.x;
        }

        // Clamp the x value so it is within the bounds
        updatedX = Math.Clamp(updatedX,leftEdge,rightEdge);


        // Update the position of the paddle
        transform.position = new Vector3(updatedX, transform.position.y, transform.position.z);
       
    }

    void calculateEdges(){

        // Get the width of the paddle
        paddleWidth = transform.localScale.x;

        // Calculate the max and min x position the paddle can go to remain inbounds
        rightEdge = totalGameWidth / 2f - paddleWidth / 2f;
        leftEdge = totalGameWidth / -2f + paddleWidth / 2f;
    }

    


}
