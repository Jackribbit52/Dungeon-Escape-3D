using UnityEngine;
using UnityEngine.SceneManagement;

public class restart : MonoBehaviour
{
    public void RestartLevel()
    {
        // Reloads the current scene
        SceneManager.LoadScene(0);
    }
}
