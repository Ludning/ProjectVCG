using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RecipeManager : MonoBehaviour // Assets/Scripts/Manager/RecipeManager.cs
{
    [SerializeField] private RecipePopup recipePopup; // UI?
    private Dictionary<string, RecipeBase> recipes = new(); // Recipe 사전
    
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
    public bool IsComplete => recipes.All(recipe => recipe.Value.IsComplete);

    public void InitRecipeState()
    {
        foreach (KeyValuePair<string, RecipeBase> recipe in recipes)
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
        if (recipes.TryGetValue(recipeName, out RecipeBase recipe))
        {
            recipe.IsComplete = true;
            recipePopup.HideRecipe(recipeName);
        }
    }
}
