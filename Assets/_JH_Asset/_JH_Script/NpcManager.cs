using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Make sure to include this for legacy Text
using DG.Tweening;    // Make sure to include this for DOTween

public class NpcManager : SingleTonMono<NpcManager>
{
    private Dictionary<string, string[]> dialogueDictionary;
    private Coroutine TalkCoroutineRunning;

    public Text dialogueText;  // Reference to the legacy Text component

    private void Start()
    {
        dialogueDictionary = new Dictionary<string, string[]>();

        // Add dialogues for different situations
        dialogueDictionary.Add("HintMessage", new string[]
        {
            "왼쪽으로 돌아야 할 듯",
            "오른쪽으로 돌아야 할 듯",
            "뒤로 돌아야 할 듯",
            "앞으로 갈 수 없어",
            "음식을 내려놓을 수 없어"
        });
        dialogueDictionary.Add("IntroduceBlockRun", new string[]
        {
            "이 블럭은 달리기 블럭이야",
            "끝까지 달릴 수 있어"
        });

        dialogueDictionary.Add("IntroduceBlockLoop", new string[]
        {
            "이 블럭은 반복문 블럭이야",
            "원하는 동작을 반복할 수 있지",
            "사용해봐!"
        });
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            GetDialogue("HintMessage");
        }
    }
    public void GetDialogue(string msg)
    {
        if (dialogueDictionary.ContainsKey(msg))
        {
            if (TalkCoroutineRunning != null)
            {
                StopCoroutine(TalkCoroutineRunning);
            }
            TalkCoroutineRunning = StartCoroutine(StartTalk(msg, dialogueDictionary[msg]));
        }
    }

    public IEnumerator StartTalk(string key, string[] msg)
    {
        if (key == "HintMessage")
        {
            foreach (var line in msg)
            {
                yield return DisplayTextWithTypingEffect(line);
                yield return new WaitForSeconds(1f); // Wait time before showing the next hint message
            }
        }
        else
        {
            foreach (var line in msg)
            {
                yield return DisplayTextWithTypingEffect(line);
                yield return new WaitForSeconds(2f); // Wait time between paragraphs
            }
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
        yield return new WaitForSeconds(1f);
    }
}
