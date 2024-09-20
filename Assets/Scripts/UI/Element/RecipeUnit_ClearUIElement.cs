using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeUnit_ClearUIElement : LevelUnitUIElement
{
    [SerializeField] private Image ProductImage;
    [SerializeField] private Image CookingPropertyImage;
    [SerializeField] private Transform IngredientLayout;
    public override void Init(string recipeIndex, NodeData nodeData = null)
    {
        RecipeData recipeData = DataManager.Instance.GetGameData<RecipeData>(recipeIndex);

        Sprite productSprite = LoadFoodSprite(recipeData.Product);
        if(productSprite != null)
            ProductImage.sprite = productSprite;
        
        Sprite cookerySprite = LoadCookingPropertySprite(recipeData.Cookery);
        if(cookerySprite != null)
            CookingPropertyImage.sprite = cookerySprite;

        GameObject ingredientPrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>("IngredientUIElement");

        InstantiateIngredientUI(ingredientPrefab, recipeData.Ingre_01);
        InstantiateIngredientUI(ingredientPrefab, recipeData.Ingre_02);
        InstantiateIngredientUI(ingredientPrefab, recipeData.Ingre_03);
        InstantiateIngredientUI(ingredientPrefab, recipeData.Ingre_04);
    }
    private void InstantiateIngredientUI(GameObject prefab, ItemType itemType)
    {
        if (itemType == ItemType.NULL)
            return;
        GameObject ingredientUIElement = Instantiate(prefab, IngredientLayout);
        Image image = ingredientUIElement.GetComponent<Image>();
        image.sprite = LoadFoodSprite(itemType);
    }
    private Sprite LoadFoodSprite(ItemType itemType)
    {
        if (itemType == ItemType.NULL)
            return null;
        FoodData foodData = DataManager.Instance.GetGameData<FoodData>(((int)itemType).ToString());
        Sprite sprite = ResourceManager.Instance.LoadResourceWithCaching<Sprite>(foodData.Icon);
        return sprite;
    }
    private Sprite LoadCookingPropertySprite(CookingPropertyType cookingPropertyType)
    {
        if (cookingPropertyType == CookingPropertyType.NULL)
            return null;
        CookingPropertyData cookingPropertyData = DataManager.Instance.GetGameData<CookingPropertyData>(((int)cookingPropertyType).ToString());
        Sprite sprite = ResourceManager.Instance.LoadResourceWithCaching<Sprite>(cookingPropertyData.IconName);
        return sprite;
    }
}
