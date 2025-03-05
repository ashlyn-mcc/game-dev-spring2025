using UnityEngine;
using TMPro; 

public class PlatformerPlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject cam;
    [SerializeField]
    private TextMeshProUGUI gemsText;

    private CharacterController cc;
    private Vector3 velocity = Vector3.zero;
    private float yVelocity = 0;
    private float horizontalmoveSpeed = 8;
    private float jumpForce = 8;
    private float gravity = -18f;
    private int jumpCount = 0;
    private Transform platform;
    private Vector3 platformPreviousPos;
    private int gemsCollected = 0;
    private Vector3 startPosition; 

    void Start()
    {
        cc = GetComponent<CharacterController>();
        startPosition = transform.position; 
    }

    void Update()
    {
        if (cc.isGrounded)
        {
            yVelocity = -2;
            jumpCount = 0;
        }
        else
        {
            yVelocity += gravity * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < 2)
        {
            jumpCount++;
            yVelocity = jumpForce;
            transform.SetParent(null);
        }

        float hAxis = Input.GetAxis("Horizontal");

        velocity = Vector3.zero;

        Vector3 adjustedCamRight = cam.transform.right;
        adjustedCamRight.y = 0;
        adjustedCamRight.Normalize();
        velocity += adjustedCamRight * hAxis * horizontalmoveSpeed;

        velocity.y = yVelocity;

        if (platform != null)
        {
            Vector3 platformMovement = platform.position - platformPreviousPos;
            cc.Move(platformMovement);
        }

        velocity = Vector3.ClampMagnitude(velocity, 10);
        cc.Move(velocity * Time.deltaTime);
        platformPreviousPos = platform != null ? platform.position : platformPreviousPos;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("water"))
        {
            horizontalmoveSpeed = 4;
            jumpForce = 4;
        }

        if (collision.CompareTag("horizontal") || collision.CompareTag("vertical"))
        {
            platform = collision.transform;
            platformPreviousPos = platform.position;
            transform.SetParent(platform);
        }

        if (collision.CompareTag("collectable"))
        {
            gemsCollected++;
            gemsText.text = gemsCollected + " ";
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("lava"))
        {
            ResetPlayerPosition();
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("water"))
        {
            horizontalmoveSpeed = 8;
            jumpForce = 8;
        }

        if (collision.CompareTag("horizontal") || collision.CompareTag("vertical"))
        {
            transform.SetParent(null);
            platform = null;
        }
    }

    void ResetPlayerPosition()
    {
        transform.SetParent(null); 
        cc.enabled = false;
        transform.position = startPosition;
        cc.enabled = true; 
    }
}
