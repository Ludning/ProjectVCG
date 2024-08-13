using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum StageIndex {
    Chapter1_1,
    Chapter1_2,
    Serving1,
    Serving2,
    Serving3,
    Serving4
}


// 내가 버튼을 눌렀어 이게 챕터 버튼인가? 아닌가?

// 챕터 버튼이라면?
    // 어떤 챕터 버튼을 눌럿는가? 챕터 기록
    // 챕터에 맞는 스테이지 노출

// 챕터 버튼이 아니라면?
    // 어떤 스테이지인지 기록
    // 스테이지 오픈 함수
public class Chapter : MonoBehaviour
{



    private string _currentChapter = null;                                             // 현재 챕터
    private string _currentStage = null;                                               // 현재 스테이지
    [SerializeField] private Button[] ChapterStageBtnArr;// 챕터와 스테이지의 모든 버튼을 담음
    [SerializeField] private GameObject ChapterPopup;
    [SerializeField] private GameObject StagePopup1;
    [SerializeField] private GameObject StagePopup2;
    Dictionary<StageIndex, List<StageIndex>> _dic = new Dictionary<StageIndex, List<StageIndex>>();
    StageIndex[] stageIndices = (StageIndex[])Enum.GetValues(typeof(StageIndex));

    void Start()
    {
        _dic.Add(StageIndex.Chapter1_1, new List<StageIndex> { StageIndex.Serving1, StageIndex.Serving2 });
        _dic.Add(StageIndex.Chapter1_2, new List<StageIndex> { StageIndex.Serving3, StageIndex.Serving4 });
        for (int i = 0; i < stageIndices.Length; i++)
        {
            int stageIndex = i;                                                 // i 값을 로컬 변수에 저장 (클로저 문제 해결)
            string btnName = stageIndices[stageIndex].ToString();               

            ChapterStageBtnArr[i].onClick.AddListener(() => SetCurrentChapterStage(btnName));
           

        }
    }
    void SetCurrentChapterStage(string btnName)
    {
        if (btnName.Contains("Chapter")){
            _currentChapter = btnName;
            if(btnName == StageIndex.Chapter1_1.ToString())
            {
                ChapterPopup.SetActive(false);
                StagePopup1.SetActive(true);
                StagePopup2.SetActive(false);
            } else if(btnName == StageIndex.Chapter1_2.ToString())
            {
                ChapterPopup.SetActive(false);
                StagePopup1.SetActive(false);
                StagePopup2.SetActive(true);
            }

        } else
        {
            _currentStage = btnName;
        }
    }

    void ClearStage()
    {
        if (_currentChapter == null || _currentStage == null) return;
        if (Enum.TryParse(_currentChapter, out StageIndex currentChapterIndex))
        {
            // _currentStage 값이 _dic[currentChapterIndex] 리스트에 있는지 확인
            if (_dic.ContainsKey(currentChapterIndex) && _dic[currentChapterIndex].Contains((StageIndex)Enum.Parse(typeof(StageIndex), _currentStage)))
            {
                List<StageIndex> stages = _dic[currentChapterIndex];

                // 현재 스테이지가 마지막 스테이지인지 확인
                if (stages[stages.Count - 1].ToString() == _currentStage)
                {
                    Debug.Log("현재 스테이지는 마지막 스테이지입니다.");
                    // 여기에 마지막 스테이지인 경우에 해야 할 추가 작업을 추가하세요.
                }
                else
                {
                    Debug.Log("현재 스테이지는 마지막 스테이지가 아닙니다.");
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(_currentStage);
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ClearStage();
        }
    }
}
