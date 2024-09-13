using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour // Assets/Scripts/Manager/RecipeManager.cs
{
    #region Fields
    [SerializeField] private LevelPopup levelPopup; // Recipe UI
    
    private readonly List<LevelUnitBase> _levelUnitList = new(); // Recipe List
    
    private int _cursor = 0; // Index for Recipe List
    public LevelUnitBase CurrentLevelUnit => _levelUnitList[_cursor]; // Getter for Index Item of Recipe List
    // 모든 레시피가 완료되었는지 확인하는 프로퍼티
    public bool IsAllComplete => _levelUnitList.All(recipe => CurrentLevelUnit.IsComplete);
    #endregion Fields
    
    public void Init(string stageClearCondition)
    {
        Clear();
        string[] clearIndexs = stageClearCondition.Split(", ");
        foreach (var clearIndex in clearIndexs)
        {
            LevelData data = DataManager.Instance.GetGameData<LevelData>(clearIndex);
            switch (data.Sort_Clear)
            {
                case SortClearType.ARRIVE:
                    AddLevelUnit(SortClearType.ARRIVE, data.Index, data.Tile_Key);
                    break;
                case SortClearType.SERVING:
                    AddLevelUnit(SortClearType.SERVING, data.Index, data.Tile_Key);
                    break;
                case SortClearType.RECIPE:
                    AddLevelUnit(SortClearType.RECIPE, data.Index, data.Recipe_Index);
                    break;
                case SortClearType.RECIPE_CLEAR:
                    AddLevelUnit(SortClearType.RECIPE_CLEAR, data.Index, data.Recipe_Index);
                    break;
            }
        }
    }

    // Recipe 목록에 있는 모든 Recipe의 isComplete를 false로 초기화한다.
    public void ResetRecipeState()
    {
        foreach (LevelUnitBase recipe in _levelUnitList)
        {
            recipe.Reset();
        }
    }
	
    // Recipe 목록에 Recipe를 추가한다.
    private void AddLevelUnit(SortClearType type, string levelKey, string value)
    {
        switch (type)
        {
            case SortClearType.ARRIVE:
                LevelUnitBase arriveUnit = new ArriveUnit(levelKey, value, levelPopup.LevelUIParent);
                _levelUnitList.Add(arriveUnit);
                break;
            case SortClearType.SERVING:
                LevelUnitBase servingUnit = new ServingUnit(levelKey, value, levelPopup.LevelUIParent);
                _levelUnitList.Add(servingUnit);
                break;
            case SortClearType.RECIPE:
                RecipeData recipeData = DataManager.Instance.GetGameData<RecipeData>(value);
                RecursionAddLevelUnit(SortClearType.RECIPE, recipeData.Ingre_01);
                RecursionAddLevelUnit(SortClearType.RECIPE, recipeData.Ingre_02);
                RecursionAddLevelUnit(SortClearType.RECIPE, recipeData.Ingre_03);
                RecursionAddLevelUnit(SortClearType.RECIPE, recipeData.Ingre_04);
                LevelUnitBase recipeUnit = new RecipeUnit(value, levelPopup.LevelUIParent);
                _levelUnitList.Add(recipeUnit);
                break;
            case SortClearType.RECIPE_CLEAR:
                RecipeData recipeClearData = DataManager.Instance.GetGameData<RecipeData>(value);
                RecursionAddLevelUnit(SortClearType.RECIPE, recipeClearData.Ingre_01);
                RecursionAddLevelUnit(SortClearType.RECIPE, recipeClearData.Ingre_02);
                RecursionAddLevelUnit(SortClearType.RECIPE, recipeClearData.Ingre_03);
                RecursionAddLevelUnit(SortClearType.RECIPE, recipeClearData.Ingre_04);
                LevelUnitBase recipeUnit_Clear = new RecipeUnit_Clear(value, levelPopup.LevelUIParent);
                _levelUnitList.Add(recipeUnit_Clear);
                break;
            default:
                return;
        }
    }

    private void RecursionAddLevelUnit(SortClearType type, string itemName)
    {
        if (!string.IsNullOrWhiteSpace(itemName))
        {
            ItemType itemType = StringEnumConverter.ParserStringToEnum<ItemType>(itemName);
            FoodData foodData = DataManager.Instance.GetGameData<FoodData>(((int)itemType).ToString());
            if(!string.IsNullOrWhiteSpace(foodData.Recipe_Index))
                AddLevelUnit(type, null, foodData.Recipe_Index);
        }
    }
    
    public bool CheckLevel(TileBase tile)
    {
        return CurrentLevelUnit.CheakLevel(tile);
    }

    public void CompleteCurrentRecipe()
    {
        CurrentLevelUnit.IsComplete = true;
        //levelPopup.HideRecipe(CurrentLevelUnit.RecipeName);
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
    
    public void Clear()
    {
        foreach (var levelUnit in _levelUnitList)
            levelUnit.Clear();
        _levelUnitList.Clear();
        _cursor = 0;
    }
}
