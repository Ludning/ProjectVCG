using System;

namespace Minki.TileSystem
{
    // Kitchen Tile; Player Can NOT Move to Kitchen Tile, but Can Cook Food in Front of Here.
    public class KitchenTile : BaseTile
    {
        private void Awake()
        {
            TypeName = TileType.Kitchen;
        }
    }
}
