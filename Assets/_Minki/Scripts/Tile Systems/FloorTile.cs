namespace Minki.TileSystem
{
    // Floor Tile; Player Can Move to Floor Tile.
    public class FloorTile : BaseTile
    {
        private void Awake()
        {
            TypeName = TileType.Floor;
        }
    }
}
