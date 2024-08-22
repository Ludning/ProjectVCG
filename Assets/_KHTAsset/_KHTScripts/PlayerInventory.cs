using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    int MaxCount = 10;
    Stack<ItemBase> itemStack = new Stack<ItemBase>();
    [SerializeField] Transform inventoryParent;
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
            item.transform.SetParent(inventoryParent);
            
            Debug.Log($"InventoryParent : {inventoryParent.position}");
            Debug.Log($"ItemStackPosition : {ItemStackPosition}");
            
            item.transform.localPosition = ItemStackPosition;
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
