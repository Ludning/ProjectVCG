using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro; // Make sure to include this for DOTween

public class NPCManager : MonoBehaviour
{
    //private Dictionary<string, string[]> _dialogueDictionary;
    [SerializeField] private UIContainer UIContainer;

    private void Awake()
    {
        Debug.Log("test");
    }
    
    public void GetMessage(ErrorType msg)
    {
        if (msg == ErrorType.NoError)
            return;

        NPCMessagePopup messagePopup = UIContainer.GetUIBase<NPCMessagePopup>(MainUIType.NPCMessagePopup);
        messagePopup.gameObject.SetActive(true);
        Debug.Log(msg);
        if (msg == ErrorType.NoTile)
            Debug.Log("aa");

        string key = ((int)msg).ToString();
        ErrorMessageData data = DataManager.Instance.GetGameData<ErrorMessageData>(key);
        string context = data.Context;

        Debug.Log(context);
        GetDialogue(messagePopup.MessageTextComoponent, context);
    }
    private void GetDialogue(TextMeshProUGUI textComponent, string msg, int index = -1)
    {
        textComponent.text = "";
        textComponent.DOKill();

        textComponent.DOText(msg, 3f);
    }

    public void Clear()
    {
        NPCMessagePopup messagePopup = UIContainer.GetUIBase<NPCMessagePopup>(MainUIType.NPCMessagePopup);
        messagePopup.gameObject.SetActive(false);
    }
}
