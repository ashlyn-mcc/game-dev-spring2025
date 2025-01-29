using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class gameManager : MonoBehaviour
{

    public buildinGenerator buildGen;
    public breakoutGenerator breakGen;

    private bool gameOver = false;

    public GameObject buildBall;
    public GameObject breakBall;

    public GameObject gameOverScreen;

    public TMP_Text winnerText;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log(breakGen.tempCount);
        if (breakGen.tempCount == 40){
            winnerText.text = "Breakout Player";
            Destroy(buildBall);
            Destroy(breakBall);
            gameOverScreen.SetActive(true);
        } else if (buildGen.tempCount == 40){
            winnerText.text = "Buildin Player";
            Destroy(buildBall);
            Destroy(breakBall);
            gameOverScreen.SetActive(true);
        }

        breakGen.tempCount = 0;
        buildGen.tempCount = 0;
    }
}
