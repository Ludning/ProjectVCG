using System.Collections.Generic;
using UnityEngine;

public class LevelPopup : MonoBehaviour, IUIBase // Assets/Scripts/UI/RecipePopup.cs
{
    [SerializeField] private Transform context;
    
    // 한 스테이지 내의 모든 레시피를 등록하는 함수
    public void Init()
    {
        
    }
    
    // UI 화면에 레시피를 출력하는 함수
    public void AddLevel(GameObject levelUIElement)
    {
        levelUIElement.transform.SetParent(context);
    }

    public void ClearLevel()
    {
        
    }
    
    // UI 화면에서 레시피를 삭제하는 함수
    /*public void HideRecipe(string recipeName)
    {
        if (recipesDictionary.TryGetValue(recipeName, out LevelUnitUIElement element))
        {
            Destroy(element);
            recipesDictionary.Remove(recipeName);
        }
    }*/
}
