using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UIContainer : MonoBehaviour
{
    public ChapterSelectPopup ChapterSelectPopup;
    public StageSelectPopup StageSelectPopup;
    public StageClearPopup StageClearPopup;
    public NPCMessagePopup NPCMessagePopup;
    [FormerlySerializedAs("RecipePopup")] public LevelPopup levelPopup;
    public GameMenuPopup GameMenuPopup;
    public InventoryPopup InventoryPopup;

    private void OnEnable()
    {
        ChapterSelectPopup.gameObject.SetActive(true);
        StageSelectPopup.gameObject.SetActive(false);
        StageClearPopup.gameObject.SetActive(false);
        NPCMessagePopup.gameObject.SetActive(false);
        levelPopup.gameObject.SetActive(false);
        GameMenuPopup.gameObject.SetActive(false);
        InventoryPopup.gameObject.SetActive(false);
    }

    public void Init()
    {
        ChapterSelectPopup.gameObject.SetActive(false);
        StageSelectPopup.gameObject.SetActive(false);
        StageClearPopup.gameObject.SetActive(false);
        NPCMessagePopup.gameObject.SetActive(false);
        levelPopup.gameObject.SetActive(true);
        GameMenuPopup.gameObject.SetActive(true);
        InventoryPopup.gameObject.SetActive(true);
    }
    
    public void Clear()
    {
        ChapterSelectPopup.gameObject.SetActive(true);
        StageSelectPopup.gameObject.SetActive(false);
        StageClearPopup.gameObject.SetActive(false);
        NPCMessagePopup.gameObject.SetActive(false);
        levelPopup.gameObject.SetActive(false);
        GameMenuPopup.gameObject.SetActive(false);
        InventoryPopup.gameObject.SetActive(false);
    }
}
