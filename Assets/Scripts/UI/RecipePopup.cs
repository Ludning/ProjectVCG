using System.Collections.Generic;
using UnityEngine;

public class RecipePopup : MonoBehaviour // Assets/Scripts/UI/RecipePopup.cs
{
    [SerializeField] private Transform context;
    private Dictionary<string, RecipePopupElement> recipesDictionary = new();
    
    // 한 스테이지 내의 모든 레시피를 등록하는 함수
    public void Init()
    {
        
    }
    
    // UI 화면에 레시피를 출력하는 함수
    public void DisplayRecipe(string recipeName)
    {
        GameObject recipeUIPrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>(recipeName);
        GameObject recipeObject = Instantiate(recipeUIPrefab, context);
        RecipePopupElement recipePopupElement = recipeObject.GetComponent<RecipePopupElement>();
        recipePopupElement.Init(recipeName);
        recipesDictionary.Add(recipeName, recipePopupElement);
    }
    
    // UI 화면에서 레시피를 삭제하는 함수
    public void HideRecipe(string recipeName)
    {
        if (recipesDictionary.TryGetValue(recipeName, out RecipePopupElement recipeUI))
        {
            Destroy(recipeUI);
            recipesDictionary.Remove(recipeName);
        }
    }
}
