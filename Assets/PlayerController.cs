
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float smoothSpeed = 10f;
    public float rotationSpeed = 12f;

    private Vector3 targetDirection = Vector3.zero;
    private Rigidbody rb;

    [Header("Swipe Input")]
    private Vector2 startTouch;
    private Vector2 endTouch;
    private bool swipeDetected;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true;
    }

    void Update()
    {
        HandleKeyboardInput();
        DetectSwipe();
    }

    void FixedUpdate()
    {
        MovePlayer();
        RotatePlayer();
    }

    // -------------------------
    // KEYBOARD INPUT (Editor Test)
    // -------------------------
    void HandleKeyboardInput()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            targetDirection = Vector3.forward;

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            targetDirection = Vector3.back;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            targetDirection = Vector3.left;

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            targetDirection = Vector3.right;

        // Stop movement when no keys held
        if (!Input.GetKey(KeyCode.W) &&
            !Input.GetKey(KeyCode.S) &&
            !Input.GetKey(KeyCode.A) &&
            !Input.GetKey(KeyCode.D) &&
            !Input.GetKey(KeyCode.UpArrow) &&
            !Input.GetKey(KeyCode.DownArrow) &&
            !Input.GetKey(KeyCode.LeftArrow) &&
            !Input.GetKey(KeyCode.RightArrow))
        {
            targetDirection = Vector3.zero;
        }
    }

    // -------------------------
    // TOUCH + MOUSE SWIPE INPUT
    // -------------------------
    void DetectSwipe()
    {
        // Mouse for Mac / PC testing
        if (Input.GetMouseButtonDown(0))
        {
            startTouch = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            endTouch = Input.mousePosition;
            swipeDetected = true;
        }

        // Mobile touch
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
                startTouch = touch.position;

            if (touch.phase == TouchPhase.Ended)
            {
                endTouch = touch.position;
                swipeDetected = true;
            }
        }
    }

    // -------------------------
    // MOVEMENT
    // -------------------------
    void MovePlayer()
    {
        if (swipeDetected)
        {
            Vector2 swipe = endTouch - startTouch;

            if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
                targetDirection = swipe.x > 0 ? Vector3.right : Vector3.left;
            else
                targetDirection = swipe.y > 0 ? Vector3.forward : Vector3.back;

            swipeDetected = false;
        }

        Vector3 targetVelocity = targetDirection.normalized * moveSpeed;

        rb.velocity = Vector3.Lerp(
            rb.velocity,
            targetVelocity,
            Time.fixedDeltaTime * smoothSpeed
        );
    }

    // -------------------------
    // ROTATE TOWARD MOVEMENT
    // -------------------------
    void RotatePlayer()
    {
        if (targetDirection != Vector3.zero)
        {
            Quaternion targetRot =
                Quaternion.LookRotation(targetDirection);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                Time.fixedDeltaTime * rotationSpeed
            );
        }
    }
}
