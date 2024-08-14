using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// [정리]
// 1. 챕터창 팝업 오픈 함수 OpenChapter() 게임 시작할 때 호출 해야할 듯? 
// 2. TODO 스테이지 선택 함수 SelectStage() 스테이지 선택 이후 초기화 로직을 여기서 작성해야 할 듯
// 3. 스테이지 클리어 했을 때 호출해야하는 함수 ClearStage();

// [기획팀에게 피드백 받아야 하는 내용]
// 1. 메인메뉴 버튼은 어디에 위치하고 눌렀을 때 UI 어떤 버튼 나오는지 메인메뉴 대략적인 UI배치 필요할듯



// 스테이지 또는 챕터를 추가할 때 아래 열거형에 반영해야 합니다.
enum ChapterIndex
{
    Chapter1_1,
    Chapter1_2
}

enum StageIndex
{
    Serving1,
    Serving2,
    Serving3,
    Serving4
}

public class Chapter : MonoBehaviour
{
    // 현재 선택된 챕터와 스테이지 이름을 저장
    private string _currentChapter = null;
    private string _currentStage = null;

    // UI 요소 참조
    [Header("Btn_ChapterAndStage")]
    [SerializeField] private Button[] ChapterBtn;     // 챕터 선택 버튼들
    [SerializeField] private Button[] StageBtn;       // 스테이지 선택 버튼들

    [Header("Popup")]
    [SerializeField] private GameObject ClearPopup;   // 스테이지 클리어 팝업
    [SerializeField] private GameObject ChapterPopup; // 챕터 선택 팝업
    [SerializeField] private GameObject StagePopup1;  // 첫 번째 스테이지 팝업
    [SerializeField] private GameObject StagePopup2;  // 두 번째 스테이지 팝업

    [Header("MoveBtn")]
    [SerializeField] private Button OpenChapterBtn;   // 챕터 선택 팝업 열기 버튼
    [SerializeField] private Button NextChapterBtn;   // 다음 챕터로 이동 버튼

    [Header("CloseBtn")]
    [SerializeField] private Button CloseChapterPopup;// 챕터 팝업 닫기 버튼
    [SerializeField] private Button CloseStage1Popup; // 챕터 1-1 팝업 닫기 버튼
    [SerializeField] private Button CloseStage2Popup; // 챕터 1-2 팝업 닫기 버튼

    [Header("테스트용")] // 테스트 후 제거
    [SerializeField] private Text _currentStageView;  // 테스트 후 제거
    [SerializeField] private Text _currentChapterView;// 테스트 후 제거
    [SerializeField] private Button OnOpenChapter;    // 테스트 후 제거
    [SerializeField] private Button OnGameClear;      // 테스트 후 제거


    // 챕터와 해당 스테이지 목록을 저장할 딕셔너리
    Dictionary<ChapterIndex, List<StageIndex>> _dic = new Dictionary<ChapterIndex, List<StageIndex>>();

    // 열거형 값을 배열로 가져옴
    ChapterIndex[] chapterIndices = (ChapterIndex[])Enum.GetValues(typeof(ChapterIndex));
    StageIndex[] stageIndices = (StageIndex[])Enum.GetValues(typeof(StageIndex));

    private void Awake()
    {
        // 각 챕터에 해당하는 스테이지를 딕셔너리에 추가
        _dic.Add(ChapterIndex.Chapter1_1, new List<StageIndex> { StageIndex.Serving1, StageIndex.Serving2 });
        _dic.Add(ChapterIndex.Chapter1_2, new List<StageIndex> { StageIndex.Serving3, StageIndex.Serving4 });
    }

    void Start()
    {
        for (int i = 0; i < ChapterBtn.Length; i++)             // 챕터 버튼 클릭 이벤트 설정
        {
            int idx = i;
            string btnName = ChapterBtn[idx].name;
            ChapterBtn[idx].onClick.AddListener(() => SelectChapter(btnName));
        }
        for (int i = 0; i < StageBtn.Length; i++)               // 스테이지 버튼 클릭 이벤트 설정
        {
            int idx = i;
            string btnName = StageBtn[idx].name;
            StageBtn[idx].onClick.AddListener(() => SelectStage(btnName));
        }
        OpenChapterBtn.onClick.AddListener(() => OpenChapter());// 챕터 선택 팝업 열기 버튼 클릭 이벤트 설정        
        NextChapterBtn.onClick.AddListener(() => LastStage());  // 다음 챕터로 이동 버튼 클릭 이벤트 설정

        CloseStage1Popup.onClick.AddListener(() => ActivePopup(ChapterPopup));
        CloseStage2Popup.onClick.AddListener(() => ActivePopup(ChapterPopup));


        OnOpenChapter.onClick.AddListener(() => OpenChapter()); // Test 후 삭제
        OnGameClear.onClick.AddListener(() => ClearStage());    // Test 후 삭제
    }

    // 챕터 선택 버튼 클릭 할 때 호출
    void SelectChapter(string btnName)
    {
        _currentChapter = btnName;  // 현재 챕터 이름 저장

        // 선택한 챕터의 스테이지 팝업 표시
        if (Enum.TryParse(_currentChapter, out ChapterIndex selectedChapter))
        {
            switch (selectedChapter)
            {
                case ChapterIndex.Chapter1_1:
                    ActivePopup(StagePopup1);
                    break;
                case ChapterIndex.Chapter1_2:
                    ActivePopup(StagePopup2);
                    break;
            }
        }
    }

    // 스테이지 선택 버튼 클릭 할 때 호출
    void SelectStage(string stageName) 
    {
        _currentStage = stageName;  // 현재 스테이지 이름 저장
        ActivePopup();              // 모든 팝업 닫기
    }

    // 챕터 팝업창 여는 함수
    public void OpenChapter() => ActivePopup(ChapterPopup);

    public void ActivePopup(GameObject obj = null)
    {
        GameObject[] popups = { ClearPopup, ChapterPopup, StagePopup1, StagePopup2 };
        foreach (GameObject popup in popups)
        {
            if(obj == null)
            {
                popup.SetActive(false);
            } else
            {
                popup.SetActive(popup == obj);
            }
        }
    }
    // 현재 스테이지를 클리어 했을 때 호출
    void ClearStage()
    {
        // 현재 선택된 챕터와 스테이지가 없으면 리턴
        if (_currentChapter == null || _currentStage == null) return;

        // 현재 챕터의 스테이지 리스트에서 현재 스테이지가 마지막인지 확인
        if (Enum.TryParse(_currentChapter, out ChapterIndex currentChapterIndex))
        {
            if (_dic.ContainsKey(currentChapterIndex) && _dic[currentChapterIndex].Contains((StageIndex)Enum.Parse(typeof(StageIndex), _currentStage)))
            {
                List<StageIndex> stages = _dic[currentChapterIndex];

                // 마지막 스테이지인지 확인
                if (stages[stages.Count - 1].ToString() == _currentStage)
                {
                    Debug.Log("현재 스테이지는 마지막 스테이지입니다.");
                    ActivePopup(ClearPopup);

                }
                // 마지막 스테이지가 아니면 다음 스테이지로 이동
                else
                {
                    Debug.Log("마지막 스테이지가 아닙니다");
                    NextStage(stages);
                }
            }
        }
    }

    // 다음 스테이지로 이동
    void NextStage(List<StageIndex> stages)
    {
        StageIndex currentStageEnum;
        if (Enum.TryParse(_currentStage, out currentStageEnum))
        {
            int currentIndex = stages.IndexOf(currentStageEnum);

            if (currentIndex != -1 && currentIndex < stages.Count - 1)
            {
                // 다음 스테이지로 업데이트
                currentIndex++;
                _currentStage = stages[currentIndex].ToString();
                Debug.Log("Moved to the next stage: " + _currentStage);

                // 추가적으로 씬 로딩이나 UI 업데이트 등의 논리를 여기에 추가할 수 있음
            }
            else
            {
                Debug.Log("Already at the last stage or current stage not found in the list.");
            }
        }
        else
        {
            Debug.LogError("Failed to parse _currentStage to StageIndex enum.");
        }
    }

    // 마지막 스테이지를 클리어 한 후 다음 챕터로 이동
    void LastStage()
    {
        if (Enum.TryParse(_currentChapter, out ChapterIndex currentChapterEnum))
        {
            int currentChapterIndex = Array.IndexOf(chapterIndices, currentChapterEnum);

            // 다음 챕터가 존재하는지 확인
            if (currentChapterIndex != -1 && currentChapterIndex < chapterIndices.Length - 1)
            {
                ChapterIndex nextChapter = chapterIndices[currentChapterIndex + 1];

                // 다음 챕터의 첫 번째 스테이지로 이동
                if (_dic.ContainsKey(nextChapter))
                {
                    _currentChapter = nextChapter.ToString();
                    _currentStage = _dic[nextChapter][0].ToString();  // 첫 번째 스테이지로 설정

                    Debug.Log("Moved to the next chapter: " + _currentChapter + ", first stage: " + _currentStage);
                    ClearPopup.SetActive(false);
                }
                else
                {
                    Debug.Log("The next chapter does not have any stages defined.");
                }
            }
            else
            {
                Debug.Log("현재 챕터는 마지막 챕터입니다. 더 이상 진행할 챕터가 없습니다.");
            }
        }
        else
        {
            Debug.LogError("Failed to parse _currentChapter to ChapterIndex enum.");
        }
    }

    void Update()
    {
        _currentChapterView.text = $"현재 챕터 : {_currentChapter}";
        _currentStageView.text = $"현재 스테이지 : {_currentStage}";

    }
}
