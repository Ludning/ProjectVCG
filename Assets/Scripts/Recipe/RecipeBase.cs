using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeBase
{
    public string recipeName;
    public bool isComplete;

    //생성자에서 RecipeBase의 목표 음식 또는 재료를 초기화
    public RecipeBase(string recipeName)
    {
        this.recipeName = recipeName;
        isComplete = false;
    }

    public void Reset()
    {
        isComplete = false;
    }
}
