using UnityEngine;
using TMPro; 

public class TaskManager : MonoBehaviour
{
    public bool givenTrophy = false;
    public bool hitBullseyes = false;
    public string movement = "";
    public bool startMoving = true;
    public int numBullseyes = 0;
    public int numTrophies = 0;
    public GameObject door;
    private bool dontReturn = false;

    public TMP_Text bullseyeText; 
     public TMP_Text trophyText;  


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
   
        if (numBullseyes >= 3)
        {
            movement = "";
            hitBullseyes = true;
        }

        if (bullseyeText != null)
        {
            bullseyeText.text = "Bullseyes Hit: " + numBullseyes.ToString();
        }

        if (givenTrophy && !dontReturn){
            trophyText.text = "Trophies: 1";
            numTrophies += 1;
            dontReturn = true;
        }

        if (numTrophies >= 1){
            door.SetActive(false);
        }
    }
}
