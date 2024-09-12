using System.Collections.Generic;
using UnityEngine;

public class LevelPopup : MonoBehaviour, IUIBase // Assets/Scripts/UI/RecipePopup.cs
{
    public Transform LevelUIParent;
    
    /*// 한 스테이지 내의 모든 레시피를 등록하는 함수
    public void Init()
    {
        
    }
    
    // UI 화면에 레시피를 출력하는 함수
    public void AddLevel(GameObject levelUIElement)
    {
        levelUIElement.transform.SetParent(context);
        levelUIElement.transform.localScale = Vector3.one;
        levelUIElement.transform.position =
            new Vector3(levelUIElement.transform.position.x, levelUIElement.transform.position.y, 0);
    }

    public void ClearLevel()
    {
        
    }*/
    
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
