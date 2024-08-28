using System;
using System.Collections;
using System.Collections.Generic;
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
    private Toggle toggle;
    
    private void Awake()
    {
        toggle = GetComponent<Toggle>();

        // Toggle의 OnValueChanged 이벤트에 리스너 추가
        toggle.onValueChanged.AddListener(OnToggleValueChanged);
    }
    private void OnToggleValueChanged(bool isOn)
    {
        if (!isOn)
            return;

        switch (type)
        {
            case ValueType.Chapter:
                GameManager.Instance.SelectedChapterIndex = Value;
                break;
            case ValueType.Stage:
                GameManager.Instance.SelectedStageIndex = Value;
                break;
        }
    }
}
