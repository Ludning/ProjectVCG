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
        //var position = owner.Controller.PlayerForwardPosition;
        //CookType cookType = owner.Table.GetCookType(position);
        //var item = owner.Table.GetTileItem(position);
        //if (cookType==CookType.Chop)
        //{
        //    owner.Table.SetTileItem(position, null);
        //    return LogicState.Success;
        //}
        return LogicState.Failure;
    }
}
