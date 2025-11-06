using System.Collections;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [Header("Movement")]
    public float playerSpeed;
    public float stamina = 100;
    private float staminaDelay;
    private float horizontalMovement, verticalMovement;
    private Vector3 movement;

    [Header("Jump")]
    public float playerJumpForce;
    private bool onGround, startJump;
    private float timeSinceStartJump;

    [Header("Rotation")]
    public float sensivilityX;
    public float sensivilityY;
    private Vector2 rotation;
    public Transform cameraObject;

    [Header("Life System")]
    public float playerHealtPoints;
    public bool isPlayerAlive = true, isBeingAttacked;
    public float chillingTime;

    [Header("Interact")]
    public GameObject interactBox;
    private bool startInteracting;
    private float timeSinceInteracting;

    [Header("Melee Attack")]
    public int playerDamage;
    public GameObject meleeAttack;
    private bool startMeleeAttack;
    public float timeSinceMeleeAttack;

    private Rigidbody rb;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if (isPlayerAlive)
        {
            InputFunction();
            StaminUp();
            StopBeingAttacked();
            LifeRegenerate();
        }
    }
    private void FixedUpdate()
    {
        if (isPlayerAlive)
        {
            PlayerMovement();
            PlayerJump();
            RotatePlayer();
            InteractFunction();
            StartMeleeAttack();
        } 
    }

    //Input Function
    private void InputFunction()
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
        //Interaction Input
        if (Input.GetKeyDown(KeyCode.E))
            startInteracting = true;
        //Todo este apartado se lo puede manejar con animaciones (aún no tengo las animaciones)
        //#################################################
        if (startInteracting)
            timeSinceInteracting += Time.deltaTime;
        if (timeSinceInteracting >= 0.3f)
        {
            timeSinceInteracting = 0;
            StopInteracting();
        }
        //#################################################
        //Attack Input
        if(Input.GetKeyDown(KeyCode.Mouse0))
            startMeleeAttack = true;
        //Todo este apartado se lo puede manejar con animaciones
        //#################################################
        if (startMeleeAttack)
            timeSinceMeleeAttack += Time.deltaTime;
        if (timeSinceMeleeAttack >= 0.5f)
        {
            timeSinceMeleeAttack = 0;
            StopMeleeAttack();
        }
        //#################################################
    }
    //Movement Functions
    private void PlayerMovement()
    {
        movement = new Vector3(horizontalMovement, 0, verticalMovement);

        if (stamina > 0f && movement != Vector3.zero)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                transform.Translate(movement * (playerSpeed * 2f) * Time.deltaTime);
                stamina -= 0.3f;
                staminaDelay = 0;
            }
            else
            {
                stamina -= 0.1f;
                transform.Translate(movement * playerSpeed * Time.deltaTime);
                staminaDelay = 0;
            }
        }
        else if (stamina <= 0f && movement != Vector3.zero)
        {
            staminaDelay = 0;
            transform.Translate(movement * (playerSpeed * 0.75f) * Time.deltaTime);
        }      
    }
    private void StaminUp()
    {
        stamina = Mathf.Clamp(stamina, 0f, 100f);
        staminaDelay = Mathf.Clamp(staminaDelay, 0f, 0.750f);
        if (movement == Vector3.zero && onGround)
            staminaDelay += Time.deltaTime;

        if (staminaDelay >= 0.750f)
        {
            stamina += Time.deltaTime * 35;
        }
    }
    //Jump Functions
    private void PlayerJump()
    {
        if (startJump && onGround)
        {
            staminaDelay = 0;
            stamina -= 15;
            onGround = false;
            startJump = false;
            timeSinceStartJump = 0;
            rb.AddForce(Vector3.up * playerJumpForce * Time.deltaTime, ForceMode.Impulse);
        }
    }
    //Rotation Functions
    private void RotatePlayer()
    {
        rotation.y = Mathf.Clamp(rotation.y, -40, 40);
        transform.localRotation = Quaternion.Euler(0, rotation.x, 0);
        cameraObject.localRotation = Quaternion.Euler(-rotation.y, 0, 0);
    }
    //Interaction Functions
    public void InteractFunction()
    {
        if (startInteracting && onGround)
        {
            interactBox.SetActive(true);
        }
    }
    public void StopInteracting()
    {
        startInteracting = false;
        interactBox.SetActive(false);
    }
    //Life System Functions
    private void LifeRegenerate()
    {
        playerHealtPoints = Mathf.Clamp(playerHealtPoints, 0, 100);
        chillingTime = Mathf.Clamp(chillingTime, 0, 5);

        if (playerHealtPoints <= 0 || playerHealtPoints >= 100) return;

        if (!isBeingAttacked)
            chillingTime += Time.deltaTime;

        if (chillingTime >= 5 && !isBeingAttacked)
        {
            playerHealtPoints += Time.deltaTime * 15f;
        }
    }
    //Attack Functions
    private void StartMeleeAttack()
    {
        if (startMeleeAttack && onGround)
        {
            meleeAttack.SetActive(true);
        }
    }
    private void StopMeleeAttack()
    {
        meleeAttack.SetActive(false);
        startMeleeAttack = false;
    }
    public void BeingAttacked(int damage)
    {
        if (!isBeingAttacked)
        {
            if (playerHealtPoints > damage)
            {
                playerHealtPoints -= damage;
                chillingTime = 0;
                isBeingAttacked = true;
            }
            else
            {
                playerHealtPoints = 0;
                isPlayerAlive = false;
                isBeingAttacked = false;
            }
        }
    }
    public void StopBeingAttacked()
    {
        if (isBeingAttacked && playerHealtPoints > 0)
        {
            chillingTime += Time.deltaTime ;
            if (chillingTime >= 0.2f)
            {
                chillingTime = 0;
                isBeingAttacked = false;
            }
        }
    }

    //Other Functions
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
            onGround = true;
    }
}
