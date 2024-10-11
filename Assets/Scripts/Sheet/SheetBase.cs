using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheetBase : MonoBehaviour
{
    public Transform SheetParent;
    public string SheetName;
    [ReadOnly] public SheetManager SheetManager;
    public int SheetLimit { get; set; }
    public int BlockCount { get; set; }

    public List<BlockSlot> BlockSlots = new List<BlockSlot>();

    public void Init()
    {
        Clear();
        for (int i = 0; i < SheetLimit; i++)
        {
            GameObject slotPrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>("BlockSlot");
            BlockSlot slot = Instantiate(slotPrefab, SheetParent).GetComponent<BlockSlot>();
            BlockSlots.Add(slot);
        }
    }
    public void Clear()
    {
        foreach (var blockSlot in BlockSlots)
            Destroy(blockSlot);
        BlockSlots.Clear();
    }
}
