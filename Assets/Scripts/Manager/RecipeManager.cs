using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeManager : MonoBehaviour
{
    [SerializeField] private RecipePopup recipePopup;
    private Dictionary<string, RecipeBase> recipes = new Dictionary<string, RecipeBase>();

    //모든 레시피가 완료되었는지 확인하는 프로퍼티
    public bool IsComplete
    {
        get
        {
            foreach (var recipe in recipes)
            {
                if(recipe.Value.isComplete == false)
                    return false;
            }
            return true;
        }
    }
    public void InitRecipeState()
    {
        foreach (var recipe in recipes)
        {
            recipe.Value.Reset();
        }
    }
    
    public void AddRecipe(string recipeName)
    {
        recipes.Add(recipeName, new RecipeBase(recipeName));
        recipePopup.DisplayRecipe(recipeName);
    }

    public void CompleteRecipe(string recipeName)
    {
        if(recipes.TryGetValue(recipeName, out RecipeBase recipe))
        {
            recipe.isComplete = true;
            recipePopup.HideRecipe(recipeName);
        }
    }
}
