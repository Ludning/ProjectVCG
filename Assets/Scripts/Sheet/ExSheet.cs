using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExSheet : MonoBehaviour
{
    public Transform SheetParent;
    public SheetManager SheetManager;

    
    public void OnClick_ExitExercise()
    {
        SheetManager.OnClick_ExitExercise();
    }
    
    public void OnClick_Clear()
    {
        SheetManager.ClearExBlockLogic();
    }
}
