using ItemSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipePopup : MonoBehaviour
{
    [SerializeField] private Transform context;
    private Dictionary<string, RecipeUI> recipesDictionary = new Dictionary<string, RecipeUI>();

    //스테이지의 모든 레시피 UI를 등록할 함수
    public void Init()
    {
        
    }
    
    public void DisplayRecipe(string recipeName)
    {
        GameObject recipeUIPrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>(recipeName);
        GameObject recipeObject = Instantiate(recipeUIPrefab, context);
        RecipeUI recipeUI = recipeObject.GetComponent<RecipeUI>();
        recipeUI.Init(recipeName);
        recipesDictionary.Add(recipeName, recipeUI);
    }
    public void HideRecipe(string recipeName)
    {
        if (recipesDictionary.TryGetValue(recipeName, out RecipeUI recipeUI))
        {
            Destroy(recipeUI);
            recipesDictionary.Remove(recipeName);
        }
    }
}
