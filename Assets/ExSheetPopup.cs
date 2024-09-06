using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExSheetPopup : MonoBehaviour, IUIBase
{
    [SerializeField] private SheetManager SheetManager;

    public void OnClick_Close()
    {
        SheetManager.ClearExBlockLogic();
    }
    

    public void OnClick_ExitExercise()
    {
        SheetManager.OnClick_ExitExercise();
    }
}
