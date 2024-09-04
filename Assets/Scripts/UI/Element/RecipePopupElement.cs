using UnityEngine;

public class RecipePopupElement : MonoBehaviour // Assets/Scripts/UI/Element/RecipePopupElement.cs
{
    // Recipe Base
    private RecipeBase _recipeBase;
    
    // Initialize
    public void Init(string recipeName)
    {
        _recipeBase = new RecipeBase(recipeName);
    }
}
