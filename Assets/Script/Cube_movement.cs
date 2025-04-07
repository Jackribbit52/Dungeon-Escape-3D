using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cube_movement : MonoBehaviour
{
    public float speed;
    public float rotation_speed;
    public Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            rb.AddForce(transform.forward * speed);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            rb.AddForce(transform.forward * speed * -1);
        }
        gameObject.transform.Rotate(0, Input.GetAxis("Horizontal") * rotation_speed * Time.deltaTime, 0);
    }
}
