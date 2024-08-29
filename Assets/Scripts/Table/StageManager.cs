using UnityEngine;
using UnityEngine.Serialization;

public class StageManager : MonoBehaviour
{
    [Header("Manager")]
    public TableManager Table;
    public SheetManager Sheet;
    public InteractableManager InteractableManager;

    [FormerlySerializedAs("ui")] public UIContainer uiContainer;
    
    public GameObject Items;
    
    public PlayerController Controller;
    public PlayerInventory Inventory;
    
    public Direction playerForwardDirection = Direction.Right;
    

    public void InitStage()
    {
        Table.InitTable("string stageInfo");
        uiContainer.gameObject.SetActive(false);
        
        
        Controller.gameObject.SetActive(true);
        Items.SetActive(true);
        
        Controller.Init(Table.startPosition, playerForwardDirection);
        
        InteractableManager.Init();
        Sheet.Init();
    }
}
