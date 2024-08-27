using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Header("Manager")]
    public TableManager Table;
    public SheetManager Sheet;
    public InteractableManager InteractableManager;

    public MainUI ui;
    
    public GameObject Items;
    
    public PlayerController Controller;
    public PlayerInventory Inventory;
    
    public Direction playerForwardDirection = Direction.Right;
    
    public MapReader Reader;

    public void InitStage()
    {
        ui.gameObject.SetActive(false);
        
        //스테이지 오브젝트 활성화
        Reader.gameObject.SetActive(true);
        Controller.gameObject.SetActive(true);
        Items.SetActive(true);
        
        Reader.ReadMap();
        
        Controller.Init(Table.startPosition, playerForwardDirection);
        
        InteractableManager.Init();
        Sheet.Init();
    }
}
