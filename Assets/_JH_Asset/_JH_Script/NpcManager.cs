using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Make sure to include this for legacy Text
using DG.Tweening;
using Frameworks; // Make sure to include this for DOTween

public class NPCManager : SingletonMonoBehaviour<NPCManager>
{
    private Dictionary<string, string[]> _dialogueDictionary;
    private Coroutine TalkCoroutineRunning;

    public Text dialogueText;  // Reference to the legacy Text component

    // Start()
    private void Start()
    {
        InitializeDialogueDictionary();
    }
    
    // Update()
    private void Update()
    {
        // 여러 줄 모두 출력할 때
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            GetDialogue("IntroduceBlockRun");
        }


        // 특정 대화만 출력
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            GetDialogue("HintMessage",0);
        }
    }

    private void InitializeDialogueDictionary()
    {
        _dialogueDictionary = new Dictionary<string, string[]>();

        // Add dialogues for different situations
        _dialogueDictionary.Add("HintMessage", new string[]
        {
            "왼쪽으로 돌아야 할 듯",
            "오른쪽으로 돌아야 할 듯",
            "뒤로 돌아야 할 듯",
            "앞으로 갈 수 없어",
            "음식을 내려놓을 수 없어"
        });
        
        _dialogueDictionary.Add("IntroduceBlockRun", new string[]
        {
            "이 블럭은 달리기 블럭이야",
            "끝까지 달릴 수 있어"
        });

        _dialogueDictionary.Add("IntroduceBlockLoop", new string[]
        {
            "이 블럭은 반복문 블럭이야",
            "원하는 동작을 반복할 수 있지",
            "사용해봐!"
        });
    }

    private void GetDialogue(string msg, int index = -1)
    {
        if (_dialogueDictionary.ContainsKey(msg))
        {
            if (TalkCoroutineRunning != null)
            {
                StopCoroutine(TalkCoroutineRunning);
                dialogueText.text = "";
                dialogueText.DOKill();
            }
            

            // 인덱스 값이 유효한 경우 해당 인덱스의 메시지만 출력
            if (index >= 0 && index < _dialogueDictionary[msg].Length)
            {
                string[] selectedMsg = new string[] { _dialogueDictionary[msg][index] };
                TalkCoroutineRunning = StartCoroutine(StartTalk(msg, selectedMsg));
            }
            else
            {
                // 인덱스 값이 유효하지 않으면 모든 메시지를 출력
                TalkCoroutineRunning = StartCoroutine(StartTalk(msg, _dialogueDictionary[msg]));
            }
        }
    }

    private IEnumerator StartTalk(string key, string[] msg)
    {
        foreach (var line in msg)
        {
            yield return DisplayTextWithTypingEffect(line);
            yield return new WaitForSeconds(1f); // Wait time before showing the next hint message
        }


        TalkCoroutineRunning = null;
    }

    private IEnumerator DisplayTextWithTypingEffect(string text)
    {
        dialogueText.text = ""; // Clear previous text

        // Using DOTween's DOText method for typing effect
        Tween typingTween = dialogueText.DOText(text, 1f); // 1f is the duration for the typing effect
        yield return typingTween.WaitForCompletion();

        // Optional: You can add a delay here before allowing the next line to start typing
        //yield return new WaitForSeconds(1f);
    }
}
