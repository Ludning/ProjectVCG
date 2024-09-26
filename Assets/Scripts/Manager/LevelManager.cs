using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour // Assets/Scripts/Manager/RecipeManager.cs
{
    #region Fields
    [SerializeField] private LevelPopup levelPopup; // Recipe UI
    
    private readonly List<LevelUnitBase> _levelUnitList = new(); // Recipe List

    [SerializeField] private HorizontalLayoutGroup _horizontalLayoutGroup;
    
    private int _cursor = 0; // Index for Recipe List
    public LevelUnitBase CurrentLevelUnit => _levelUnitList[_cursor]; // Getter for Index Item of Recipe List
    // 모든 레시피가 완료되었는지 확인하는 프로퍼티
    public bool IsAllComplete => _levelUnitList.All(recipe => recipe.IsComplete);
    #endregion

    #region 초기화
    public void Init(string stageClearCondition)
    {
        StageData stageData = GameManager.Instance.GetCurrentStageData();
        MapData mapData = DataManager.Instance.GetMapData();
        TableData tableData = mapData.TableData[stageData.Index];
        
        Debug.Log("Init Start");
        Clear();
        string[] clearIndexs = stageClearCondition.Split(", ");
        foreach (var clearIndex in clearIndexs)
        {
            LevelData levelData = DataManager.Instance.GetGameData<LevelData>(clearIndex);
            
            switch (levelData.Sort_Clear)
            {
                case SortClearType.ARRIVE:
                    AddArriveUnit(levelData.Index, levelData.Tile_Key);
                    break;
                case SortClearType.SERVING:
                    var locationKey = tableData.LevelDataDictionary.FirstOrDefault(x => x.Value == levelData.Tile_Key).Key;
                    NodeData nodeData = tableData.Table[locationKey];
                    AddServingUnit(levelData.Index, levelData.Tile_Key, nodeData);
                    break;
                case SortClearType.RECIPE:
                    AddRecipeUnit(levelData.Recipe_Index, levelData.Tile_Key);
                    break;
                case SortClearType.RECIPE_CLEAR:
                    AddRecipeClearUnit(levelData.Recipe_Index, levelData.Tile_Key);
                    break;
            }
        }
        Debug.Log("Init End");
    }
    private void AddArriveUnit(string levelKey, string tileKey)
    {
        LevelUnitBase arriveUnit = new ArriveUnit(levelKey, tileKey, levelPopup.LevelUIParent);
        _levelUnitList.Add(arriveUnit);
    }
    private void AddServingUnit(string levelKey, string tileKey, NodeData nodeData)
    {
        LevelUnitBase servingUnit = new ServingUnit(levelKey, tileKey, levelPopup.LevelUIParent, nodeData);
        _levelUnitList.Add(servingUnit);
    }
    private void AddRecipeUnit(string recipeIndex, string tileKey)
    {
        //RecipeData recipeData = DataManager.Instance.GetGameData<RecipeData>(recipeIndex);
        //AddIngreRecipeUnit(recipeData.Ingre_01);
        //AddIngreRecipeUnit(recipeData.Ingre_02);
        //AddIngreRecipeUnit(recipeData.Ingre_03);
        //AddIngreRecipeUnit(recipeData.Ingre_04);

        LevelUnitBase recipeUnit = new RecipeUnit(recipeIndex, tileKey, levelPopup.LevelUIParent);
        _levelUnitList.Add(recipeUnit);
    }
    private void AddRecipeClearUnit(string recipeIndex, string tileKey)
    {
        //RecipeData recipeClearData = DataManager.Instance.GetGameData<RecipeData>(recipeIndex);
        //AddIngreRecipeUnit(recipeClearData.Ingre_01);
        //AddIngreRecipeUnit(recipeClearData.Ingre_02);
        //AddIngreRecipeUnit(recipeClearData.Ingre_03);
        //AddIngreRecipeUnit(recipeClearData.Ingre_04);

        LevelUnitBase recipeUnit_Clear = new RecipeUnit_Clear(recipeIndex, tileKey, levelPopup.LevelUIParent);
        _levelUnitList.Add(recipeUnit_Clear);
    }
    private void AddIngreRecipeUnit(ItemType itemType)
    {
        if (itemType == ItemType.NULL)
            return;
        ItemType ingreType = itemType;
        string ingreIndex = ((int)ingreType).ToString();
        FoodData ingreData = DataManager.Instance.GetGameData<FoodData>(ingreIndex);
        if (ingreData.FoodType != FoodType.ORIGINAL)
            AddRecipeUnit(ingreData.Recipe_Index, "");
    }
    // Recipe 목록에 있는 모든 Recipe의 isComplete를 false로 초기화한다.
    public void ResetLevelState()
    {
        _cursor = 0;
        foreach (LevelUnitBase levelUnit in _levelUnitList)
            levelUnit.Reset();
    }
    public void Clear()
    {
        foreach (var levelUnit in _levelUnitList)
            levelUnit.Clear();
        levelPopup.Reset();
        _levelUnitList.Clear();
        _cursor = 0;
    }
    #endregion
    
    public bool CheckLevel(TileBase tile, BlockLogicType type)
    {
        //Debug.Log($"Cursor {_cursor}");
        //Debug.Log($"Count  {_levelUnitList.Count}");
        if (_levelUnitList.Count <= _cursor)
        {
            Debug.Log("Out of Range");
            return false;
        }
        return CurrentLevelUnit.CheakLevel(tile, type);
    }

    public void CompleteCurrentLevel()
    {
        if (_levelUnitList.Count <= _cursor)
            return;
        Debug.Log("CompleteCurrentLevel********************");
        CurrentLevelUnit.OnComplete();
        _cursor++;
    }
}
