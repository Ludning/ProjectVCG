using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RecipeManager : MonoBehaviour // Assets/Scripts/Manager/RecipeManager.cs
{
    #region Fields
    
    [SerializeField] private RecipePopup recipePopup; // Recipe UI
    
    private readonly List<RecipeBase> _recipeList = new(); // Recipe List
    
    private int _cursor = 0; // Index for Recipe List
    public RecipeBase CurrentRecipe => _recipeList[_cursor]; // Getter for Index Item of Recipe List
    
    #endregion Fields
    
    private void Awake()
    {
        Init();
    }
	
    private void Init()
    {
        // StageData로부터 레시피의 정보를 받아와서 추가한다.
        // 매개변수로 레시피의 정보를 전달받을 예정.
        
        //AddRecipe("recipeName");
    }
    
    // 모든 레시피가 완료되었는지 확인하는 프로퍼티
    public bool IsAllComplete => _recipeList.All(recipe => CurrentRecipe.IsComplete);

    // Recipe 목록에 있는 모든 Recipe의 isComplete를 false로 초기화한다.
    public void ResetRecipeState()
    {
        foreach (RecipeBase recipe in _recipeList)
        {
            recipe.Reset();
        }
    }
	
    // Recipe 목록에 Recipe를 추가한다.
    public void AddRecipe(string recipeName)
    {
        _recipeList.Add(new RecipeBase(recipeName));
        recipePopup.DisplayRecipe(recipeName);
    }
    
    // 
    public bool CheckRecipe(List<string> tileInventoryItems)
    {
        if (CurrentRecipe.Ingredients.Count != tileInventoryItems.Count)
        {
            return false;
        }
        
        List<string> deepCopiedList = new List<string>(CurrentRecipe.Ingredients);
        
        foreach (string itemName in tileInventoryItems)
        {
            if (!deepCopiedList.Contains(itemName))
            {
                return false;
            }
                
            deepCopiedList.Remove(itemName);
        }
        
        return true;
    }

    public void CompleteCurrentRecipe()
    {
        CurrentRecipe.IsComplete = true;
        recipePopup.HideRecipe(CurrentRecipe.RecipeName);
        _cursor++;
    }

    private async UniTask<LogicState> RunBlockLogics()
    {
        return LogicState.Success;
    }

    private async UniTask<LogicState> BlockLogic(BlockLogicBase block)
    {
        return LogicState.Success;
    }
}
