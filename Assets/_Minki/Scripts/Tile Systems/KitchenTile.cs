// using System.Linq;
// using UnityEngine;
//
// namespace TileSystem
// {
//     // Kitchen Tile; Player Can NOT Move to Kitchen Tile, but Can Cook Food in Front of Here.
//     // [ALERT] At ProtoType: Kitchen Tile will do Action of Ingredient Tile.
//     public class KitchenTile : BaseTile,
//     {
//         // Cooking Ingredients
//         [SerializeField] private GameObject[] requiredIngredients;
//         [SerializeField] private GameObject completeFood;
//         
//         private void Awake()
//         {
//             // Set Tile's Type.
//             TypeName = TileType.Kitchen;
//         }
//
//         // Try to Cook.
//         public GameObject Cook(GameObject[] inHandIngredients)
//         {
//             // If Required Ingredients are equal to Handed Ingredients,
//             if (requiredIngredients.SequenceEqual(inHandIngredients))
//             {
//                 // Return Complete(Cooked) Food.
//                 return completeFood;
//             }
//             // If not,
//             else
//             {
//                 // Return Nothing.
//                 return null;
//             }
//         }
//     }
// }
