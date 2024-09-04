using UnityEngine;
using UnityEngine.Serialization;

public class StageManager : MonoBehaviour
{
    [Header("Manager")]
    public TableManager TableManager;
    public SheetManager SheetManager;
    public InteractableManager InteractableManager;
    public RecipeManager RecipeManager;

    [FormerlySerializedAs("ui")] public UIContainer uiContainer;
    
    public GameObject Items;
    
    public PlayerController Controller;
    public PlayerInventory Inventory;
    
    public Direction playerForwardDirection = Direction.Right;
    

    public void InitStage(string stageIndex)
    {
        TableManager.InitTable(stageIndex);
        //uiContainer.gameObject.SetActive(false);
        
        Controller.gameObject.SetActive(true);
        Items.SetActive(true);
        
        Controller.Init(TableManager.startPosition, playerForwardDirection);
        
        InteractableManager.Init();
        SheetManager.Init();
    }

    public void ClearStage()
    {
        
    }
}
