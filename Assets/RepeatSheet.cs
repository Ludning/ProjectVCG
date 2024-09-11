using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class RepeatSheet : MonoBehaviour
{
    public Transform SheetParent;
    [ReadOnly] public SheetManager SheetManager;
    private int sheetIndex;
    private Action<int> choiceSheetAction;

    public void Init(SheetManager sheetManager, Action<int> choiceSheetAction, int sheetIndex)
    {
        this.SheetManager = sheetManager;
        this.choiceSheetAction = choiceSheetAction;
        this.sheetIndex = sheetIndex;
    }

    public void OnClickActiveRepeat()
    {
        choiceSheetAction.Invoke(sheetIndex);
    }
    public void SetRepeatCount(int count)
    {
        SheetManager.SetRepeatCount(sheetIndex, count);
    }
}
