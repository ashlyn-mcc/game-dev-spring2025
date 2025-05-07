using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float projectileForce = 10f;
    private TaskManager taskManager;

    void Start()
    {
        taskManager = FindObjectOfType<TaskManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullseye"))
        {
            if (taskManager != null)
            {
                taskManager.numBullseyes += 1;
                Debug.Log("Bullseye hit! Total Bullseyes: " + taskManager.numBullseyes);
            }
            Destroy(gameObject);
        }
    }
}
