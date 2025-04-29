using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine.SceneManagement;

[System.Serializable]
public class WeightedEffect
{
    public System.Action effect;
    public float weight;
}

public class ChestInteraction : MonoBehaviour
{
    public CountdownTimer countdown;
    //public List<System.Action> chestEffects; // List of effects (random actions)
    private List<WeightedEffect> weightedEffects;

    public Image blackScreenOverlay;
    public PlayerController playerController;
    [SerializeField]
    private EffectPopupUI popupUI;
    [SerializeField] 
    private GameObject minimapUI;
    public BackgroundMusic bgm;


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
        weightedEffects = new List<WeightedEffect>
        {
            new WeightedEffect { effect = AddTimeEffect, weight = 5 },
            new WeightedEffect { effect = RemoveTimeEffect, weight = 5 },
            new WeightedEffect { effect = BlackoutScreenEffect, weight = 5 },
            new WeightedEffect { effect = DoubleSpeedEffect, weight = 5 },
            new WeightedEffect { effect = HalfSpeedEffect, weight = 5 },
            new WeightedEffect { effect = FreezeEffect, weight = 5 },
            new WeightedEffect { effect = MinimapEffect, weight = 3 },
            new WeightedEffect { effect = EscapeScroll, weight = 1 },
            new WeightedEffect { effect = Trapdoor, weight = 1 }
        };
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Chest"))
        {
            var effect = GetRandomWeightedEffect();
            effect?.Invoke(); // Safely call if not null
            Destroy(other.gameObject);
        }
    }

    System.Action GetRandomWeightedEffect()
    {
        float totalWeight = 0f;
        foreach (var we in weightedEffects)
            totalWeight += we.weight;

        float randomValue = Random.Range(0f, totalWeight);
        float runningSum = 0f;

        foreach (var we in weightedEffects)
        {
            runningSum += we.weight;
            if (randomValue <= runningSum)
                return we.effect;
        }

        return null;
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
        Debug.Log("Speed Doubled for 5 seconds!");
        StartCoroutine(TemporarySpeedChange(2f, 5f));
        if (popupUI != null)
        {
            popupUI.ShowMessage("Your speed has been doubled!", 5f);
        }

        //Permanent
        //Debug.Log("Speed Doubled");
        //playerController.moveSpeed *= 2;
        //if (popupUI != null)
        //{
        //    popupUI.ShowMessage("Your speed has been doubled!", 5f);
        //}    
    }

    // Slow time
    void HalfSpeedEffect()
    {
        //Temporary
        Debug.Log("Speed Halved for 5 seconds!");
        StartCoroutine(TemporarySpeedChange(0.5f, 5f));
        if (popupUI != null)
        {
            popupUI.ShowMessage("Your speed has been halved!", 5f);
        }

        //Permanent
        //Debug.Log("Speed Halved");
        //playerController.moveSpeed /= 2;
        //if (popupUI != null)
        //{
        //    popupUI.ShowMessage("Your speed has been halved!", 5f);
        //}
    }

    //Code for Temporary
    IEnumerator TemporarySpeedChange(float multiplier, float duration)
    {
        float originalSpeed = playerController.moveSpeed;
        playerController.moveSpeed *= multiplier;

        yield return new WaitForSeconds(duration);

        playerController.moveSpeed = originalSpeed;
    }

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

    void EscapeScroll()
    {
        StartCoroutine(ShowEscapeMessage());
    }

    private IEnumerator ShowEscapeMessage()
    {
        popupUI.ShowMessage("You found a scroll of escape! Escaping in 3 seconds", 3f);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(3);
        countdown.AddTime(120);
        bgm.PlayEndingMusic();
    }

    void Trapdoor()
    {
        StartCoroutine(ShowTrapdoorMessage());
    }

    private IEnumerator ShowTrapdoorMessage()
    {
        popupUI.ShowMessage("You activated a trapdoor! Rest in Peace!", 2f);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(2);
        countdown.AddTime(120);
    }
}
