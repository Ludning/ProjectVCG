using System;
using System.Collections.Generic;
using UnityEngine;

public class UIContainer : MonoBehaviour
{
    public ChapterSelectPopup ChapterSelectPopup;
    public StageSelectPopup StageSelectPopup;
    public StageClearPopup StageClearPopup;
    public NPCMessagePopup NPCMessagePopup;
    public RecipePopup RecipePopup;
    public GameMenuPopup GameMenuPopup;
    public InventoryPopup InventoryPopup;

    private void OnEnable()
    {
        ChapterSelectPopup.gameObject.SetActive(true);
        StageSelectPopup.gameObject.SetActive(false);
        StageClearPopup.gameObject.SetActive(false);
        NPCMessagePopup.gameObject.SetActive(false);
        RecipePopup.gameObject.SetActive(false);
        GameMenuPopup.gameObject.SetActive(false);
        InventoryPopup.gameObject.SetActive(false);
    }

    public void Init()
    {
        ChapterSelectPopup.gameObject.SetActive(false);
        StageSelectPopup.gameObject.SetActive(false);
        StageClearPopup.gameObject.SetActive(false);
        NPCMessagePopup.gameObject.SetActive(false);
        RecipePopup.gameObject.SetActive(true);
        GameMenuPopup.gameObject.SetActive(true);
        InventoryPopup.gameObject.SetActive(true);
    }
    
    public void Clear()
    {
        ChapterSelectPopup.gameObject.SetActive(true);
        StageSelectPopup.gameObject.SetActive(false);
        StageClearPopup.gameObject.SetActive(false);
        NPCMessagePopup.gameObject.SetActive(false);
        RecipePopup.gameObject.SetActive(false);
        GameMenuPopup.gameObject.SetActive(false);
        InventoryPopup.gameObject.SetActive(false);
    }
}
