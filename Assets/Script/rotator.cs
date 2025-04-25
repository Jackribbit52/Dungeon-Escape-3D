using UnityEngine;

public class rotator : MonoBehaviour
{
    public float rotationSpeed = 30f; // degrees per second

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }


        // Update is called once per frame
        void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime, Space.Self);

    }
}
