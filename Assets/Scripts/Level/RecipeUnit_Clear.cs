using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//레시피 데이터대로 재료를 완성품으로 교체하는 기능단위, 완성품은 완료후 n초후 제거됨
public class RecipeUnit_Clear : RecipeUnit
{
    public int _waitForMiliSecondDelete = 1500;
    
    public RecipeUnit_Clear(LevelData levelData) : base(levelData)
    {
    }
    public override void InitUIElement()
    {
        GameObject prefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>("RecipeUnit_ClearUIElement");
        GameObject uiObject = Object.Instantiate(prefab);
        RecipeUnit_ClearUIElement temp = uiObject.GetComponent<RecipeUnit_ClearUIElement>();
        temp.Init();
        _levelUnitUIElement = temp;
    }
    public override void OnComplete()
    {
        HideProduct().Forget();
        base.OnComplete();
    }

    async UniTaskVoid HideProduct()
    {
        await UniTask.Delay(_waitForMiliSecondDelete);
        if(productFood != null)
            Object.Destroy(productFood);
    }
}
