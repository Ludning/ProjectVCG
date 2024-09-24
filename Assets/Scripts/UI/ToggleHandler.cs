using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ValueType
{
    Chapter,
    Stage,
}
public class ToggleHandler : MonoBehaviour
{
    public TextMeshProUGUI Context;
    public ValueType type;
    public int Value;
    public Toggle toggle;
    private const int _delay = 500;
    
    /*private void Awake()
    {
        toggle = GetComponent<Toggle>();

        // Toggle의 OnValueChanged 이벤트에 리스너 추가
        //toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }*/
    public void OnToggleValueChanged(bool isOn)
    {
        if (!isOn)
            return;

        switch (type)
        {
            case ValueType.Chapter:
                Debug.Log(Value);
                GameManager.Instance.SelectedChapterIndex = Value;
                break;
            case ValueType.Stage:
                Debug.Log(Value);
                GameManager.Instance.SelectedStageIndex = Value;
                break;
        }
        EnableButtonAfterDelay().Forget();
    }
    private async UniTaskVoid EnableButtonAfterDelay()
    {
        toggle.interactable = false;
        await UniTask.Delay(_delay);
        toggle.interactable = true;
    }
}
