using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUnitBase // Assets/Scripts/Recipe/RecipeBase.cs
{
    public LevelUnitUIElement LevelUnitUIElement;
    
    // Recipe가 완료되었는지를 판별하는 변수
    private bool _isComplete;
    public bool IsComplete => _isComplete;
    public virtual bool CheakLevel(TileBase tileBase)
    {
        return true;
    }

    public virtual void OnComplete()
    {
        _isComplete = true;
        HideUI();
    }
    public void Reset()
    {
        _isComplete = false;
        ShowUI();
    }
    public void Clear()
    {
        if (LevelUnitUIElement != null)
        {
            LayoutGroup layoutGroup = LevelUnitUIElement.transform.parent.GetComponent<LayoutGroup>();
            Object.Destroy(LevelUnitUIElement.gameObject);
            if (layoutGroup != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup.GetComponent<RectTransform>());
            }
        }
    }
    private void ShowUI()
    {
        LevelUnitUIElement?.gameObject.SetActive(true);
    }
    private void HideUI()
    {
        LevelUnitUIElement?.gameObject.SetActive(false);
    }
}
