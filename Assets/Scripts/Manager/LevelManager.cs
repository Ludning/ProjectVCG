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
    #endregion Fields
    
    public void Init(string stageClearCondition)
    {
        string[] clearIndexs = stageClearCondition.Split(", ");
        foreach (var clearIndex in clearIndexs)
        {
            LevelData data = DataManager.Instance.GetGameData<LevelData>(clearIndex);
            AddLevelUnit(data);
        }
    }
    
    // 모든 레시피가 완료되었는지 확인하는 프로퍼티
    public bool IsAllComplete => _levelUnitList.All(recipe => CurrentLevelUnit.IsComplete);

    // Recipe 목록에 있는 모든 Recipe의 isComplete를 false로 초기화한다.
    public void ResetRecipeState()
    {
        foreach (LevelUnitBase recipe in _levelUnitList)
        {
            recipe.Reset();
        }
    }
	
    // Recipe 목록에 Recipe를 추가한다.
    public void AddLevelUnit(LevelData data)
    {
        switch (data.Sort_Clear)
        {
            case SortClearType.ARRIVE:
                LevelUnitBase arriveUnit = new ArriveUnit(data);
                _levelUnitList.Add(arriveUnit);
                break;
            case SortClearType.SERVING:
                LevelUnitBase servingUnit = new ServingUnit(data);
                _levelUnitList.Add(servingUnit);
                break;
            case SortClearType.RECIPE:
                LevelUnitBase recipeUnit = new RecipeUnit(data);
                _levelUnitList.Add(recipeUnit);
                break;
            case SortClearType.RECIPE_CLEAR:
                LevelUnitBase recipeUnit_Clear = new RecipeUnit_Clear(data);
                _levelUnitList.Add(recipeUnit_Clear);
                break;
            default:
                return;
        }
        //levelPopup.DisplayLevel(data);
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
