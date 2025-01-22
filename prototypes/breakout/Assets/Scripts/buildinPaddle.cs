using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class buildinPaddle : MonoBehaviour
{

    // Speed of paddle
    private float stepSpeed = 0.1f;

    // The bounds the paddle can move
    private float rightEdge;
    private float leftEdge;

    // The total width of the game space
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

        // Adjust the x position of the paddle based on WASD
        if (Input.GetKey(KeyCode.D)){
            updatedX = transform.position.x + stepSpeed;
        } else if (Input.GetKey(KeyCode.A)){
            updatedX = transform.position.x - stepSpeed;
        }else {
            updatedX = transform.position.x;
        }

        // Ensure the paddle's x value stays within the bounds of the game
        updatedX = Math.Clamp(updatedX,leftEdge,rightEdge);


        // Update the paddle's position 
        transform.position = new Vector3(updatedX, transform.position.y, transform.position.z);
       
    }

    void calculateEdges(){

        // Get the width of the paddle
        paddleWidth = transform.localScale.x;

        // Use that to figure out how far to the left and right it can go to remain inbounds
        rightEdge = totalGameWidth / 2f - paddleWidth / 2f;
        leftEdge = totalGameWidth / -2f + paddleWidth / 2f;
    }

    


}
