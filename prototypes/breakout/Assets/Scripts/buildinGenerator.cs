using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq; 

public class buildinGenerator : MonoBehaviour
{

    // Number of columns and rows of brick
    private int numColumns = 10;
    private int numRows = 4;

    // Width and height of each brick
    private float brickWidth = 1.1f;
    private float brickHeight = 0.5f;

    public GameObject brickPrefab;

    private bool[] claimed; 

    private GameObject[,] brickArray; 

    public Material darkMaterial;
    public Material mediumMaterial;
    public Material lightMaterial;
    public Material lightestMaterial;

    public int builtCount = 0;
    
    public TMP_Text builtText;

    public int tempCount = 0;

    public breakoutGenerator breakGen;

    void Start()
    {

        // Create the 2D array of bricks
        brickArray = new GameObject[numColumns, numRows];

         claimed = new bool[numRows];

        for (int i = 0; i < numRows; i++){
            claimed[i] = false;
        }

        for (int i = 0; i < numColumns; i++){
            for (int j = 0; j < numRows; j++){

                // Calculate the position of each brick based on width and height
                Vector3 brickPos = transform.position + new Vector3(i * brickWidth, j * brickHeight, 0);

                // Instantiatw the brick
                brickArray[i,j] = Instantiate(brickPrefab, brickPos, Quaternion.identity);

                // Pass it it's place in the 2D array and a reference to this script
                brickArray[i,j].GetComponent<buildBrick>().column = i;
                brickArray[i,j].GetComponent<buildBrick>().row = j;
                brickArray[i,j].GetComponent<buildBrick>().generator = this;

                // Get the brick's renderer and give it the row material color
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

        int tempBuiltCount = 0;

          for (int i = 0; i < numRows; i++){
            for (int j = 0; j < numColumns; j++){
                if (!brickArray[j,i].GetComponent<buildBrick>().broken){
                    builtCount++;
                    tempBuiltCount++;
                }

                 if (tempBuiltCount == 10){
                   reverseProgress(i);
                }
            }
              tempBuiltCount = 0;
          }

          tempCount = builtCount;
          builtText.text = builtCount.ToString();
          builtCount = 0;
        
    }

    // Checks if there are bricks beneath a collided brick that haven't been built yet.
    public bool areBricksBuiltBeneath(int row, int column){
        
        // Go through the bricks beneath a brick
        for (int i = row-1; i >= 0; i--){

            // See if they are broken
            if (brickArray[column,i].GetComponent<buildBrick>().broken){
                return false;
            }
        }
        return true;
    }


    void reverseProgress(int rowNum){

        Debug.Log("Entered reverse Progress");

        if (!claimed[rowNum]){
            claimed[rowNum] = true;
            breakGen.underAttack();
        }
    }

    // function that will find built bricks.
    // break the lowest one's possible in five rows

    public void underAttack(){

        Debug.Log("Entered Under Attack");

        int[] columnCount = new int[numColumns];  

        for (int i = 0; i < numColumns; i++)
        {
            for (int j = 0; j < numRows; j++)
            {
                if (brickArray[i, j].GetComponent<buildBrick>().broken == false)
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


            for (int j = numRows-1; j >= 0; j--)
            {

                Debug.Log("Entering row for loop");

              if (!brickArray[columnIndex,j].GetComponent<buildBrick>().broken){

                Debug.Log("Entering innermost if");

                brickArray[columnIndex,j].GetComponent<buildBrick>().broken = true;
                brickArray[columnIndex,j].GetComponent<MeshRenderer>().enabled = false;
                brickArray[columnIndex,j].GetComponent<Collider>().isTrigger = true;
                break;

              }

            }
        }



    }

    
}
