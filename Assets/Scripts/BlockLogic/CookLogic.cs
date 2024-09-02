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
        if(!owner.TableManager.PeekTile(position).CheakTileAttribute(TileAttributeCheckType.CookAble))
            return ErrorType.InvalidCookAction;
        
        //비어있는건지 체크
        if (owner.TableManager.PeekTile(position).CheakTileAttribute(TileAttributeCheckType.InventoryEmpty))
            return ErrorType.NoIngredientOnTile;

        //타일 아이템 리스트 반환
        if (!owner.TableManager.TryGetTileItemNameList(position, out List<string> itemNameList))
            return ErrorType.UnKnownError;
        
        //레시피 체크
        if(owner.RecipeManager.CheckRecipe(itemNameList) == false)
            return ErrorType.InvalidCookCombo;
        
        
        return ErrorType.NoError;
    }

    public override LogicState Execute(StageManager owner)
    {
        var position = owner.Controller.PlayerForwardPosition;
        TileBase tile =  owner.TableManager.PeekTile(position);
        string resultName = owner.RecipeManager.CurrentRecipe.Result;

        GameObject foodPrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>(resultName);

        tile.ClearItem();
        GameObject food = Instantiate(foodPrefab);
        tile.SetItem(food.GetComponent<ItemBase>());
        //TODO
        owner.RecipeManager.CompleteCurrentRecipe();
        return LogicState.Success;
    }
}