using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;

    private static PlayerController instance;
    private Rigidbody rb;
    private float moveInput;
    private float rotateInput;

    [SerializeField]
    private bool endless;
    public int levelCount;

    void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Destroy duplicate
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

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
            Destroy(collision.gameObject);
            if (endless)
            {
                SceneManager.LoadScene(1); // Reload the level for endless mode
            }
            else if (levelCount > 1)
            {
                levelCount--;
                SceneManager.LoadScene(1); // Reload with 1 fewer level to go
            }
            else
            {
                SceneManager.LoadScene(3); // Final win screen or game over
            }
        }
    }

}

