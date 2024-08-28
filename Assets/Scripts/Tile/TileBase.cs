using UnityEngine;

public class TileBase : MonoBehaviour
{
    //타일의 속성(열거형)
    public TileType TileType;
    
    //
    
    //아이템
    public ItemBase Item;

    public void InitTile(TileData tileData)
    {
        TileType = tileData.tileType;
    }

    public virtual void OnSetItemExcute()
    {

    }
    public virtual void OnItemSpwan()
    {

    }

    public void HighlightTile()
    {
        
    }
    public void UnhighlightTile()
    {
        
    }
}
