namespace Minki.TileSystem
{
    // Wall Tile; Player Can NOT Move to Wall Tile.
    public class WallTile : BaseTile, INotWalkable
    {
        private void Awake()
        {
            // Set Tile's Type.
            TypeName = TileType.Wall;
        }
        
        // Interface Method
        public bool Walk()
        {
            return false;
        }
    }
}
