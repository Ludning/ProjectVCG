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
    [SerializeField] private GameObject AnserObj;

    private void Awake()
    {
        StartButton.WhenSelect.AddListener(OnClick_Start);
        CookButton.WhenSelect.AddListener(()=>OnClick_SetLogic(BlockLogicType.Cook));
        MoveButton.WhenSelect.AddListener(()=> OnClick_SetLogic(BlockLogicType.Move));
        PushItemButton.WhenSelect.AddListener(()=>OnClick_SetLogic(BlockLogicType.PushItem));
        RotateLeftButton.WhenSelect.AddListener(()=>OnClick_SetLogic(BlockLogicType.RotateLeft));
        RotateRightButton.WhenSelect.AddListener(()=>OnClick_SetLogic(BlockLogicType.RotateRight));
        SetItemButton.WhenSelect.AddListener(()=>OnClick_SetLogic(BlockLogicType.SetItem));
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
        CheckChildren(AnserObj.transform);

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
    void CheckChildren(Transform parent)
    {
        // A 오브젝트의 자식들을 순회
        foreach (Transform child in parent)
        {
            Debug.Log("오브젝트: " + child.name);

            // 자식 오브젝트가 있는지 확인
            if (child.childCount > 0)
            {
                Debug.Log(child.name + "은 자식 오브젝트가 있습니다.");

                // 자식 오브젝트들을 다시 순회
                foreach (Transform grandChild in child)
                {
                    Debug.Log(child.name + "의 자식: " + grandChild.name);
                }
            }
            else
            {
                Debug.Log(child.name + "은 자식 오브젝트가 없습니다.");
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            OnClick_Start();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            OnClick_SetLogic(BlockLogicType.Cook);
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            OnClick_SetLogic(BlockLogicType.Move);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            OnClick_SetLogic(BlockLogicType.PushItem);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            OnClick_SetLogic(BlockLogicType.RotateLeft);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            OnClick_SetLogic(BlockLogicType.RotateRight);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            OnClick_SetLogic(BlockLogicType.SetItem);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            OnClick_ClearLogic();
        }
    }
    public void OnClick_ClearLogic()
    {
        sheetText.text = "";
        sheetManager.ClearBlockLogic();
    }
}
