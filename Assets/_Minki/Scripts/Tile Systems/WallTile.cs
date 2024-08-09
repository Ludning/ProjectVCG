using System;

namespace Minki.TileSystem
{
    // Wall Tile; Player Can NOT Move to Wall Tile.
    public class WallTile : BaseTile
    {
        private void Awake()
        {
            TypeName = TileType.Wall;
        }
    }
}
