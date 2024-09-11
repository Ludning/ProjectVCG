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

    public void Init(SheetManager sheetManager, int sheetIndex)
    {
        this.SheetManager = sheetManager;
        this.sheetIndex = sheetIndex;
    }

    public void OnClickActiveRepeat()
    {
        SheetManager.ChoiceSheet(sheetIndex);
    }
    public void SetRepeatCount(int count)
    {
        SheetManager.SetRepeatCount(sheetIndex, count);
    }
}
