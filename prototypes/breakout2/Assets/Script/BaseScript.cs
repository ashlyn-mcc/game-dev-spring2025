using UnityEngine;

public class script : MonoBehaviour
{
    private int ballSpeed = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
       // if left is pressed, move left
       if(Input.GetKey(KeyCode.LeftArrow))
        {
            if (transform.position.x <= - 19.0f)
                return;
            Vector3 newPosition = transform.position;
            newPosition.x -= Time.deltaTime * ballSpeed;
            transform.position = newPosition;
        }
       // if right is pressed, move right
        if(Input.GetKey(KeyCode.RightArrow))
        {
            if (transform.position.x >= 19.0f)
                return;
            Vector3 newPosition = transform.position;
            newPosition.x += Time.deltaTime * ballSpeed; 
            transform.position = newPosition;
        }
    }
}
