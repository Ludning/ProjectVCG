using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TileBase : MonoBehaviour
{
    //타일의 속성(열거형)
    public TileType TileType;
    public TileAttributeType TileAttributeType;
    public CookingPropertyType CookingPropertyType;

    public string LevelKey;
    private int InventoryCount = 0;

    public GameObject IngredientItem;
    public SpriteRenderer spriteRenderer;

    public Stack<ItemBase> InventoryStack = new Stack<ItemBase>();

    public bool IsWalkAble => (TileAttributeType & TileAttributeType.Moveable) != 0;
    public bool IsPushAble => (TileAttributeType & TileAttributeType.Pushable) != 0;
    public bool IsPopAble  => (TileAttributeType & TileAttributeType.Popable)  != 0;
    public bool IsStackAble => (TileAttributeType & TileAttributeType.Stackable) != 0;
    public bool IsCookAble => (TileAttributeType & TileAttributeType.Cookable) != 0;
    public bool IsInventoryEmpty => (InventoryCount == 0) ? true : false;
    public bool IsInventoryEmptyOrFull => (InventoryCount == 0 || InventoryStack.Count == InventoryCount) ? true : false;

    private ItemHintUI _itemHintUI;
    
    public void InitTile(NodeData nodeData, string levelIndex)
    {
        LevelKey = levelIndex;
        string tileKey = ((int)nodeData.TileType).ToString();
        TileData tileData = DataManager.Instance.GetGameData<TileData>(tileKey);

        TileType = tileData.TileType;
        TileAttributeType =
            (tileData.Moveable ? TileAttributeType.Moveable : 0) |
            (tileData.Pushable ? TileAttributeType.Pushable : 0) |
            (tileData.Popable ? TileAttributeType.Popable : 0) |
            (tileData.Stackable ? TileAttributeType.Stackable : 0) |
            (tileData.TileType == TileType.COOKING ? TileAttributeType.Cookable : 0);
        CookingPropertyType = nodeData.CookingPropertyType;
        InventoryCount = tileData.InventoryCount;
       
        //TileLogic 설치
        InitTileMesh(tileData.PrefabName, nodeData);
    }

    public void InitTileMesh(string tileName, NodeData nodeData)
    {
        if (TileType == TileType.EMPTY)
            return;

        GameObject tilePrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>(tileName);
        GameObject go = Instantiate(tilePrefab, transform);

            // 자식 오브젝트에서 Outline 컴포넌트 확인 및 추가
        if (tileName == "Walking_Tile" || tileName == "Walking_Pass_Tile")
        {
            GameObject OutLinePrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>("OutLine");
            GameObject OutLineObject = Instantiate(OutLinePrefab, transform);
            OutLineObject.transform.localPosition = Vector3.up * 1.01f;
            spriteRenderer = OutLineObject.GetComponent<SpriteRenderer>();
        }

        go.transform.localPosition = Vector3.zero;

        if (nodeData.TileType == TileType.COOKING)
        {
            CookingPropertyData data = DataManager.Instance.GetGameData<CookingPropertyData>(((int)nodeData.CookingPropertyType).ToString());
            GameObject prefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>(data.PrefabName);
            GameObject cookingObject = Instantiate(prefab, go.transform);
        }
    }

    public TileAttributeType GetTileAttributes(bool walkable, bool pushable, bool popable, bool stackable)
    {
        TileAttributeType attributes = TileAttributeType.None;

        if (walkable)
        {
            attributes |= TileAttributeType.Moveable;
        }
        if (pushable)
        {
            attributes |= TileAttributeType.Pushable;
        }
        if (popable)
        {
            attributes |= TileAttributeType.Popable;
        }
        if (stackable)
        {
            attributes |= TileAttributeType.Stackable;
        }

        return attributes;
    }

    public List<ItemType> GetItemTypeList()
    {
        List<ItemType> itemTypes = new List<ItemType>();

        foreach (ItemBase item in InventoryStack)
        {
            itemTypes.Add(item.ItemType);
        }

        return itemTypes;
    }
    //아이템을 가져가는 함수
    public ItemBase TakeItem()
    {
        if (InventoryCount == -1)
        {
            if (Instantiate(IngredientItem).TryGetComponent(out ItemBase itemBase))
                return itemBase;
            Debug.Log("Error TakeItem DisplayMesh Dont Have ItemBase Component");
            return null;
        }

        if (InventoryStack.Count <= 0)
            return null;

        ItemBase item = InventoryStack.Pop();
        _itemHintUI.RemoveImage();
        if (InventoryStack.Count == 0)
            _itemHintUI.gameObject.SetActive(false);
        return item;
    }

    //아이템을 놓는 함수
    public void SetItem(ItemBase item, bool display = true)
    {
        InventoryStack.Push(item);
        item.transform.SetParent(transform, false);
        item.transform.localPosition = Vector3.up * GameManager.Instance.ItemPositionAdditive;

        if (display == false)
            return;
        
        if (_itemHintUI == null)
            InstantiateItemHintUI();
        
        _itemHintUI.gameObject.SetActive(true);
        _itemHintUI.PushImage(item.ItemType);
    }
    public void SetDisplayItem(ItemBase item)
    {
        item.transform.SetParent(transform, false);
        item.transform.localPosition = Vector3.up * GameManager.Instance.ItemPositionAdditive;
    }

    public void ChangeCurrentTileSpriteColor(bool change)
    {
        if(spriteRenderer!=null)
            spriteRenderer.color = (change == true) ? Color.red : Color.blue;
    }

    private void InstantiateItemHintUI()
    {
        Debug.Log("Instantiate ItemHint UI");
        GameObject itemHintUIPrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>("ItemHintUI");
        _itemHintUI = Instantiate(itemHintUIPrefab, transform, false).GetComponent<ItemHintUI>();
    }
    public void ClearItem()
    {
        foreach (var itemBase in InventoryStack)
            Destroy(itemBase.gameObject);
        if(IngredientItem != null)
            Destroy(IngredientItem);
        InventoryStack.Clear();
        Debug.Log("ClearTileItem");

        if (_itemHintUI != null)
        {
            _itemHintUI.Clear();
            _itemHintUI = null;
        }
    }
    public void OnItemSpawn()
    {

    }
    public void HighlightTile()
    {
        
    }
    public void UnhighlightTile()
    {
        
    }
}
