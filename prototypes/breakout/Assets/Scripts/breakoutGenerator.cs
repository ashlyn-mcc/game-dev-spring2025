using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class breakoutGenerator : MonoBehaviour
{

    // Number of brick columns and rows
    private int numColumns = 10;
    private int numRows = 4;

    // Width and height of each brick
    private float brickWidth = 1.1f;
    private float brickHeight = 0.5f;

    public GameObject brickPrefab;

    private GameObject[,] brickArray; 

    public Material darkMaterial;
    public Material mediumMaterial;
    public Material lightMaterial;
    public Material lightestMaterial;


    public int brokenCount = 0;
    
    public TMP_Text brokenText;

    public int tempCount = 0;

    void Start()
    {

        // Create 2D array to store brick objects
        brickArray = new GameObject[numColumns, numRows];

        for (int i = 0; i < numColumns; i++){
            for (int j = 0; j < numRows; j++){

                // Calculate the position of each brick
                Vector3 brickPos = transform.position + new Vector3(i * brickWidth, j * -brickHeight, 0);

                // Instantiate each brick 
                brickArray[i,j] = Instantiate(brickPrefab, brickPos, Quaternion.identity);

                // get the renderer of the brick and assign it the row color
                Renderer renderer =  brickArray[i,j].GetComponent<Renderer>();

                if (j == 0){
                    renderer.material = darkMaterial; 
                } else if (j == 1){
                    renderer.material = mediumMaterial; 
                } else if (j == 2){
                    renderer.material = lightMaterial; 
                } else {
                    renderer.material = lightestMaterial; 
                }


            }
        }
    }

    void Update()
    {
        
         for (int i = 0; i < numColumns; i++){
            for (int j = 0; j < numRows; j++){
                if (brickArray[i,j].GetComponent<breakBrick>().broken){
                    brokenCount++;
                }
            }
          }
          
          tempCount = brokenCount;
          brokenText.text = brokenCount.ToString();
          brokenCount = 0;

    }
}
