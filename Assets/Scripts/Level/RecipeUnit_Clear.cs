using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//레시피 데이터대로 재료를 완성품으로 교체하는 기능단위, 완성품은 완료후 n초후 제거됨
public class RecipeUnit_Clear : RecipeUnit
{
    public int _waitForMiliSecondDelete = 1500;
    
    public RecipeUnit_Clear(string recipeIndex, string tileKey, Transform uiParent) : base(recipeIndex, tileKey, uiParent)
    {
    }
    protected override void InitUIElement(Transform uiParent)
    {
        GameObject prefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>("RecipeUnit_ClearUIElement");
        GameObject uiObject = Object.Instantiate(prefab, uiParent);
        RecipeUnit_ClearUIElement temp = uiObject.GetComponent<RecipeUnit_ClearUIElement>();
        temp.Init(_recipeIndex);
        LevelUnitUIElement = temp;
    }
    public override void OnComplete()
    {
        ClearProductWaitForSecond().Forget();
        base.OnComplete();
    }

    async UniTaskVoid ClearProductWaitForSecond()
    {
        Debug.Log("HideProduct");
        GameObject tempProductFood = productFood;
        productFood = null;
        await UniTask.Delay(_waitForMiliSecondDelete);
        if(tempProductFood != null)
            Object.Destroy(tempProductFood);
    }
}
