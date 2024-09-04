using Frameworks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    private int selectedChapterIndex;

    public int SelectedChapterIndex
    {
        get
        {
            return selectedChapterIndex;
        }
        set
        {
            Debug.Log("selectedChapterIndex Change");
            selectedChapterIndex = value;
        }
    }
    public int SelectedStageIndex;
    public List<StageData> GetChapterStageList(int chapterIndex)
    {
        var stageDataDict = DataManager.Instance.GetGameDataDictionary<StageData>();
        return stageDataDict.Values
            .Where(stageData => stageData.Chapter == chapterIndex)
            .OrderBy(stageData => stageData.Stage)
            .ToList();
    }
    public StageData GetStage(List<StageData> stageDatas, int stageIndex)
    {
        return stageDatas.FirstOrDefault(stageData => stageData.Stage == stageIndex);
    }
}
