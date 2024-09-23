using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    Stack<ItemBase> itemStack = new Stack<ItemBase>();
    [SerializeField] Transform inventoryParent;
    
    public Vector3 ItemStackPosition
    {
        get
        {
            return itemStack.Count * new Vector3(0, 1, 0);
        }
    }

    public bool IsInventoryEmpty => itemStack.Count == 0;
    public bool IsInventoryOverflow => itemStack.Count >= DataManager.Instance.GetGameData<PcData>("0").InventoryMax;

    public void Init()
    {
        
    }
    public void Clear()
    {
        itemStack.Clear();
    }
    
    public void PushItem(ItemBase item)
    {
        if(itemStack.Count < DataManager.Instance.GetGameData<PcData>("0").InventoryMax)
        {
            item.transform.SetParent(inventoryParent);
            
            Debug.Log($"InventoryParent : {inventoryParent.position}");
            Debug.Log($"ItemStackPosition : {ItemStackPosition}");
            
            item.transform.localPosition = Vector3.zero;
            itemStack.Push(item);
        }
    }

    public ItemBase PopItem(Vector2Int position)
    {
        var item = itemStack.Pop();
        item.transform.SetParent(null);
        //item.transform.position = position;
        return item;
    }
}
