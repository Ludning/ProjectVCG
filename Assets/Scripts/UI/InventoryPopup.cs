using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPopup : MonoBehaviour
{
    [SerializeField] private Transform Context;
    private List<Image> ItemList = new List<Image>();
    private int _cursor;
    
    public void AddItem(string ItemIndex)
    {
        FoodData data = DataManager.Instance.GetGameData<FoodData>(ItemIndex);
    }

    public void RemoveItem()
    {
        
    }
}
