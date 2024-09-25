using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSheet : SheetBase
{
    public void OnClickActiveMain()
    {
        SheetManager.ChoiceSheet(0);
    }
}
