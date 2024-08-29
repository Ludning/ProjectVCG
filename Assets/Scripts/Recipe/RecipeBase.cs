using System.Collections.Generic;

public class RecipeBase // Assets/Scripts/Recipe/RecipeBase.cs
{
    public string RecipeName { get; private set; } // Recipe의 이름
    public bool IsComplete { get; set; } // Recipe가 완료되었는지를 판별하는 변수

    public List<string> Ingredients = new List<string>();

    // [생성자] RecipeBase의 목표 음식 또는 재료를 초기화
    public RecipeBase(string recipeName)
    {
        RecipeName = recipeName;
        IsComplete = false;
    }

    public void Reset()
    {
        IsComplete = false;
    }
}
