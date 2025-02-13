using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq; 

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

    private bool[] claimed; 

    public Material darkMaterial;
    public Material mediumMaterial;
    public Material lightMaterial;
    public Material lightestMaterial;


    public int brokenCount = 0;
    
    public TMP_Text brokenText;

    public int tempCount = 0;

    public bool canAttack = false;

    public buildinGenerator buildGen;

    void Start()
    {

        // Create 2D array to store brick objects
        brickArray = new GameObject[numColumns, numRows];

        claimed = new bool[numRows];

        for (int i = 0; i < numRows; i++){
            claimed[i] = false;
        }

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
        
        int tempBrokenCount = 0;

         for (int i = 0; i < numRows; i++){

            for (int j = 0; j < numColumns; j++){

                if (brickArray[j,i].GetComponent<breakBrick>().broken){
                    brokenCount++;
                    tempBrokenCount++;
                }

                if (tempBrokenCount == 10){
                   reverseProgress(i);
                }
            }
            tempBrokenCount = 0;
          }
          
          tempCount = brokenCount;
          brokenText.text = brokenCount.ToString();
          brokenCount = 0;

    }

    void reverseProgress(int rowNum){

        Debug.Log("Entered reverse Progress");

        if (!claimed[rowNum]){
            claimed[rowNum] = true;
            buildGen.underAttack();
        }
    }
    // function that checks
    // 1. has the row been previously cleared? 
    // 2. if has, ignore.
    // 3. if has not, reverse five of the other player's blocks



    public void underAttack(){

        Debug.Log("Entered Under Attack");

        int[] columnCount = new int[numColumns];  

        for (int i = 0; i < numColumns; i++)
        {
            for (int j = 0; j < numRows; j++)
            {
                if (brickArray[i, j].GetComponent<breakBrick>().broken == true)
                {
                    columnCount[i]++;
                }
            }
        }

        Debug.Log("Column count" + columnCount.Length);



        var indexedColumnCount = columnCount
                                    .Select((value, index) => new { Index = index, Value = value })
                                    .ToArray();

        var sortedColumns = indexedColumnCount
                                .OrderByDescending(x => x.Value)
                                .Take(5) 
                                .ToArray();

        Debug.Log("Sorted Columns" + sortedColumns);

        int[] topFiveColumnIndices = sortedColumns.Select(x => x.Index).ToArray();

        for (int i = 0; i < topFiveColumnIndices.Length;i++){
            Debug.Log("#"+i+". "+topFiveColumnIndices[i]);
        }

        // go to each of the columns
        for (int i = 0; i < 5; i++)
        {
            Debug.Log("Entering column for loop");
            int columnIndex = topFiveColumnIndices[i];


            for (int j = 0; j < numRows; j++)
            {

                Debug.Log("Entering row for loop");

              if (brickArray[columnIndex,j].GetComponent<breakBrick>().broken){

                Debug.Log("Entering innermost if");

                brickArray[columnIndex,j].GetComponent<breakBrick>().broken = false;
                brickArray[columnIndex,j].GetComponent<MeshRenderer>().enabled = true;
                brickArray[columnIndex,j].GetComponent<BoxCollider>().enabled = true;
                break;

              }

            }
        }



    }

    
}
