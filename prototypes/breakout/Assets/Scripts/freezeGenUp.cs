using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class freezeGenUp : MonoBehaviour
{

    private float timeForNextPowerUp;

    private float timeElapsed = 0;

    public GameObject powerUpPrefab;

    public bool direction;

    public bool freezeBreak = false;
    public bool freezeBuild = false;

    public buildinBall ballBuild;
    public breakoutBall ballBreak;


    // Start is called before the first frame update
    void Start()
    {
       
        setTimer();

    }

    // Update is called once per frame
    void Update()
    {

        timeElapsed += Time.deltaTime;
        
        if (timeElapsed > timeForNextPowerUp){
            timeElapsed = 0;
            setTimer();
            createPowerUp();
        }

        if (freezeBuild){
            freezeBuild = false;

            // indicate to the ball it is frozen
            ballBreak.frozen = true;

            StartCoroutine(TempFreeze());
        }

    }

    void setTimer(){
       timeForNextPowerUp = Random.Range(15f, 30f);
    }

    void createPowerUp(){

        Vector3 pos = new Vector3(getDropPos(),1.4f,-0.28f);
        GameObject obj = Instantiate(powerUpPrefab, pos, Quaternion.Euler(270, 0, 0));
        obj.GetComponent<freezeUp>().goingUp = direction;
        obj.GetComponent<freezeUp>().generator = this;
    }

    float getDropPos(){
        return Random.Range(-5.3f, 5.3f);
    }

    IEnumerator TempFreeze(){
        
        yield return new WaitForSeconds(5f); 
        ballBreak.frozen = false;
    }

}
