using UnityEngine;

public class TargetScript : MonoBehaviour
{
    public TaskManager taskManager; 
    public float speed = 1f;
    public float radius = 2f;

    private Vector3[] squarePoints;
    private int currentPoint = 0;

    private float angle = 0f; 
    private Vector3 startPosition;

   public GameObject bullseyeText; 

    void Start()
    {
        startPosition = transform.position;

        squarePoints = new Vector3[4];
        squarePoints[0] = startPosition + Vector3.left * radius;
        squarePoints[1] = squarePoints[0] + Vector3.up * radius;
        squarePoints[2] = squarePoints[1] + Vector3.right * radius * 2;
        squarePoints[3] = squarePoints[2] + Vector3.down * radius * 2;
    }

    void Update()
    {
        if (taskManager == null || string.IsNullOrEmpty(taskManager.movement))
            return;

        if (taskManager.movement == "linear")
        {
            MoveLinear();
            bullseyeText.SetActive(true);
            
        }
        else if (taskManager.movement == "sinusoidal")
        {
            MoveSinusoidal();
            bullseyeText.SetActive(true);
        }
    }

    void MoveLinear()
    {
        Vector3 targetPos = squarePoints[currentPoint];
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
        {
            currentPoint = (currentPoint + 1) % squarePoints.Length;
        }
    }

    void MoveSinusoidal()
    {
        angle += speed * Time.deltaTime;
        float y = Mathf.Sin(angle) * radius;
        float x = Mathf.Cos(angle) * radius;
        transform.position = startPosition + new Vector3(x, y, 0);
    }
}
