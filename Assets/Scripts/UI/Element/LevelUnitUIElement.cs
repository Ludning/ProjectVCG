using UnityEngine;
using UnityEngine.UIElements;

public class LevelUnitUIElement : MonoBehaviour // Assets/Scripts/UI/Element/RecipePopupElement.cs
{
    [SerializeField] private Image ProductImage;
    [SerializeField] private Transform IngredientLayout;
    
    // Initialize
    public virtual void Init()
    {
    }
}
