using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public PlayerController Controller;
    public PlayerInventory Inventory;
    public TableManager Table;
    public MapReader[] Reader;

    public Direction playerForwardDirection = Direction.Right;
    public void InitStage(int num)

    {
        for (int i = 0; i < Reader.Length; i++)
        {
            if (i == num)
            {
                Reader[i].gameObject.SetActive(true);
                Reader[i].ReadMap();                  
            }
            else
            {
                Reader[i].gameObject.SetActive(false);
            }
        }
        Controller.Init(Table.startPosition, playerForwardDirection);
    }

}
