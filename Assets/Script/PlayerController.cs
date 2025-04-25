using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;

    private Rigidbody rb;
    private float moveInput;
    private float rotateInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Store input here
        moveInput = Input.GetAxis("Vertical");
        rotateInput = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        float targetYRotation = rotateInput * rotationSpeed * Time.fixedDeltaTime;
        if (Mathf.Abs(targetYRotation) > 0.01f)
        {
            Quaternion deltaRotation = Quaternion.Euler(0f, targetYRotation, 0f);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }

        // Move forward/backward
        Vector3 moveVector = transform.forward * moveInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + moveVector);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Stairs"))
        {
            SceneManager.LoadScene(3);
        }
    }
}

