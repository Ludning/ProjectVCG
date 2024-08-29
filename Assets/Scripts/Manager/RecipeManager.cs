using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RecipeManager : MonoBehaviour // Assets/Scripts/Manager/RecipeManager.cs
{
    [SerializeField] private RecipePopup recipePopup; // UI?
    private List<RecipeBase> recipes = new List<RecipeBase>();

    public int cursor = 0;
    RecipeBase CurrentRecipe => recipes[cursor];
    
    private void Awake()
    {
        Init();
    }
	
    private void Init()
    {
        // AddRecipe("recipeName");
    }
    
    // = Init(); Stage에서 레시피의 목록을 불러온다.
    private void ReadRecipesFromStage()
    {
        
    }
    
    // 모든 레시피가 완료되었는지 확인하는 프로퍼티
    public bool IsComplete => recipes.All(recipe => CurrentRecipe.IsComplete);

    public void InitRecipeState()
    {
        foreach (var recipe in recipes)
        {
            recipe.Reset();
        }
    }
	
    public void AddRecipe(string recipeName)
    {
        recipes.Add(new RecipeBase(recipeName));
        recipePopup.DisplayRecipe(recipeName);
    }
    public bool CheckRecipe(List<string> tileInventoryItems)
    {
        if(CurrentRecipe.Ingredients.Count != tileInventoryItems.Count)
            return false;
        
        List<string> deepCopiedList = new List<string>(CurrentRecipe.Ingredients);
        
        foreach (var itemName in tileInventoryItems)
        {
            if(!deepCopiedList.Contains(itemName))
                return false;
            deepCopiedList.Remove(itemName);
        }
        return true;
    }

    public void CompleteRecipe(string recipeName)
    {
        CurrentRecipe.IsComplete = true;
        recipePopup.HideRecipe(recipeName);
    }
}
