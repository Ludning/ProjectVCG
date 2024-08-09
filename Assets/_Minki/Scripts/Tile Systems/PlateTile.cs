using System.Collections.Generic;
using Minki.FoodSystem;
using UnityEngine;

namespace Minki.TileSystem
{
    // Plate Tile; Player Can NOT Move to Plate Tile, but Can Lift Food on Plate Tile.
    public class PlateTile : BaseTile, ILiftable
    {
        // A List of Plated Food.
        // [SerializeField]: Plate Tile can Have Complete Food Already on Start at some Stages(Such as Tutorial).
        [SerializeField] private BaseFood platedFood;
        public BaseFood PlatedFood
        {
            set => platedFood = value;
        }
        
        private void Awake()
        {
            // Set Tile's Type.
            TypeName = TileType.Plate;
        }
        
        #region Interface Methods

        // This Method will be Called by Player; Player Lifts All the Food on Plate.
        public BaseFood Lift()
        {
            // Player Gets the Plated Food Data.
            return platedFood;
        }

        public void Lift(Queue<BaseFood> inHand)
        {
            // Player Gets the Plated Food Data into Hand.
            inHand.Enqueue(platedFood);
        }
        
        #endregion Interface Methods
    }
}
