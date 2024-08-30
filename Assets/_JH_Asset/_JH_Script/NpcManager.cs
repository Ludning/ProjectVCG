using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro; // Make sure to include this for DOTween

public class NPCManager : MonoBehaviour
{
    private Dictionary<string, string[]> _dialogueDictionary;
    [SerializeField] private UIContainer UIContainer;

    private void Awake()
    {
        Debug.Log("test");
    }
    // Update()
    private void Update()
    {
        // 여러 줄 모두 출력할 때
   /*     if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            string context = DataManager.Instance.GetGameData<ErrorData>(((int)ErrorType.NoTile).ToString()).Context;
            GetDialogue(context);
            //GetDialogue("IntroduceBlockRun");
        }


        // 특정 대화만 출력
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GetDialogue("HintMessage",0);
        }*/
    }

    public void GetMessage(ErrorType msg)
    {
        NPCMessagePopup messagePopup = UIContainer.GetUIBase<NPCMessagePopup>(MainUIType.NPCMessagePopup);
        messagePopup.gameObject.SetActive(true);
        Debug.Log(msg);
        string context = DataManager.Instance.GetGameData<ErrorData>(((int)msg).ToString()).Context;
        GetDialogue(messagePopup.MessageTextComoponent, context);
    }
    private void GetDialogue(TextMeshProUGUI textComponent, string msg, int index = -1)
    {
        textComponent.text = "";
        textComponent.DOKill();

        textComponent.DOText(msg, 3f);
    }
}
