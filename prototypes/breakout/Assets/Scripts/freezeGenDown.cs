using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class freezeGenDown : MonoBehaviour
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
            ballBuild.frozen = true;

            StartCoroutine(TempFreeze());
        }

    }

    void setTimer(){
       timeForNextPowerUp = Random.Range(15f, 30f);
    }

    void createPowerUp(){

        Vector3 pos = new Vector3(getDropPos(),-0.62f,-0.28f);
        GameObject obj = Instantiate(powerUpPrefab, pos, Quaternion.Euler(270, 0, 0));
        obj.GetComponent<freezeDown>().goingUp = direction;
        obj.GetComponent<freezeDown>().generator = this;
    }

    float getDropPos(){
        return Random.Range(-5.3f, 5.3f);
    }

    IEnumerator TempFreeze(){
        
        yield return new WaitForSeconds(5f); 
        ballBuild.frozen = false;
    }

}
