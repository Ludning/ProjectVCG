using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class RepeatSheet : SheetBase
{
    public TextMeshProUGUI TMP_SheetName;
    private int sheetIndex;

    public void Init(SheetManager sheetManager, int sheetIndex, string sheetName)
    {
        this.SheetManager = sheetManager;
        this.sheetIndex = sheetIndex;
        this.TMP_SheetName.text = sheetName;
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
