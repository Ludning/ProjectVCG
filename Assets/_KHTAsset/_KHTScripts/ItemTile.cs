using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTile : TileBase
{
    [SerializeField] GameObject ItemPrefab;
    public override void OnItemSpwan()
    {
        GameObject.Instantiate(ItemPrefab);
    }
}