using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelCount : MonoBehaviour
{
    public PlayerController controller;
    public TMP_Text levelCounter;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.endless)
        {
            levelCounter.text = "Solved:      " + controller.levelcounter;
        }
        if (!controller.endless)
        {
            levelCounter.text = "Levels left: " + controller.levelCount;
        }
    }
}
