using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RepeatSheet : MonoBehaviour
{
    public GameObject RepeatSheetParent;
    private int sheetIndex;
    private Action<int> choiceSheetAction;

    public void Init(Action<int> choiceSheetAction, int sheetIndex)
    {
        this.choiceSheetAction = choiceSheetAction;
        this.sheetIndex = sheetIndex;
    }

    public void OnClickActiveRepeat()
    {
        choiceSheetAction.Invoke(sheetIndex);
    }
}
