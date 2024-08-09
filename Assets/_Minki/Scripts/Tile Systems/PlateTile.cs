using System;

namespace Minki.TileSystem
{
    // Plate Tile; Player Can NOT Move to Plate Tile, but Can Move Cooked Food to Here.
    public class PlateTile : BaseTile
    {
        private void Awake()
        {
            TypeName = TileType.Plate;
        }
    }
}
