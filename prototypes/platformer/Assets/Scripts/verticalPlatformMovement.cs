using UnityEngine;

public class verticalPlatformMovement : MonoBehaviour
{
    [SerializeField] 
    private float moveAmount = 3f;

    [SerializeField] 
    private float speed = 2f;

    private Vector3 startPos;
    private Vector3 targetPos;
    private bool movingUp = true;

    void Start()
    {
        startPos = transform.position;
        targetPos = startPos + new Vector3(0, moveAmount, 0);
    }

    void Update()
    {
        if (movingUp)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPos) < 0.01f)
            {
                movingUp = false;
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPos, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, startPos) < 0.01f)
            {
                movingUp = true;
            }
        }
    }
}
