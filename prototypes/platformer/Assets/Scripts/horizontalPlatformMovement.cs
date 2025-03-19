using UnityEngine;

public class HorizontalPlatformMovement : MonoBehaviour
{
    [SerializeField] 
    private float moveAmount = 3f; 

    [SerializeField] 
    private float speed = 2f; 

    private Vector3 startPos;
    private Vector3 targetPos;
    
    private bool movingLeft = true;

    void Start()
    {
        startPos = transform.position;
        targetPos = startPos - new Vector3(moveAmount, 0, 0);
    }

    void Update()
    {
        if (movingLeft)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPos) < 0.01f)
            {
                movingLeft = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, startPos) < 0.01f)
            {
                movingLeft = true;
            }
        }
    }
}
