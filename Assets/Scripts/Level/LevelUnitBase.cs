using System.Collections.Generic;
using UnityEngine;

public class LevelUnitBase // Assets/Scripts/Recipe/RecipeBase.cs
{
    public LevelUnitUIElement LevelUnitUIElement;
    
    // Recipe가 완료되었는지를 판별하는 변수
    private bool _isComplete;
    public bool IsComplete
    {
        get => _isComplete;
        set
        {
            _isComplete = value;
            if (_isComplete == true)
                OnComplete();
        }
    }
    public virtual bool CheakLevel(TileBase tileBase)
    {
        return true;
    }

    public virtual void OnComplete()
    {
        HideUI();
    }
    public void Reset()
    {
        IsComplete = false;
        ShowUI();
    }
    public void Clear()
    {
        if(LevelUnitUIElement != null)
            Object.Destroy(LevelUnitUIElement.gameObject);
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
