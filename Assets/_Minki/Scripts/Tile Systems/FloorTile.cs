namespace Minki.TileSystem
{
    // Floor Tile; Player Can Move to Floor Tile.
    public class FloorTile : BaseTile, IWalkable
    {
        private void Awake()
        {
            // Set Tile's Type.
            TypeName = TileType.Floor;
        }

        // Interface Method
        public bool Walk()
        {
            return true;
        }
    }
}
