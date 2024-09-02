using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Make sure to include this for legacy Text
using DG.Tweening;
using Frameworks;
using TMPro; // Make sure to include this for DOTween
using DG.Tweening;

public class NPCManager : SingletonMonoBehaviour<NPCManager>
{
    private Dictionary<string, string[]> _dialogueDictionary;

    public TextMeshProUGUI dialogueText;  // Reference to the legacy Text component

    // Update()
    private void Update()
    {
        // 여러 줄 모두 출력할 때
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            string context = DataManager.Instance.GetGameData<ErrorMessageData>(((int)ErrorType.NoTile).ToString()).Context;
            GetDialogue(context);
            //GetDialogue("IntroduceBlockRun");
        }


        // 특정 대화만 출력
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GetDialogue("HintMessage",0);
        }
    }

    private void GetDialogue(string msg, int index = -1)
    {
        dialogueText.text = "";
        dialogueText.DOKill();

        dialogueText.DOText(msg, 3f);
    }
}
