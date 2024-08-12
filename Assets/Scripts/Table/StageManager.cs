using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public PlayerController Controller;
    public PlayerInventory Inventory;
    public TableManager Table;
    public MapReader Reader;

    public Direction playerForwardDirection = Direction.Right;

    private void Awake()
    {
        Reader.ReadMap();
        
        Controller.Init(Table.startPosition, playerForwardDirection);
    }
}
