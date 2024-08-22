using UnityEngine;

namespace FoodSystem
{
    // [Temporary] Food Systems
    public enum FoodType
    {
        PotatoChips,
        Pizza,
        Chicken,
    }
    
    // A Top-Level Class for Food Item Classes.
    public class BaseFood : MonoBehaviour
    {
        public FoodType TypeName { get; protected set; }
    }
}
