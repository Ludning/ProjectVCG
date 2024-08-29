public class CookLogic : BlockLogicBase
{
    public override ErrorType IsExecutable(StageManager owner)
    {
        var position = owner.Controller.PlayerForwardPosition;
        TileType tileType = owner.Table.GetTileType(position);
        ItemBase item = owner.Table.GetTileItem(position);

        return (tileType == TileType.Kitchen && item != null) ? ErrorType.NoError : ErrorType.InvalidCookCombo;
    }

    public override LogicState Execute(StageManager owner)
    {
        //if ()
        //{

        //    return true;
        //}
        return LogicState.Success;
    }
}
