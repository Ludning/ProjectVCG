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

        var stageDataDict = DataManager.Instance.GetGameDataDictionary<StageData>();
        switch (type)
        {
            case ValueType.Chapter:
                GameManager.Instance.SelectedChapterIndex = Value;
                GameManager.Instance.FilteredList = stageDataDict.Values
                    .Where(stageData => stageData.Chapter == Value)
                    .ToList();
                break;
            case ValueType.Stage:
                GameManager.Instance.SelectedStageIndex = Value;
                GameManager.Instance.Stage = GameManager.Instance.FilteredList
                    .First(stageData => stageData.Stage == Value);
                break;
        }
    }
}
