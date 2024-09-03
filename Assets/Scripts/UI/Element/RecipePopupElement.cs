using UnityEngine;
using UnityEngine.UIElements;

public class RecipePopupElement : MonoBehaviour // Assets/Scripts/UI/Element/RecipePopupElement.cs
{
    [SerializeField] private Image ProductImage;
    [SerializeField] private Transform IngredientLayout;
    public void Init(string recipeName)
    {
        //DataManager.Instance.GetGameData<Recipe>();
    }
}
