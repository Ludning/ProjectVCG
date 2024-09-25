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

    //메인화면(스테이지선택화면)으로 초기화
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

    //스테이지 초기화
    public void InitStage()
    {
        StageData stageData = GameManager.Instance.GetCurrentStageData();
        
        Controller.gameObject.SetActive(true);
        Items.SetActive(true);
        
        TableData tableData = DataManager.Instance.GetTableData(stageData.Index);
        
        TableManager.InitTable(tableData);
        /*Vector3 playerPosition = TableManager.GetTilePosition(tableData.PlayerPosition, PositionType.Player);
        Controller.Init(tableData.PlayerPosition, playerPosition, tableData.PlayerDirection);*/
        InteractableManager.Init(stageData);
        SheetManager.Init();
        UIContainer.InitGame();
        levelManager.Init(stageData.StageClearCondition);
    }

    public void ResetStage()
    {
        StageData stageData = GameManager.Instance.GetCurrentStageData();
        TableData tableData = DataManager.Instance.GetTableData(stageData.Index);
        
        Controller.Clear();
        Inventory.Clear();
        
        TableManager.ResetTable(tableData);
        levelManager.ResetLevelState();
        //NPCManager.Clear();
        UIContainer.InitGame();
        
        InteractableManager.InteractableButtons[BlockLogicType.Start].gameObject.SetActive(true);
        InteractableManager.InteractableButtons[BlockLogicType.Reset].gameObject.SetActive(false);
    }
    
    //스테이지 비우기
    public void ClearStage()
    {
        TableManager.Clear();
        SheetManager.Clear();
        InteractableManager.Clear();
        levelManager.Clear();
        NPCManager.Clear();
        UIContainer.Clear();
        Controller.Clear();
        Inventory.Clear();
        Clear();
    }
    public void Clear()
    {
        Controller.gameObject.SetActive(false);
        Items.SetActive(false);
    }
}
