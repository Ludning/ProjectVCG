using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class StageManager : MonoBehaviour
{
    [Header("Manager")]
    public TableManager TableManager;
    public SheetManager SheetManager;
    public InteractableManager InteractableManager;
    public NPCManager NPCManager;
    public LevelManager levelManager;

    [Header("Component")]
    [FormerlySerializedAs("ui")] public UIContainer UIContainer;
    
    public GameObject Items;
    
    public PlayerController Controller;
    public PlayerInventory Inventory;


    public void InitStage()
    {
        StageData stageData = GameManager.Instance.GetCurrentStageData();
        
        Controller.gameObject.SetActive(true);
        Items.SetActive(true);
        
        TableData tableData = DataManager.Instance.GetTableData(stageData.Index);
        
        TableManager.InitTable(tableData);
        Vector3 playerPosition = TableManager.GetTilePosition(tableData.PlayerPosition);
        Controller.Init(tableData.PlayerPosition, playerPosition, tableData.PlayerDirection);
        InteractableManager.Init(stageData.ShowBlock);
        SheetManager.Init();
        UIContainer.InitGame();
        levelManager.Init(stageData.StageClearCondition);
    }
    public void InitMain()
    {
        TableManager.Clear();
        SheetManager.Clear();
        InteractableManager.Clear();
        levelManager.Clear();
        NPCManager.Clear();
        UIContainer.InitStageSelect();
        Clear();
    }
    public void ClearStage()
    {
        TableManager.Clear();
        SheetManager.Clear();
        InteractableManager.Clear();
        levelManager.Clear();
        NPCManager.Clear();
        UIContainer.Clear();
        Clear();
    }
    public void Clear()
    {
        Controller.gameObject.SetActive(false);
        Items.SetActive(false);
    }
}
