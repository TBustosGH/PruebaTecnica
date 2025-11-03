using UnityEngine;

public class playerController : MonoBehaviour
{
    [Header("Movement")]
    public float playerSpeed;
    private float horizontalMovement, verticalMovement;
    private Vector3 movement;

    [Header("Jump")]
    public float playerJumpForce;
    private bool onGround, startJump;
    public float timeSinceStartJump;

    [Header("Rotation")]
    public Vector2 rotation;
    public float sensivilityX, sensivilityY;
    public Transform cameraObject;

    private Rigidbody rb;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        //Movement Input
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");
        //Rotation Input
        rotation.y += Input.GetAxisRaw("Mouse Y") * sensivilityY;
        rotation.x += Input.GetAxisRaw("Mouse X") * sensivilityX;
        
        //Jump Input && Start Jump Bool
        if (Input.GetKeyDown(KeyCode.Space))
            startJump = true;

        if (startJump)
        {
            timeSinceStartJump += Time.deltaTime;

            if (timeSinceStartJump >= 0.2f)
            {
                startJump = false;
                timeSinceStartJump = 0;
            }
        }
            
    }
    private void FixedUpdate()
    {
        PlayerMovement();
        PlayerJump();
        RotatePlayer();
    }

    //Movement Functions
    private void PlayerMovement()
    {
        movement = new Vector3(horizontalMovement, 0, verticalMovement);

        if (Input.GetKey(KeyCode.LeftShift))
            transform.Translate(movement * (playerSpeed * 2f) * Time.deltaTime);
        else if (Input.GetKey(KeyCode.LeftControl))
            transform.Translate(movement * (playerSpeed * 0.5f) * Time.deltaTime);
        else
            transform.Translate(movement * playerSpeed * Time.deltaTime);
    }
    //Jump Functions
    private void PlayerJump()
    {
        if (startJump && onGround)
        {
            onGround = false;
            startJump = false;
            timeSinceStartJump = 0;
            rb.AddForce(Vector3.up * playerJumpForce * Time.deltaTime, ForceMode.Impulse);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
            onGround = true;
        else
            onGround = false;
    }
    //Rotation Functions
    private void RotatePlayer()
    {
        rotation.y = Mathf.Clamp(rotation.y, -40, 40);
        transform.localRotation = Quaternion.Euler(0, rotation.x, 0);
        cameraObject.localRotation = Quaternion.Euler(-rotation.y, 0, 0);
    }
}
