using System.Collections.Generic;
using UnityEngine;

public class LevelUnitBase // Assets/Scripts/Recipe/RecipeBase.cs
{
    protected readonly LevelData _levelData;

    protected LevelUnitUIElement _levelUnitUIElement;
    
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
    protected LevelUnitBase(LevelData levelData)
    {
        _levelData = levelData;
    }
    public virtual void InitUIElement() { }
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
        if(_levelUnitUIElement != null)
            Object.Destroy(_levelUnitUIElement.gameObject);
    }
    private void ShowUI()
    {
        _levelUnitUIElement?.gameObject.SetActive(true);
    }
    private void HideUI()
    {
        _levelUnitUIElement?.gameObject.SetActive(false);
    }
}
