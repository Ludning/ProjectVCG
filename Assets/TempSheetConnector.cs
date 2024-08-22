using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TempSheetConnector : MonoBehaviour
{
    [SerializeField] private SheetManager sheetManager;
    [SerializeField] private TextMeshProUGUI sheetText;

    [SerializeField] private InteractableUnityEventWrapper StartButton;
    [SerializeField] private InteractableUnityEventWrapper CookButton;
    [SerializeField] private InteractableUnityEventWrapper MoveButton;
    [SerializeField] private InteractableUnityEventWrapper PushItemButton;
    [SerializeField] private InteractableUnityEventWrapper RotateLeftButton;
    [SerializeField] private InteractableUnityEventWrapper RotateRightButton;
    [SerializeField] private InteractableUnityEventWrapper SetItemButton;
    [SerializeField] private InteractableUnityEventWrapper ClearButton;
    
    
    [SerializeField] private GameObject CookPrefab;
    [SerializeField] private GameObject MovePrefab;
    [SerializeField] private GameObject PushItemPrefab;
    [SerializeField] private GameObject RotateLeftPrefab;
    [SerializeField] private GameObject RotateRightPrefab;
    [SerializeField] private GameObject SetItemPrefab;
    

    private void Awake()
    {
        StartButton.WhenSelect.AddListener(OnClick_Start);
        CookButton.WhenSelect.AddListener(()=>OnClick_SetLogic(BlockLogicType.Cook));
        MoveButton.WhenSelect.AddListener(()=> OnClick_SetLogic(BlockLogicType.Move));
        PushItemButton.WhenSelect.AddListener(()=>OnClick_SetLogic(BlockLogicType.PushItem));
        RotateLeftButton.WhenSelect.AddListener(()=>OnClick_SetLogic(BlockLogicType.RotateLeft));
        RotateRightButton.WhenSelect.AddListener(()=>OnClick_SetLogic(BlockLogicType.RotateRight));
        SetItemButton.WhenSelect.AddListener(()=>OnClick_SetLogic(BlockLogicType.PopItem));
        ClearButton.WhenSelect.AddListener(OnClick_ClearLogic);
    }
    public void OnClick_Start()
    {
        Debug.Log("Start Logic");
        sheetManager.RunSheetBlock();
    }

    public void OnClick_SetLogic(BlockLogicType type)
    {
        Debug.Log(type.ToString());
        sheetText.text += $"\n{type.ToString()}";

        BlockLogicBase logicBase = null;
        switch (type)
        {
            case BlockLogicType.Cook:
                logicBase = Instantiate(CookPrefab).GetComponent<CookLogic>();
                break;
            case BlockLogicType.Move:
                logicBase = Instantiate(MovePrefab).GetComponent<MoveLogic>();
                break;
            case BlockLogicType.PushItem:
                logicBase = Instantiate(PushItemPrefab).GetComponent<PushItemLogic>();
                break;
            case BlockLogicType.RotateLeft:
                logicBase = Instantiate(RotateLeftPrefab).GetComponent<RotateLeftLogic>();
                break;
            case BlockLogicType.RotateRight:
                logicBase = Instantiate(RotateRightPrefab).GetComponent<RotateRightLogic>();
                break;
            case BlockLogicType.PopItem:
                logicBase = Instantiate(SetItemPrefab).GetComponent<PopItemLogic>();
                break;
        }

        if (logicBase != null)
        {
            logicBase.transform.position = new Vector3(100, 100, 100);
            sheetManager.AddBlockLogic(logicBase);
        }
    }
    public void OnClick_ClearLogic()
    {
        sheetText.text = "";
        sheetManager.ClearBlockLogic();
    }
}
