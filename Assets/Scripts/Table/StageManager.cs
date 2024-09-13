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

    private StageData StageData;
    
    public void InitStage(StageData stageData)
    {
        StageData = stageData;
        
        Controller.gameObject.SetActive(true);
        Items.SetActive(true);
        
        TableData tableData = DataManager.Instance.GetTableData(StageData.Index);
        
        TableManager.InitTable(tableData);
        Vector3 playerPosition = TableManager.GetTilePosition(tableData.PlayerPosition);
        Controller.Init(tableData.PlayerPosition, playerPosition, tableData.PlayerDirection);
        InteractableManager.Init(stageData.ShowBlock);
        SheetManager.Init();
        UIContainer.Init();
        levelManager.Init(stageData.StageClearCondition);
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
