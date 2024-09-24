using Frameworks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    Dictionary<Vector2Int, StageData> stageDatas = new Dictionary<Vector2Int, StageData>();

    private bool isInitialized = false;
    
    public int firstChapterIndex { get; set; }
    public int lastChapterIndex { get; set; }
    public int SelectedChapterIndex { get; set; }
    public int SelectedStageIndex { get; set; }

    public readonly float PlayerPositionAdditive = 1.5f;
    public readonly float ItemPositionAdditive = 2f;
    public readonly float NpcPositionAdditive = 1f;
    
    public void Init()
    {
        firstChapterIndex = int.MaxValue;
        lastChapterIndex = int.MinValue;
        
        Debug.Log("Get StageData Start");
        var stageDataDictionary =  DataManager.Instance.GetGameDataDictionary<StageData>();
        Debug.Log("Get StageData End");
        
        foreach (var stageData in stageDataDictionary.Values)
        {
            Vector2Int key = new Vector2Int(stageData.Chapter, stageData.Stage);
            stageDatas.TryAdd(key, stageData);
            
            if (firstChapterIndex > stageData.Chapter)
                firstChapterIndex = stageData.Chapter;
            if (lastChapterIndex < stageData.Chapter)
                lastChapterIndex = stageData.Chapter;
        }

        isInitialized = true;
    }

    public StageData GetCurrentStageData()
    {
        Vector2Int chapterStageVector = new Vector2Int(SelectedChapterIndex, SelectedStageIndex);
        //Debug.Log($"현재 스테이지 : {chapterStageVector}");
        return stageDatas[chapterStageVector];
    }

    public List<int> GetChapterIndexList()
    {
        List<int> chapterIndexList = new List<int>();
        foreach (var stageIndex in stageDatas.Keys)
        {
            if(!chapterIndexList.Contains(stageIndex.x))
                chapterIndexList.Add(stageIndex.x);
        }
        return chapterIndexList.OrderBy(chapterIndex => chapterIndex).ToList();
    }
    public List<int> GetStageIndexList(int selectChapter)
    {
        List<int> stageIndexList = new List<int>();
        foreach (var stageIndex in stageDatas.Keys)
        {
            if(stageIndex.x == selectChapter)
                stageIndexList.Add(stageIndex.y);
        }
        return stageIndexList.OrderBy(stageIndex => stageIndex).ToList();
    }
    public StageData GetStage(List<StageData> stageDatas, int stageIndex)
    {
        return stageDatas.FirstOrDefault(stageData => stageData.Stage == stageIndex);
    }

    public bool OutOfRange_ChapterIndex()
    {
        return SelectedChapterIndex < firstChapterIndex || SelectedChapterIndex > lastChapterIndex;
    }
    public bool OutOfRange_StageIndex(int chapterIndex)
    {
        return SelectedStageIndex < GetFirstStageIndex(chapterIndex) || SelectedStageIndex > GetLastStageIndex(chapterIndex);
    }

    public bool HasNextChapter()
    {
        int nextChapter = SelectedChapterIndex + 1;
        Vector2Int chapterStage = new Vector2Int(nextChapter, GetFirstStageIndex(nextChapter));
        return stageDatas.ContainsKey(chapterStage);
    }
    public bool HasNextStage()
    {
        Vector2Int chapterStage = new Vector2Int(SelectedChapterIndex, SelectedStageIndex + 1);
        return stageDatas.ContainsKey(chapterStage);
    }

    public int GetFirstStageIndex(int chapterIndex)
    {
        int temp = int.MaxValue;
        foreach (var key in stageDatas.Keys)
        {
            if (key.x == chapterIndex)
            {
                if (temp > key.y)
                    temp = key.y;
            }
        }
        return temp;
    }
    public int GetLastStageIndex(int chapterIndex)
    {
        int temp = int.MinValue;
        foreach (var key in stageDatas.Keys)
        {
            if (key.x == chapterIndex)
            {
                if (temp < key.y)
                    temp = key.y;
            }
        }
        return temp;
    }
}
