using UnityEngine;
using TMPro;

public class EffectPopupUI : MonoBehaviour
{
    public TMP_Text messageText;
    [SerializeField]
    private GameObject panelObject;

    void Awake()
    {
        panelObject.SetActive(false);
    }

    public void ShowMessage(string message, float duration)
    {
        if (panelObject == null)
        {
            Debug.LogWarning("Popup panel object is missing!");
            return;
        }

        panelObject.SetActive(true);
        messageText.text = message;
        CancelInvoke(nameof(Hide));
        Invoke(nameof(Hide), duration);
    }

    void Hide()
    {
        if (panelObject != null)
        {
            panelObject.SetActive(false);
        }
    }
}



