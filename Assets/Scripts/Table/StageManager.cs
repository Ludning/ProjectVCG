using System.Collections.Generic;
using TilemapSystem;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Header("Manager")]
    public TableManager Table;
    public SheetManager Sheet;
    public InteractableManager InteractableManager;
    
    public PlayerController Controller;
    public PlayerInventory Inventory;
    
    public Direction playerForwardDirection = Direction.Right;
    
    public MapReader Reader;

    public void InitStage()
    {
        Reader.ReadMap();

        Controller.Init(Table.startPosition, playerForwardDirection);
        
        InteractableManager.Init();
        Sheet.Init();
    }
}
