using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeUnitUIElement : LevelUnitUIElement
{
    [SerializeField] private Image ProductImage;
    [SerializeField] private Image CookingPropertyImage;
    [SerializeField] private Transform IngredientLayout;
    public override void Init(string recipeIndex)
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

    private void InstantiateIngredientUI(GameObject prefab, string itemName)
    {
        if (string.IsNullOrWhiteSpace(itemName))
            return;
        GameObject ingredientUIElement = Instantiate(prefab, IngredientLayout);
        Image image = ingredientUIElement.GetComponent<Image>();
        image.sprite = LoadFoodSprite(itemName);
    }
    private Sprite LoadFoodSprite(string itemName)
    {
        if (string.IsNullOrWhiteSpace(itemName))
            return null;
        ItemType foodType = StringEnumConverter.ParserStringToEnum<ItemType>(itemName);
        FoodData foodData = DataManager.Instance.GetGameData<FoodData>(((int)foodType).ToString());
        Sprite sprite = ResourceManager.Instance.LoadResourceWithCaching<Sprite>(foodData.Icon);
        return sprite;
    }
    private Sprite LoadCookingPropertySprite(string cookeryName)
    {
        if (string.IsNullOrWhiteSpace(cookeryName))
            return null;
        CookingPropertyType cookingPropertyType = StringEnumConverter.ParserStringToEnum<CookingPropertyType>(cookeryName);
        CookingPropertyData cookingPropertyData = DataManager.Instance.GetGameData<CookingPropertyData>(((int)cookingPropertyType).ToString());
        Debug.Log(cookingPropertyData.IconName);
        Sprite sprite = ResourceManager.Instance.LoadResourceWithCaching<Sprite>(cookingPropertyData.IconName);
        return sprite;
    }
}
