public class CookLogic : BlockLogicBase
{
    public override bool IsExecutable(StageManager owner)
    {
        var position = owner.Controller.PlayerForwardPosition;
        TileType tileType = owner.Table.GetTileType(position);
        CookType cookType = owner.Table.GetCookType(position);
        ItemBase item = owner.Table.GetTileItem(position);

        return (tileType == TileType.Kitchen && item != null) ? true : false;
    }

    public override LogicState Execute(StageManager owner)
    {
        
        return LogicState.Failure;
    }
}
