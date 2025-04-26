using UnityEngine;

public class MinimapCameraFollow : MonoBehaviour
{
    public Transform player; // Assign the player here
    public float height = 30f; // Height above the player

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 newPosition = player.position;
            newPosition.y += height; // Set the height
            transform.position = newPosition;

            transform.rotation = Quaternion.Euler(90f, 0f, 0f); // Look straight down
        }
    }
}

