using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearPopup : MonoBehaviour
{
    [SerializeField] private StageManager StageManager;
    public void OnClick_SelectChapter()
    {
        StageManager.InitMain();
    }
    public void OnClick_NextChapter()
    {
        StageManager.ClearStage();
        GameManager.Instance.SelectedChapterIndex++;
        GameManager.Instance.SelectedStageIndex = GameManager.Instance.GetFirstStageIndex(GameManager.Instance.SelectedChapterIndex);
        StageManager.InitStage();
    }
}
