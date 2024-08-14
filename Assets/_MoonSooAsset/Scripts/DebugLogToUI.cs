using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugLogToUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI logText;  // UI Text 연결
    private string logMessages = "";  // 모든 로그 메시지를 저장할 변수

    void OnEnable()
    {
        // 로그 메시지가 발생할 때마다 OnLogMessageReceived 메서드를 호출하도록 설정
        Application.logMessageReceived += OnLogMessageReceived;
    }

    void OnDisable()
    {
        // 오브젝트가 비활성화될 때 델리게이트에서 메서드 제거
        Application.logMessageReceived -= OnLogMessageReceived;
    }

    void OnLogMessageReceived(string condition, string stackTrace, LogType type)
    {
        string coloredMessage = condition;

        // LogType에 따른 색상 설정
        switch (type)
        {
            case LogType.Error:
            case LogType.Exception:
                coloredMessage = $"<color=red>{condition}</color>";
                break;
            case LogType.Warning:
                coloredMessage = $"<color=yellow>{condition}</color>";
                break;
            case LogType.Log:
                coloredMessage = $"<color=black>{condition}</color>";
                break;
            default:
                coloredMessage = condition;
                break;
        }

        // 로그 메시지 추가
        logMessages += coloredMessage + "\n";
    
        // 로그 메시지 업데이트
        logText.text = logMessages;
    }
}
