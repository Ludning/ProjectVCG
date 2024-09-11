using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSheet : MonoBehaviour
{
    public Transform SheetParent;
    public SheetManager SheetManager;
    private Action<int> choiceSheetAction;
    
    public void OnClickActiveMain()
    {
        SheetManager.ChoiceSheet(0);
    }
}
