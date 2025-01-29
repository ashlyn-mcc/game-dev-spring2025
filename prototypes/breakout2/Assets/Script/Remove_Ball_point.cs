using UnityEngine;

public class Remove_Ball_point : MonoBehaviour
{
    [SerializeField]
    public GameObject ball;
    private bool ballActive;
    private int ball_count = 5;
    private int point = 0;
    private int max_point_count = 45;
    private bool gameEnd;    
    void Start(){
        ballActive = true;
        gameEnd = false;
    }
    void Update(){
        if(Input.GetKeyDown(KeyCode.Space)){
            if(ballActive == false && ball_count > 0 && !(gameEnd)){
                ballActive = true;
                GameObject instance = Instantiate(ball, new Vector3(0,-2.61f,0),Quaternion.identity);
            }
        }
        
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("ball")){
            Destroy(other.gameObject);
            ballActive = false;
            ball_count--;
        }
    }
    
    void OnGUI(){
        GUIStyle labelStyle = new GUIStyle();
        labelStyle.fontSize = 50;

        if(point <max_point_count){
            if(ball_count>0){
                GUI.Label(new Rect(0,0,100,200),"Ball: "+ball_count+"",labelStyle);
            }
            else{
                GUI.Label(new Rect(0,0,100,200),"Died",labelStyle);
                GUI.Label(new Rect(Screen.width/2-150,40,100,200),"Game Over",labelStyle);
                gameEnd = true;
            }

            if(ballActive == false && ball_count>0){
                GUI.Label(new Rect(Screen.width/2-150,40,100,200),"Press Space Bar",labelStyle);
            }
            else {
                GUI.Label(new Rect(Screen.width/2-150,40,100,200),"",labelStyle);
            }   
        }
        else{
            GUI.Label(new Rect(0,0,100,200),"Mission Clear!",labelStyle);
            GUI.Label(new Rect(Screen.width/2-150,40,100,200),"Success",labelStyle);
            gameEnd = true;
        }
       
        GUI.Label(new Rect(0,80,100,200),"Point: "+point+"",labelStyle);
    }

    public void ReceiveSignal(){
        point++;
    }
}
