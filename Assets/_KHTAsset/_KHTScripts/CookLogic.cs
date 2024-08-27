public class CookLogic : BlockLogicBase
{
    public override bool IsExecutable(StageManager owner)
    {
        var position = owner.Controller.PlayerForwardPosition;
        TileType tileType = owner.Table.GetTileType(position);
        ItemBase item = owner.Table.GetTileItem(position);

        return (tileType == TileType.Kitchen && item != null) ? true : false;
    }

    public override bool Execute(StageManager owner)
    {
        //if ()
        //{

        //    return true;
        //}
        return false;
    }
}
