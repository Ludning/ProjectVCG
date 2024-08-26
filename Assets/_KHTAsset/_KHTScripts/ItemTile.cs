using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemTile : TileBase
{
    [SerializeField] GameObject ItemPrefab;

    //private void Update()
    //{
    //    if(Item == null)
    //        OnItemSpwan();
    //}
    public override void OnItemSpwan()
    {
        GameObject.Instantiate(ItemPrefab);
    }
}