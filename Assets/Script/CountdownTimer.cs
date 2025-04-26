using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    public float totalTime;
    public TMP_Text timerText;

    private static CountdownTimer instance;
    private float currentTime;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // destroy duplicate
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        currentTime = totalTime;
        UpdateTimerText();
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
            Debug.Log("Time's up!");
            timerText.text = "You Lose!!!";
            AddTime(120);
            SceneManager.LoadScene(2);
        }
    }

    void UpdateTimerText()
    {
        if (timerText == null) return;

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
        currentTime = Mathf.Max(currentTime - seconds, 0);
    }

    public void SetTimerTextReference(TMP_Text newText)
    {
        timerText = newText;
        UpdateTimerText();
    }
}
