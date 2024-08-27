using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterSelectUI : MonoBehaviour, IUIElement
{
    [SerializeField] private GameObject NextUI;

    public int _selectedChapterIndex;
    
    public void OnSelectChapter(int chapterIndex)
    {
        _selectedChapterIndex = chapterIndex;
    }
    public void OnClick_OK()
    {
        if (_selectedChapterIndex == 0)
            return;
        NextUI.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Init()
    {
    }
}
