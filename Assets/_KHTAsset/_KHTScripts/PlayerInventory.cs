using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    int MaxCount = 10;
    Stack<ItemBase> itemStack = new Stack<ItemBase>();
    public Vector3 ItemStackPosition
    {
        get
        {
            return itemStack.Count * new Vector3(0, 1, 0);
        }
    }

    public void PushItem(ItemBase item)
    {
        if(itemStack.Count < MaxCount)
        {
            item.transform.SetParent(transform);
            item.transform.position = ItemStackPosition;
            itemStack.Push(item);
        }
        
    }

    public ItemBase PopItem(Vector3 position)
    {
        var item = itemStack.Pop();
        item.transform.SetParent(null);
        item.transform.position = position;
        return item;

    }
}
