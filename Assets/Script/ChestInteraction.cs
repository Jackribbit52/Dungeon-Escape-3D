using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ChestInteraction : MonoBehaviour
{
    public CountdownTimer countdown;
    public List<System.Action> chestEffects; // List of effects (random actions)
    public Image blackScreenOverlay;
    public PlayerController playerController;
    public EffectPopupUI popupUI;


    void Start()
    {
        // Add more effects as needed
        chestEffects = new List<System.Action>
        {
            AddTimeEffect,
            RemoveTimeEffect,
            BlackoutScreenEffect,
            DoubleSpeedEffect,
            HalfSpeedEffect,
            FreezeEffect
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
        int timeChange = Random.Range(5, 11); // 5 to 10 seconds
        countdown.AddTime(timeChange);
        Debug.Log("Chest bonus! +" + timeChange + " seconds");
        popupUI.ShowMessage("You gained some time!", 5f);
    }

    // Removes random time from the timer
    void RemoveTimeEffect()
    {
        int timeChange = Random.Range(5, 11); // 5 to 10 seconds
        countdown.RemoveTime(timeChange);
        Debug.Log("Chest penalty! -" + timeChange + " seconds");
        popupUI.ShowMessage("You lost some time!", 5f);
    }

    // Blindness
    void BlackoutScreenEffect()
    {
        Debug.Log("Black screen for 5 seconds");
        StartCoroutine(BlackoutCoroutine());
        popupUI.ShowMessage("You've been blinded!", 5f);
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
        popupUI.ShowMessage("Your speed has been doubled!", 5f);
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
        popupUI.ShowMessage("Your speed has been halved!", 5f);

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
        popupUI.ShowMessage("You have been frozen!", 5f);

    }

    IEnumerator FreezePlayer(float duration)
    {
        float originalSpeed = playerController.moveSpeed;
        playerController.moveSpeed = 0f;

        yield return new WaitForSeconds(duration);

        playerController.moveSpeed = originalSpeed;
    }
}
