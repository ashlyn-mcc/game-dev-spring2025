using UnityEngine;

public class InkNPC : MonoBehaviour
{
    public string knotToLaunch = "TalkToNPC1";
    private bool playerInRange = false;
    private InkStoryManager inkManager;

    void Start()
    {
        inkManager = FindObjectOfType<InkStoryManager>();
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.T))
        {
            if (!inkManager.knotActive)
            {
                inkManager.TalkToCharacter(knotToLaunch);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("In range");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Out of range");
        }
    }
}
