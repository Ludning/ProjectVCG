using UnityEngine;

namespace ItemSystem
{
    public enum ItemType
    {
        PotatoChips,
        Chicken,
        Pizza,
    }
    
    public class BaseItem : MonoBehaviour
    {
        [SerializeField] private ItemType typeName;
    }
}
