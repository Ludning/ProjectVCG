using System.Collections.Generic;
using Minki.FoodSystem;
using UnityEngine;

namespace Minki.TileSystem
{
    // Customer Tile; Player Can NOT move to Customer Tile, but Can Serve Cooked Food in Front of Here.
    public class CustomerTile : BaseTile, IDropable
    {
        [SerializeField] private BaseFood desiredFood;
        
        private void Awake()
        {
            // Set Tile's Type.
            TypeName = TileType.Customer;
        }

        #region Interface Methods
        
        // This Method will be Called by Player; Player Serves the Food in Hand to Customer.
        public void Drop(BaseFood foodInHand)
        {
            if (foodInHand.TypeName == desiredFood.TypeName)
            {
                // Let GameManager know that Player Cleared this Stage.
            }
        }

        // If Using Queue<T>,
        public void Drop(Queue<BaseFood> foodInHand)
        {
            if (foodInHand.Dequeue().TypeName == desiredFood.TypeName)
            {
                // Let GameManager know that Player Cleared this Stage.
            }
        }
        
        #endregion Interface Methods
    }
}
