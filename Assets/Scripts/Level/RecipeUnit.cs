using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//레시피 데이터대로 재료를 완성품으로 교체하는 기능단위
public class RecipeUnit : LevelUnitBase
{
    protected string _recipeIndex;
    protected RecipeData recipeData; // Recipe의 이름
    protected GameObject productFood;
    public List<ItemType> Ingredients = new List<ItemType>();
    
    /*public string RecipeName { get; private set; } // Recipe의 이름
    public List<string> Ingredients = new List<string>();
    public string ResultItem;*/

    public RecipeUnit(string recipeIndex, Transform uiParent)
    {
        _recipeIndex = recipeIndex;
        RecipeData recipeData = DataManager.Instance.GetGameData<RecipeData>(_recipeIndex);
        this.recipeData = recipeData;
        
        if(recipeData.Ingre_01 != ItemType.NULL)
            Ingredients.Add(recipeData.Ingre_01);
        if(recipeData.Ingre_02 != ItemType.NULL)
            Ingredients.Add(recipeData.Ingre_02);
        if(recipeData.Ingre_03 != ItemType.NULL)
            Ingredients.Add(recipeData.Ingre_03);
        if(recipeData.Ingre_04 != ItemType.NULL)
            Ingredients.Add(recipeData.Ingre_04);
        
        InitUIElement(uiParent);
        Reset();
    }
    protected virtual void InitUIElement(Transform uiParent)
    {
        GameObject prefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>("RecipeUnitUIElement");
        GameObject uiObject = Object.Instantiate(prefab, uiParent);
        RecipeUnitUIElement temp = uiObject.GetComponent<RecipeUnitUIElement>();
        temp.Init(_recipeIndex);
        LevelUnitUIElement = temp;
    }
    public override bool CheakLevel(TileBase tileBase)
    {
        if (Ingredients.Count != tileBase.InventoryStack.Count)
            return false;
        
        List<ItemType> deepCopiedList = new List<ItemType>(Ingredients);
        
        foreach (ItemBase item in tileBase.InventoryStack)
        {
            if (!deepCopiedList.Contains(item.ItemType))
                return false;
            deepCopiedList.Remove(item.ItemType);
        }
        
        return true;
    }
    public GameObject SpawnProductFood()
    {
        GameObject prefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>(recipeData.Product.ToString());
        //TODO
        //아이템의 위치를 지정해줘야함
        productFood = Object.Instantiate(prefab);
        return prefab;
    }
}
