using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CookLogic : BlockLogicBase
{
    private float _runningTime = 1f;
    public override ErrorType IsExecutable(StageManager owner)
    {
        var position = owner.Controller.PlayerForwardPosition;

        //타일 존재 체크
        if(owner.TableManager.PeekTile(position) == null)
            return ErrorType.NoTile; 
        
        //지형 체크
        if(!owner.TableManager.PeekTile(position).IsCookAble)
            return ErrorType.InvalidCookAction;
        
        //비어있는건지 체크
        if (owner.TableManager.PeekTile(position).IsInventoryEmpty)
            return ErrorType.NoIngredientOnTile;

        //타일 아이템 리스트 반환
        if (!owner.TableManager.TryGetTileItemTypeList(position, out List<ItemType> itemTypeList))
            return ErrorType.UnKnownError;
        
        //레시피 체크 (해당 아이템들이 레시피에 있는지)
        TileBase tile = owner.TableManager.PeekTile(position);
        if(GameManager.Instance.FindPossibleRecipes(tile.GetItemTypeList(), tile.CookingPropertyType) == ItemType.NULL)
            return ErrorType.InvalidCookCombo;

        _runningTime = 1f;
        owner.Controller.SetAnimationState(AnimationState.IsCook, true);
        return ErrorType.NoError;
    }

    public override LogicState Execute(StageManager owner)
    {
        if (_runningTime > 0)
        {
            _runningTime -= Time.deltaTime;
            return LogicState.Running;
        }
        
        var position = owner.Controller.PlayerForwardPosition;
        TileBase tile =  owner.TableManager.PeekTile(position);
        
        ItemType productFood = GameManager.Instance.FindPossibleRecipes(tile.GetItemTypeList(), tile.CookingPropertyType);
        FoodData foodData = DataManager.Instance.GetGameData<FoodData>(((int)productFood).ToString());
        GameObject foodPrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>(foodData.PrefabName);
        GameObject food = Instantiate(foodPrefab);
        
        tile.ClearItem();
        tile.SetItem(food.GetComponent<ItemBase>());
        
        owner.Controller.SetAnimationState(AnimationState.IsCook, true);
        return LogicState.Success;
    }
    public override void CheakClear(StageManager owner)
    {
        var cookTilePosition = owner.Controller.PlayerForwardPosition;
        TileBase tile = owner.TableManager.PeekTile(cookTilePosition);
        if (owner.levelManager.CheckLevel(tile, BlockLogicType.Cook) == true)
        {
            if(owner.levelManager.CurrentLevelUnit is RecipeUnit_Clear recipeUnit_Clear)
                recipeUnit_Clear.Async_ClearProductWaitForSecond(tile).Forget();
            owner.levelManager.CompleteCurrentLevel();
        }
    }
}