using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TempSheetConnector : MonoBehaviour
{
    [SerializeField] private SheetManager sheetManager;
    [SerializeField] private TextMeshProUGUI sheetText;

    [SerializeField] private Button StartButton;
    [SerializeField] private Button CookButton;
    [SerializeField] private Button MoveButton;
    [SerializeField] private Button PushItemButton;
    [SerializeField] private Button RotateLeftButton;
    [SerializeField] private Button RotateRightButton;
    [SerializeField] private Button SetItemButton;
    [SerializeField] private Button ClearButton;
    
    
    [SerializeField] private GameObject CookPrefab;
    [SerializeField] private GameObject MovePrefab;
    [SerializeField] private GameObject PushItemPrefab;
    [SerializeField] private GameObject RotateLeftPrefab;
    [SerializeField] private GameObject RotateRightPrefab;
    [SerializeField] private GameObject SetItemPrefab;
    

    private void Awake()
    {
        StartButton.onClick.AddListener(OnClick_Start);
        CookButton.onClick.AddListener(()=>OnClick_SetLogic(BlockLogicType.Cook));
        MoveButton.onClick.AddListener(()=>OnClick_SetLogic(BlockLogicType.Move));
        PushItemButton.onClick.AddListener(()=>OnClick_SetLogic(BlockLogicType.PushItem));
        RotateLeftButton.onClick.AddListener(()=>OnClick_SetLogic(BlockLogicType.RotateLeft));
        RotateRightButton.onClick.AddListener(()=>OnClick_SetLogic(BlockLogicType.RotateRight));
        SetItemButton.onClick.AddListener(()=>OnClick_SetLogic(BlockLogicType.SetItem));
        ClearButton.onClick.AddListener(OnClick_ClearLogic);
    }
    public void OnClick_Start()
    {
        Debug.Log("Start Logic");
        sheetManager.RunSheetBlock();
    }

    public void OnClick_SetLogic(BlockLogicType type)
    {
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
            case BlockLogicType.SetItem:
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
