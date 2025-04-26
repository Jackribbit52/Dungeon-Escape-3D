using UnityEngine;
using TMPro;

public class EffectPopupUI : MonoBehaviour
{
    public TMP_Text messageText;

    public void Awake()
    {
        gameObject.SetActive(false);
    }

    public void ShowMessage(string message, float duration)
    {
        gameObject.SetActive(true);
        messageText.text = message;
        CancelInvoke(nameof(Hide)); // Just in case
        Invoke(nameof(Hide), duration);
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }
}

