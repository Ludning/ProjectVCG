using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UIContainer : MonoBehaviour
{
    public ChapterSelectPopup ChapterSelectPopup;
    public StageSelectPopup StageSelectPopup;
    public NPCMessagePopup NPCMessagePopup;
    public LevelPopup levelPopup;
    public GameMenuPopup GameMenuPopup;
    public InventoryPopup InventoryPopup;
    public ClearPopup ClearPopup;

    public void InitGame()
    {
        ChapterSelectPopup.gameObject.SetActive(false);
        StageSelectPopup.gameObject.SetActive(false);
        NPCMessagePopup.gameObject.SetActive(false);
        levelPopup.gameObject.SetActive(true);
        GameMenuPopup.gameObject.SetActive(true);
        InventoryPopup.gameObject.SetActive(true);
        ClearPopup.gameObject.SetActive(false);
        
        InventoryPopup.Init();
    }
    
    public void InitStageSelect()
    {
        ChapterSelectPopup.gameObject.SetActive(true);
        StageSelectPopup.gameObject.SetActive(false);
        NPCMessagePopup.gameObject.SetActive(false);
        levelPopup.gameObject.SetActive(false);
        GameMenuPopup.gameObject.SetActive(false);
        InventoryPopup.gameObject.SetActive(false);
        ClearPopup.gameObject.SetActive(false);
    }

    public void Clear()
    {
        ChapterSelectPopup.gameObject.SetActive(false);
        StageSelectPopup.gameObject.SetActive(false);
        NPCMessagePopup.gameObject.SetActive(false);
        levelPopup.gameObject.SetActive(false);
        GameMenuPopup.gameObject.SetActive(false);
        InventoryPopup.gameObject.SetActive(false);
        ClearPopup.gameObject.SetActive(false);
    }
}
