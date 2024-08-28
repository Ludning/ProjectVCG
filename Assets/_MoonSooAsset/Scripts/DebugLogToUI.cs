using System.Text;
using TMPro;
using UnityEngine;

public class DebugLogToUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI logText; // UI Text 연결
    private StringBuilder _logMessages; // 모든 로그 메시지를 저장할 변수

    private void OnEnable()
    {
        // 로그 메시지가 발생할 때마다 OnLogMessageReceived 메서드를 호출하도록 설정
        Application.logMessageReceived += OnLogMessageReceived;
    }

    private void OnDisable()
    {
        // 오브젝트가 비활성화될 때 델리게이트에서 메서드 제거
        Application.logMessageReceived -= OnLogMessageReceived;
    }

    private void OnLogMessageReceived(string logString, string stackTrace, LogType type)
    {
        // LogType에 따른 색상 설정
        string color = type switch
        {
            LogType.Error or LogType.Exception => ColorUtility.ToHtmlStringRGB(Color.red),
            LogType.Warning => ColorUtility.ToHtmlStringRGB(Color.yellow),
            LogType.Log => ColorUtility.ToHtmlStringRGB(Color.black),
            _ => ColorUtility.ToHtmlStringRGB(Color.white),
        };

        // 로그 메시지 추가
        _logMessages.AppendLine($"<color=#{color}>{logString}</color>");

        // 로그 메시지 업데이트
        logText.text = _logMessages.ToString();
    }
}
