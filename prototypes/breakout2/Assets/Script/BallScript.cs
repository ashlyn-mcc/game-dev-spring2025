using UnityEngine;

public class BallScript : MonoBehaviour
{
    [SerializeField]
    public Rigidbody rb;
    //Material variables
    public Material redMaterial;
    public Material orangeMaterial;
    public Material yellowMaterial;
    public Material greenMaterial;
    public Material blueMaterial;
    //Color variables
    private Color red;
    private Color orange;
    private Color yellow;
    private Color green;
    private Color blue;
    //GameObject variables
    private GameObject remove_ball_point_object;
    private Remove_Ball_point remove_Ball_Point;
    //Particle Object variables
    public GameObject splatterParticles;
    //base gameObject
    private GameObject baseObject;
    //sound part
    private GameObject soundObject;
    private Sound_Manager sound_Manager;
    

    void Start()
    {
        //The position of ball is reflected by the position of base
        baseObject = GameObject.Find("Base");
        float y = transform.position.y;
        transform.position = new Vector3(baseObject.transform.position.x, y,baseObject.transform.position.z);  

        //checking remove ball point object
        remove_ball_point_object = GameObject.Find("Remove_Ball_point");
        remove_Ball_Point = remove_ball_point_object.GetComponent<Remove_Ball_point>();
        int randomNumber = Random.Range(0, 2);
        if (randomNumber == 1){
            rb.linearVelocity = new Vector3(-5, -10, 0);
        }
        else{
            rb.linearVelocity = new Vector3(5, -10, 0);
        }

        //color
        red = redMaterial.color;
        orange = orangeMaterial.color;
        yellow = yellowMaterial.color;
        green = greenMaterial.color;
        blue = blueMaterial.color;

        //sound
        soundObject = GameObject.Find("Sound_Manager");
        sound_Manager = soundObject.GetComponent<Sound_Manager>();

    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("brick")){
            Color color = collision.gameObject.GetComponent<Renderer>().material.color;
            if(color == red)
            {
                rb.linearVelocity = new Vector3(-10,20,0);
                print("red");

            }
            else if(color == orange){
                rb.linearVelocity = new Vector3(9,-18,0);
                print("orange");
            }
            else if (color == yellow){
                rb.linearVelocity = new Vector3(-8,16,0);
                print("yellow");
            }
            else if(color == green){
                rb.linearVelocity = new Vector3(7,-14,0);
                print("green");
            }
            else{
                rb.linearVelocity = new Vector3(-6,12,0);
                print("blue");
            }
            
            if (remove_Ball_Point != null){
                remove_Ball_Point.ReceiveSignal();
            }
            ContactPoint contact = collision.contacts[0];
            GameObject particle = Instantiate(splatterParticles, contact.point, Quaternion.identity);
            Destroy(particle, 1.0f);
            Destroy(collision.gameObject);
            sound_Manager.explodeEffect();
        }
        else{
            sound_Manager.throwEffect();
        }
        if(collision.gameObject.CompareTag("wall")){
            rb.linearVelocity = new Vector3(5,-10,0);
            sound_Manager.throwEffect();
        }
    }
}
