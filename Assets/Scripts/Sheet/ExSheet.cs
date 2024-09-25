using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExSheet : SheetBase
{
    public void OnClick_ExitExercise()
    {
        SheetManager.OnClick_ExitExercise();
    }
    
    public void OnClick_Clear()
    {
        SheetManager.ClearExBlockLogic();
    }
}
