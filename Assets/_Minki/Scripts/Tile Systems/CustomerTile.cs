using System;

namespace Minki.TileSystem
{
    // Customer Tile; Player Can NOT move to Customer Tile, but Can Serve Cooked Food in Front of Here.
    public class CustomerTile : BaseTile
    {
        private void Awake()
        {
            TypeName = TileType.Customer;
        }
    }
}
