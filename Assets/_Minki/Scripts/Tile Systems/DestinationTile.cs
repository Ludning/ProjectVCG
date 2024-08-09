using System;

namespace Minki.TileSystem
{
    // Destination Tile; Player Clears the Stage if they Reach Here.
    public class DestinationTile : BaseTile, IWalkable
    {
        private void Awake()
        {
            // Set Tile's Type.
            TypeName = TileType.Destination;
        }

        // Interface Method
        // TODO: Will use just Method, or Collision Event?
        public bool Walk()
        {
            // Call to GameManager that Player cleared this Stage!
            return true;
        }
    }
}
