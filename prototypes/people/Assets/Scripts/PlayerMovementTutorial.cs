using UnityEngine;

public class BasicPlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform orientation; 
    public GameObject projectilePrefab;
    public Transform shootingPoint;    
    public float projectileForce = 20f;

    private Rigidbody rb;
    private Vector3 moveDirection;

    public Transform cameraTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleMovementInput();

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ShootProjectile();
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    void HandleMovementInput()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        
        moveDirection.y = 0f;
    }

    void Move()
    {
        rb.MovePosition(rb.position + moveDirection.normalized * moveSpeed * Time.fixedDeltaTime);
    }

  void ShootProjectile()
    {
    Quaternion projectileRotation = Quaternion.LookRotation(cameraTransform.forward);

    GameObject projectile = Instantiate(projectilePrefab, shootingPoint.position, projectileRotation);

    Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

    projectileRb.linearVelocity = cameraTransform.forward.normalized * projectileForce;
    }


}
