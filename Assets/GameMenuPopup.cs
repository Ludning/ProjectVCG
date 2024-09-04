using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuPopup : MonoBehaviour
{
    [SerializeField] private StageManager StageManager;

    public void OnClick_OK()
    {
        StageManager.ClearStage();
    }
}
