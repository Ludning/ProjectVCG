using System.Collections.Generic;
using Minki.FoodSystem;
using UnityEngine;

namespace Minki.TileSystem
{
    // A Top-Level Class for Tile Item Classes.
    public class BaseTile : MonoBehaviour
    {
        // Tile Type; Defines What Type of the Tile is.
        public TileType TypeName { get; protected set; }
    }

    #region Interfaces
    
    // Can Walk
    public interface IWalkable
    {
        bool Walk();
    }

    // Can NOT Walk
    public interface INotWalkable
    {
        bool Walk();
    }
    
    // Can Lift Food (or Ingredient).
    public interface ILiftable
    {
        BaseFood Lift(); // Lift Food or Ingredient.
        void Lift(Queue<BaseFood> inHand); // Lift Food or Ingredient. (If Using Queue<T>)
    }

    // Can Drop Food (or Ingredient).
    public interface IDropable
    {
        // [Alert]: Player Drops with LIFO(Last In, First Out) Rules.
        void Drop(BaseFood foodInHand); // Drop Food or Ingredient.
        void Drop(Queue<BaseFood> foodInHand); // Drop Food or Ingredient. (If Using Queue<T>)
    }
    
    // Can Cook
    // public interface ICookable
    // {
    //     GameObject Cook(GameObject[] inHandIngredients);
    // }

    #endregion Interfaces
}
