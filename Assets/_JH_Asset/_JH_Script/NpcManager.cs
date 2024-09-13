using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro; // Make sure to include this for DOTween

public class NPCManager : MonoBehaviour
{
    //private Dictionary<string, string[]> _dialogueDictionary;
    [SerializeField] private UIContainer UIContainer;

    
    public void GetMessage(ErrorType msg)
    {
        if (msg == ErrorType.NoError)
            return;

        UIContainer.NPCMessagePopup.gameObject.SetActive(true);

        string key = ((int)msg).ToString();
        ErrorMessageData data = DataManager.Instance.GetGameData<ErrorMessageData>(key);
        string context = data.Context;
        GetDialogue(UIContainer.NPCMessagePopup.MessageTextComoponent, context);
    }
    private void GetDialogue(TextMeshProUGUI textComponent, string msg, int index = -1)
    {
        textComponent.text = "";
        textComponent.DOKill();

        textComponent.DOText(msg, 3f);
    }

    public void Clear()
    {
        UIContainer.NPCMessagePopup.gameObject.SetActive(false);
    }
}
