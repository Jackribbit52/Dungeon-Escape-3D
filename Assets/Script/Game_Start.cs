using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Start : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("callScene", 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void callScene()
    {
        SceneManager.LoadScene(1);
    }
}
