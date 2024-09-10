using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using UnityEngine.Serialization;

public class StageManager : MonoBehaviour
{
    [Header("Manager")]
    public TableManager TableManager;
    public SheetManager SheetManager;
    public InteractableManager InteractableManager;
    public RecipeManager RecipeManager;
    public NPCManager NPCManager;

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


        //SheetManager.TestLogics();
    }

    public void ClearStage()
    {
        TableManager.Clear();
        SheetManager.Clear();
        InteractableManager.Clear();
        RecipeManager.Clear();
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
