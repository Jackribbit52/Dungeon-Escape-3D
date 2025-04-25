using UnityEngine;

public class ChestInteraction : MonoBehaviour
{
    public CountdownTimer countdown; // Reference to your timer script
    [SerializeField] private GameObject greenBurstEffect;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Chest"))
        {
            int timeChange = Random.Range(5, 11); // 5 to 10 seconds
            bool addTime = Random.value > 0.5f;

            if (addTime)
            {
                countdown.AddTime(timeChange);
                Debug.Log("Chest bonus! +" + timeChange + " seconds");
            }
            else
            {
                countdown.RemoveTime(timeChange);
                Debug.Log("Chest penalty! -" + timeChange + " seconds");
            }

            // Optional: destroy or disable chest after use
            Destroy(other.gameObject);
        }
    }
}
