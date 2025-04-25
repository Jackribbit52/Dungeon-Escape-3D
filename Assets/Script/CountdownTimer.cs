using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Required for TextMeshPro

public class CountdownTimer : MonoBehaviour
{
    public float totalTime; // Total time in seconds
    public TMP_Text timerText; // Assign this in the Inspector
    private float currentTime;

    void Start()
    {
        currentTime = totalTime;
    }

    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerText();
        }
        else
        {
            // Timer finished, add your logic here
            Debug.Log("Time's up!");
            timerText.text = "You Lose!!!"; // Or display "Time's up!" or any other message
            SceneManager.LoadScene(2);
        }


    }

    void UpdateTimerText()
    {
        // Format the time as minutes:seconds (e.g., 01:30)
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void AddTime(int seconds)
    {
        currentTime += seconds;
    }

    public void RemoveTime(int seconds)
    {
        currentTime -= seconds;
        currentTime = Mathf.Max(currentTime, 0); // prevent negative time
    }
}
