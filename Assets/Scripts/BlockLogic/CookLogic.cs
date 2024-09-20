using System.Collections.Generic;
using UnityEngine;

public class CookLogic : BlockLogicBase
{
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
        if (!owner.TableManager.TryGetTileItemNameList(position, out List<ItemType> itemTypeList))
            return ErrorType.UnKnownError;
        
        //레벨 체크 (이번 조리가 레시피랑 일치한지)
        TileBase tile = owner.TableManager.PeekTile(position);
        if(!owner.levelManager.CheckLevel(tile))
            return ErrorType.InvalidCookCombo;
        
        
        return ErrorType.NoError;
    }

    public override LogicState Execute(StageManager owner)
    {
        var position = owner.Controller.PlayerForwardPosition;
        TileBase tile =  owner.TableManager.PeekTile(position);

        tile.ClearItem();

        switch (owner.levelManager.CurrentLevelUnit)
        {
            case RecipeUnit_Clear recipeUnit_Clear:
                tile.SetItem(recipeUnit_Clear.SpawnProductFood().GetComponent<ItemBase>());
                recipeUnit_Clear.OnComplete();
                break;
            case RecipeUnit recipeUnit:
                tile.SetItem(recipeUnit.SpawnProductFood().GetComponent<ItemBase>());
                break;
            default:
                return LogicState.Failure;
        }
        owner.levelManager.CompleteCurrentRecipe();
        return LogicState.Success;
    }
    public override void CheakClear(StageManager owner)
    {
        var cookTilePosition = owner.Controller.PlayerForwardPosition;
        TileBase tile = owner.TableManager.PeekTile(cookTilePosition);
        if(owner.levelManager.CheckLevel(tile) == true)
            owner.levelManager.CompleteCurrentRecipe();
    }
}