
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]

    public float moveSpeed = 8f;
    public float smoothSpeed = 10f;
    public float rotationSpeed = 12f;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true;
    }

    void Update()
    {
     //   HandleKeyboardInput();

    }
    void FixedUpdate()
    {
        MovePlayer();
    }


    void MovePlayer()
    {
        float h= Input.GetAxis("Horizontal");
        float v= Input.GetAxis("Vertical");

       Vector3 move = new Vector3(h, 0, v);

        rb.MovePosition(rb.position + move * moveSpeed * Time.fixedDeltaTime);

    }
}
