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
    [FormerlySerializedAs("ui")] public UIContainer uiContainer;
    
    public GameObject Items;
    
    public PlayerController Controller;
    public PlayerInventory Inventory;
    
    public void InitStage(string stageIndex)
    {
        TableData tableData = DataManager.Instance.GetGameData<TableData>(stageIndex);
        
        TableManager.InitTable(stageIndex);
        //uiContainer.gameObject.SetActive(false);
        
        Controller.gameObject.SetActive(true);
        Items.SetActive(true);

        Controller.Init(tableData.PlayerPosition, tableData.PlayerDirection);
        
        InteractableManager.Init();
        SheetManager.Init();
    }

    public void ClearStage()
    {
        TableManager.Clear();
        SheetManager.Clear();
        InteractableManager.Clear();
        RecipeManager.Clear();
        NPCManager.Clear();
    }
}
