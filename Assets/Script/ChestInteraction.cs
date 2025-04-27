using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.SceneManagement;

public class ChestInteraction : MonoBehaviour
{
    public CountdownTimer countdown;
    public List<System.Action> chestEffects; // List of effects (random actions)
    public Image blackScreenOverlay;
    public PlayerController playerController;
    [SerializeField]
    private EffectPopupUI popupUI;
    [SerializeField] 
    private GameObject minimapUI;


    void Start()
    {
        InitializeChestEffects();
        SceneManager.sceneLoaded += OnSceneLoaded;
        minimapUI.SetActive(false);

        // Always try finding UI on start
        if (popupUI == null)
        {
            popupUI = FindFirstObjectByType<EffectPopupUI>();
            if (popupUI == null)
            {
                Debug.LogWarning("Popup UI could not be found on Start!");
            }
        }
    }



    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Always unsubscribe to prevent memory leaks
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded: " + scene.name);

        countdown = FindFirstObjectByType<CountdownTimer>();
        blackScreenOverlay = FindFirstObjectByType<Image>(); // If this is unique, otherwise be more specific
        playerController = FindFirstObjectByType<PlayerController>();

        

        if (popupUI == null)
        {
            popupUI = FindFirstObjectByType<EffectPopupUI>(); // Find the popup UI in the scene
            if (popupUI == null)
            {
                Debug.LogWarning("Popup UI could not be found after scene reload!");
            }
        }
        else
        {
            Debug.Log("Popup UI successfully found after scene reload.");
        }
    }


    void InitializeChestEffects()
    {
        chestEffects = new List<System.Action>
        {
            AddTimeEffect,
            RemoveTimeEffect,
            BlackoutScreenEffect,
            DoubleSpeedEffect,
            HalfSpeedEffect,
            FreezeEffect,
            MinimapEffect
        };
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Chest"))
        {
            // Select a random effect from the list
            int randomEffectIndex = Random.Range(0, chestEffects.Count);
            chestEffects[randomEffectIndex].Invoke(); // Call the selected effect
            Destroy(other.gameObject);
        }
    }

    // Adds random time to the timer
    void AddTimeEffect()
    {
        int timeChange = Random.Range(10, 21); // 10 to 20 seconds
        countdown.AddTime(timeChange);
        Debug.Log("Chest bonus! +" + timeChange + " seconds");
        if (popupUI != null)
        {
            popupUI.ShowMessage("You gained some time!", 5f);
        }
    }

    // Removes random time from the timer
    void RemoveTimeEffect()
    {
        int timeChange = Random.Range(10, 21); // 10 to 20 seconds
        countdown.RemoveTime(timeChange);
        Debug.Log("Chest penalty! -" + timeChange + " seconds");
        if (popupUI != null)
        {
            popupUI.ShowMessage("You lost some time!", 5f);
        }
    }

    // Blindness
    void BlackoutScreenEffect()
    {
        Debug.Log("Black screen for 5 seconds");
        StartCoroutine(BlackoutCoroutine());
        if (popupUI != null)
        {
            popupUI.ShowMessage("You've been blinded!", 5f);
        }
    }

    IEnumerator BlackoutCoroutine()
    {
        // Fade in
        blackScreenOverlay.color = new Color(0, 0, 0, 1); // fully opaque
        yield return new WaitForSeconds(5f);
        // Fade out
        blackScreenOverlay.color = new Color(0, 0, 0, 0); // transparent
    }

    // Fast time
    void DoubleSpeedEffect()
    {
        //Temporary
        //Debug.Log("Speed Doubled for 5 seconds!");
        //StartCoroutine(TemporarySpeedChange(2f, 5f));

        //Permanent
        Debug.Log("Speed Doubled");
        playerController.moveSpeed *= 2;
        if (popupUI != null)
        {
            popupUI.ShowMessage("Your speed has been doubled!", 5f);
        }    
    }

    // Slow time
    void HalfSpeedEffect()
    {
        //Temporary
        //Debug.Log("Speed Halved for 5 seconds!");
        //StartCoroutine(TemporarySpeedChange(0.5f, 5f));

        //Permanent
        Debug.Log("Speed Halved");
        playerController.moveSpeed /= 2;
        if (popupUI != null)
        {
            popupUI.ShowMessage("Your speed has been halved!", 5f);
        }
    }

    //Code for Temporary
    //IEnumerator TemporarySpeedChange(float multiplier, float duration)
    //{
    //    float originalSpeed = playerController.moveSpeed;
    //    playerController.moveSpeed *= multiplier;

    //    yield return new WaitForSeconds(duration);

    //    playerController.moveSpeed = originalSpeed;
    //}

    // Freeze
    void FreezeEffect()
    {
        Debug.Log("Player frozen for 5 seconds!");
        StartCoroutine(FreezePlayer(5f));
        if (popupUI != null)
        {
            popupUI.ShowMessage("You have been frozen!", 5f);
        }

    }

    IEnumerator FreezePlayer(float duration)
    {
        float originalSpeed = playerController.moveSpeed;
        playerController.moveSpeed = 0f;

        yield return new WaitForSeconds(duration);

        playerController.moveSpeed = originalSpeed;
    }

    //Minimap
    void MinimapEffect()
    {
        if (minimapUI == null)
        {
            minimapUI = GameObject.FindGameObjectWithTag("Minimap");
            if (minimapUI == null)
            {
                Debug.LogWarning("Minimap UI not found when trying to activate minimap effect.");
                return; // Exit early
            }
        } 

        if (popupUI != null)
        {
            popupUI.ShowMessage("A minimap has been revealed!", 5f);
        }
        else
        {
            Debug.LogWarning("Popup UI not found! Could not show message.");
        }
        StartCoroutine(ShowMinimapTemporarily());
    }

    private IEnumerator ShowMinimapTemporarily()
    {
        if (minimapUI != null)
        {
            minimapUI.SetActive(true);
            yield return new WaitForSeconds(30f);
            minimapUI.SetActive(false);           
        }
        else
        {
            Debug.LogWarning("Minimap UI is missing! Cannot show minimap effect.");
        }
    }
}
