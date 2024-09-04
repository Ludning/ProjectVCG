using UnityEngine;
using UnityEngine.UIElements;

public class RecipePopupElement : MonoBehaviour // Assets/Scripts/UI/Element/RecipePopupElement.cs
{
    
    [SerializeField] private Image ProductImage;
    [SerializeField] private Transform IngredientLayout;
    // Recipe Base
    private RecipeBase _recipeBase;
    
    // Initialize
    public void Init(string recipeName)
    {
        _recipeBase = new RecipeBase(recipeName);
    }
}
