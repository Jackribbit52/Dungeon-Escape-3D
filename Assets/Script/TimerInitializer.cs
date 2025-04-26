using UnityEngine;
using TMPro;

public class TimerInitializer : MonoBehaviour
{
    void Start()
    {
        TMP_Text sceneTimerText = GameObject.FindWithTag("TimerText")?.GetComponent<TMP_Text>();

        if (sceneTimerText != null)
        {
            CountdownTimer timer = Object.FindFirstObjectByType<CountdownTimer>();
            if (timer != null)
            {
                timer.SetTimerTextReference(sceneTimerText);
            }
        }
    }
}

