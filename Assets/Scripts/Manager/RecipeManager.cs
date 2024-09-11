using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RecipeManager : MonoBehaviour // Assets/Scripts/Manager/RecipeManager.cs
{
    #region Fields
    
    [SerializeField] private RecipePopup recipePopup; // 레시피 UI
    
    private readonly List<RecipeBase> _recipeList = new(); // 레시피 목록
    
    private int _cursor = 0; // 레시피 목록에서, 특정 위치를 지정하기 위한 인덱스 변수
    public RecipeBase CurrentRecipe => _recipeList[_cursor]; // 인덱스가 가리키는 레시피를 가져오는 함수? 프로퍼티?
    
    #endregion Fields
    
    private void Awake()
    {
        
    }
	
    public void Init(StageData stageData)
    {
        /* 레시피의 정보를 매개변수로 받아와서 초기화한다.
         * 레시피의 정보는 아래의 위치에서 가져올 수 있을 것으로 예상된다.
         *   - StageData.TargetFoodIcon
         * RecipeData의 'Index' 데이터를 사용하여 접근해야 한다. (변수명과 Index가 동기화되어 있다.)
         * [TODO]: TargetFoodIcon의 값들 중에 존재하지 않는 값이 적용되어 있는 듯하다: [11000 ~ 11001, DATA_PRODUCT]
         */

        // 스테이지 내에 목표로 하는 레시피가 여러 개일 수 있다.
        string[] targetFoodIcons = stageData.TargetFoodIcon.Split(", "); // ', '으로 구분되어 있을 것.

        foreach (string targetFoodIcon in targetFoodIcons)
        {
            RecipeData recipeData = DataManager.Instance.GetGameData<RecipeData>(targetFoodIcon);

            // 예외 처리
            if (recipeData == null)
            {
                Debug.LogError("레시피를 불러오지 못했습니다. 레시피의 값이 올바르게 작성되어 있는지 확인하세요.");
                return;
            }
            
            // 레시피에 해당하는 재료를 찾는다.
            // TODO: 아래의 코드는 Ingre 데이터들이 FoodData의 Index로 지정되어야 한다. 현재는 Icon/Value에 해당하는 값으로 작성되어 있다.
            FoodData food01 = DataManager.Instance.GetGameData<FoodData>(recipeData.Ingre_01);
            FoodData food02 = DataManager.Instance.GetGameData<FoodData>(recipeData.Ingre_02);
            FoodData food03 = DataManager.Instance.GetGameData<FoodData>(recipeData.Ingre_03);
            FoodData food04 = DataManager.Instance.GetGameData<FoodData>(recipeData.Ingre_04);

            List<FoodData> foodDatas = new() { food01, food02, food03, food04 };

            // 예외 처리
            foreach (FoodData food in foodDatas)
            {
                if (food == null)
                {
                    Debug.LogError("레시피에 해당하는 재료를 불러오지 못했습니다. 재료의 값이 올바르게 작성되어 있는지 확인하세요.");
                }
            }
 
            GameObject recipePrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>(recipeData.Product);
            Instantiate(recipePrefab);
        }
        
        
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
    
    public void Clear()
    {
        _recipeList.Clear();
        _cursor = 0;
    }
}
