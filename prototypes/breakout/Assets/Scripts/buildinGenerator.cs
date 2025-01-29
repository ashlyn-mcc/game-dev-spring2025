using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class buildinGenerator : MonoBehaviour
{

    // Number of columns and rows of brick
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

    public int builtCount = 0;
    
    public TMP_Text builtText;

    public int tempCount = 0;
    void Start()
    {

        // Create the 2D array of bricks
        brickArray = new GameObject[numColumns, numRows];

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

          for (int i = 0; i < numColumns; i++){
            for (int j = 0; j < numRows; j++){
                if (!brickArray[i,j].GetComponent<buildBrick>().broken){
                    builtCount++;
                }
            }
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
}
