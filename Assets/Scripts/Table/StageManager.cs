using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Header("Manager")]
    public TableManager Table;
    public SheetManager Sheet;
    public InteractableManager InteractableManager;
    
    public MapReader Reader;
    
    public PlayerController Controller;
    public PlayerInventory Inventory;
    
    public Direction playerForwardDirection = Direction.Right;

    private void Awake()
    {
        Reader.ReadMap();
        
        Controller.Init(Table.startPosition, playerForwardDirection);
        
        InteractableManager.Init();
        
        Sheet.Init();
    }
}
