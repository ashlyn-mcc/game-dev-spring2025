using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breakoutGenerator : MonoBehaviour
{

    private int numColumns = 10;
    private int numRows = 4;

    private float brickWidth = 1.1f;
    private float brickHeight = 0.5f;

    public GameObject brickPrefab;

    private GameObject[,] brickArray; 

    public Material darkMaterial;
    public Material mediumMaterial;
    public Material lightMaterial;
    public Material lightestMaterial;

    // Start is called before the first frame update
    void Start()
    {

        brickArray = new GameObject[numColumns, numRows];

        for (int i = 0; i < numColumns; i++){
            for (int j = 0; j < numRows; j++){
                Vector3 brickPos = transform.position + new Vector3(i * brickWidth, j * -brickHeight, 0);
                brickArray[i,j] = Instantiate(brickPrefab, brickPos, Quaternion.identity);

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
