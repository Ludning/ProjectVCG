using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DialogueSystem;
using UnityEngine.UI;

public class NPCManager : MonoBehaviour
{
    [SerializeField] private Text dialogueText;
    
    private Dictionary<string, string[]> _dialogueDictionary;
    private Coroutine _dialogueCoroutine;

    private void Start()
    {
        CreateDialogues(_dialogueDictionary);
    }
    
    private void Update()
    {
        DebugWithInput(); // [Debug Test]
    }

    private void CreateDialogues(Dictionary<string, string[]> dialogueDictionary)
    {
        // Dictionary의 참조가 없다면, 새롭게 생성한다.
        dialogueDictionary ??= new();

        // Dictionary를 초기화한다.
        dialogueDictionary.Clear();
        
        // Dictionary에 값을 추가한다.
        // Hint Messages
        dialogueDictionary.Add(Dialogues.HintMessage, new []
        {
            Dialogues.NeedToTurnLeft,
            Dialogues.NeedToTurnRight,
            Dialogues.NeedToTurnBack,
            Dialogues.UnableToForward,
            Dialogues.UnableTodDrop,
        });
        
        // Run Block
        dialogueDictionary.Add(Dialogues.IntroduceRunBlock, new[]
        {
            Dialogues.BlockRun01,
            Dialogues.BlockRun02,
        });
        
        // Loop Block
        dialogueDictionary.Add(Dialogues.IntroduceLoopBlock, new[]
        {
            Dialogues.BlockLoop01,
            Dialogues.BlockLoop02,
            Dialogues.BlockLoop03,
        });
    }
    
    private void GetDialogue(string messageType)
    {
        if (_dialogueDictionary.ContainsKey(messageType))
        {
            if (_dialogueCoroutine != null)
            {
                StopCoroutine(_dialogueCoroutine);
                dialogueText.text = string.Empty;
                dialogueText.DOKill();
            }
            
            _dialogueCoroutine = StartCoroutine(StartTalk(messageType, _dialogueDictionary[messageType]));
        }
    }

    private void GetDialogue(string messageType, int index)
    {
        if (_dialogueDictionary.TryGetValue(messageType, out string[] value))
        {
            string selectedMessage = value[index];
        }
    }


    private IEnumerator StartTalk(string key, string[] msg)
    {
        foreach (var line in msg)
        {
            yield return DisplayTextWithTypingEffect(line);
            yield return new WaitForSeconds(1f); // Wait time before showing the next hint message
        }
    }

    private IEnumerator DisplayTextWithTypingEffect(string text)
    {
        dialogueText.text = string.Empty; // Clear previous text

        // Using DOTween's DOText method for typing effect
        Tween typingTween = dialogueText.DOText(text, 1f); // 1f is the duration for the typing effect
        
        yield return typingTween.WaitForCompletion();

        // Optional: You can add a delay here before allowing the next line to start typing
        //yield return new WaitForSeconds(1f);
    }

    
    // [Debug Test]
    private void DebugWithInput()
    {
        // 여러 줄 모두 출력할 때
        if (Input.GetKeyDown(KeyCode.Alpha0))
        { 
            GetDialogue(Dialogues.IntroduceRunBlock);
        }
        
        // 특정 대화만 출력
        if (Input.GetKeyDown(KeyCode.Alpha1))
        { 
            GetDialogue(Dialogues.HintMessage, 0);
        }
    }
}
