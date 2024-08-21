using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// [정리]
// 1. 챕터창 팝업을 여는 함수 OpenChapter()는 게임 시작할 때 호출하는 것이 좋을 듯.
// 2. TODO: 스테이지 선택 함수 SelectStage()는 스테이지 선택 후 초기화 로직을 작성해야 함.
// 3. 스테이지를 클리어했을 때 호출하는 함수 ClearStage().


public class Chapter : MonoBehaviour
{
    // 현재 선택된 챕터와 스테이지 이름을 저장
    private string _currentChapter = null;
    private string _currentStage = null;
    private bool _isChapterSelectionActive;

    // UI 요소 참조
    [Header("Btn_ChapterAndStage")]
    [SerializeField] private Button[] ChapterBtn;     // 챕터 선택 버튼들
    [SerializeField] private Button[] StageBtn;       // 스테이지 선택 버튼들

    [Header("Popup")]
    [SerializeField] private GameObject ClearPopup;   // 스테이지 클리어 팝업
    [SerializeField] private GameObject ExitPopup;    // 돌아가기 팝업
    [SerializeField] private GameObject ChapterPopup; // 챕터 선택 팝업
    [SerializeField] private GameObject StagePopup1;  // 첫 번째 챕터의 스테이지 팝업
    [SerializeField] private GameObject StagePopup2;  // 두 번째 챕터의 스테이지 팝업
    [SerializeField] private GameObject StagePopup3;  // 두 번째 챕터의 스테이지 팝업

    [Header("MoveBtn")]
    [SerializeField] private Button OpenChapterBtn;   // 챕터 선택 팝업 열도록 하는 버튼 (마지막 스테이지 클리어 시 나오는 팝업의 일부 버튼)
    [SerializeField] private Button NextChapterBtn;   // 다음 챕터로 이동 버튼

    [Header("CloseBtn")]
    [SerializeField] private Button CloseChapterPopup;// 챕터 팝업 닫기 버튼
    [SerializeField] private Button CloseStage1Popup; // 첫 번째 챕터의 팝업 닫기 버튼
    [SerializeField] private Button CloseStage2Popup; // 두 번째 챕터의 팝업 닫기 버튼
    [SerializeField] private Button CloseStage3Popup; // 두 번째 챕터의 팝업 닫기 버튼

    [Header("OtherBtns")]
    [SerializeField] private Button chapterSelectButton;    // 게임 시작 시점에서만 보이는 챕터 선택 버튼
    [SerializeField] private Button BeforeBtn;              // 돌아가기 버튼
    [SerializeField] private Button BeforeYesBtn;           // 돌아가기 확인 팝업의 Yes 버튼
    [SerializeField] private Button BeforeNoBtn;            // 돌아가기 확인 팝업의 No 버튼
    [SerializeField] private Button[] PopupChangeButton;    // 이전,다음 스테이지로 변경 팝업창

    [Header("OtherScript")]
    [SerializeField] private StageManager stageManager;

    [Header("테스트용")] // 테스트 후 제거 예정
    [SerializeField] private Text _currentStageView;  // 현재 스테이지 표시용 텍스트 (테스트용)
    [SerializeField] private Text _currentChapterView;// 현재 챕터 표시용 텍스트 (테스트용)
    [SerializeField] private Button OnGameClear;      // 스테이지 클리어 테스트용 버튼 (테스트 후 삭제)

    // 챕터와 해당 스테이지 목록을 저장할 딕셔너리
    Dictionary<ChapterIndex, List<StageIndex>> _dic = new Dictionary<ChapterIndex, List<StageIndex>>();

    // 열거형을 배열로 가져옴
    ChapterIndex[] chapterIndices = (ChapterIndex[])Enum.GetValues(typeof(ChapterIndex));

    private void Awake()
    {
        // 게임 시작 시 챕터 선택 버튼이 비활성화 상태라면 활성화
        if (!chapterSelectButton.gameObject.activeSelf) chapterSelectButton.gameObject.SetActive(true);

        // 각 챕터에 해당하는 스테이지 목록을 딕셔너리에 추가
        _dic.Add(ChapterIndex.Chapter1, new List<StageIndex> { StageIndex.Serving1, StageIndex.Serving2, StageIndex.Serving3, StageIndex.Serving4 });
        _dic.Add(ChapterIndex.Chapter2, new List<StageIndex> { StageIndex.Serving5, StageIndex.Serving6, StageIndex.Serving7});
        _dic.Add(ChapterIndex.Chapter3, new List<StageIndex> { StageIndex.Serving8});
    }

    void Start()
    {
        #region Event
        // 챕터 선택 버튼 클릭 이벤트 설정
        for (int i = 0; i < ChapterBtn.Length; i++)
        {
            int idx = i;
            string btnName = ChapterBtn[idx].name;
            ChapterBtn[idx].onClick.AddListener(() => SelectChapter(btnName));
        }

        // 스테이지 선택 버튼 클릭 이벤트 설정
        for (int i = 0; i < StageBtn.Length; i++)
        {
            int idx = i;
            string btnName = StageBtn[idx].name;
            StageBtn[idx].onClick.AddListener(() => SelectStage(btnName));
        }

    /*    for (int i = 0; i < PopupChangeButton.Length; i++)
        {
            int idx = i;

            PopupChangeButton[idx].onClick.AddListener(() =>
            {
                // 버튼을 클릭할 때마다 _currentStage 또는 다른 전역 변수의 현재 값을 전달
                string currentStage = _currentStage; // 예시로 전역 변수 _currentStage를 사용
                SelectStage(currentStage);
            });
        }
*/
        // 챕터 선택 팝업 열기 버튼 클릭 이벤트 설정
        OpenChapterBtn.onClick.AddListener(() => OpenChapter());

        // 다음 챕터로 이동 버튼 클릭 이벤트 설정
        NextChapterBtn.onClick.AddListener(() => LastStage());

        // 챕터 1 팝업 닫기 버튼 클릭 시 챕터 팝업 열기
        CloseStage1Popup.onClick.AddListener(() => ActivePopup(ChapterPopup));
        CloseStage2Popup.onClick.AddListener(() => ActivePopup(ChapterPopup));
        CloseStage3Popup.onClick.AddListener(() => ActivePopup(ChapterPopup));

        // 돌아가기 버튼 클릭 시 돌아가기 팝업 표시
        BeforeBtn.onClick.AddListener(() => ExitPopup.SetActive(true));

        // 돌아가기 확인 팝업의 No 버튼 클릭 시 팝업 닫기
        BeforeNoBtn.onClick.AddListener(() => ExitPopup.SetActive(false));

        // 돌아가기 확인 팝업의 Yes 버튼 클릭 시 로직 처리
        BeforeYesBtn.onClick.AddListener(() =>
        {
            BeforeBtn.gameObject.SetActive(false);
            _isChapterSelectionActive = false;
            ActivePopup(ChapterPopup);  // 챕터 선택 팝업 열기
        });

        // 챕터 선택 팝업 닫기 버튼 클릭 이벤트 설정
        CloseChapterPopup.onClick.AddListener(() =>
        {
            chapterSelectButton.gameObject.SetActive(true);
            ActivePopup();  // 모든 팝업 닫기
        });

        // 게임 시작 시 챕터 선택 버튼 클릭 시 챕터 선택창 열기
        chapterSelectButton.onClick.AddListener(() =>
        {
            OpenChapter();
            chapterSelectButton.gameObject.SetActive(false);
        });

        // 테스트 용으로 게임 클리어 처리
        OnGameClear.onClick.AddListener(() => ClearStage());    // 테스트 후 삭제
        #endregion
    }

    // 챕터 선택 버튼 클릭 시 호출
    void SelectChapter(string btnName)
    {
        _currentChapter = btnName;  // 현재 선택된 챕터 이름 저장

        // 선택한 챕터에 맞는 스테이지 팝업 표시
        if (Enum.TryParse(_currentChapter, out ChapterIndex selectedChapter))
        {
            switch (selectedChapter)
            {
                case ChapterIndex.Chapter1:
                    ActivePopup(StagePopup1); // 첫 번째 챕터 팝업 열기
                    break;
                case ChapterIndex.Chapter2:
                    ActivePopup(StagePopup2); // 두 번째 챕터 팝업 열기
                    break;
                case ChapterIndex.Chapter3:
                    ActivePopup(StagePopup3); // 두 번째 챕터 팝업 열기
                    break;
            }
        }
    }

    // 스테이지 선택 버튼 클릭 시 호출
    void SelectStage(string stageName)
    {
        // 챕터 선택 중이 아닌 상태라면 챕터 선택 버튼 비활성화 및 돌아가기 버튼 활성화
        LoadMap(stageName); 
        BeforeBtn.gameObject.SetActive(true);

        _currentStage = stageName;  // 현재 선택된 스테이지 이름 저장
        ActivePopup();              // 모든 팝업 닫기
    }
    public void LoadMap(string name)
    {
        StageIndex StageNumber;
        if (name == StageIndex.Serving1.ToString()) StageNumber = StageIndex.Serving1;
        else if (name == StageIndex.Serving2.ToString()) StageNumber = StageIndex.Serving2;
        else if (name == StageIndex.Serving3.ToString()) StageNumber = StageIndex.Serving3;
        else if (name == StageIndex.Serving4.ToString()) StageNumber = StageIndex.Serving4;
        else if (name == StageIndex.Serving5.ToString()) StageNumber = StageIndex.Serving5;
        else if (name == StageIndex.Serving6.ToString()) StageNumber = StageIndex.Serving6;
        else if (name == StageIndex.Serving7.ToString()) StageNumber = StageIndex.Serving7;
        else if (name == StageIndex.Serving8.ToString()) StageNumber = StageIndex.Serving8;
        else StageNumber = StageIndex.None;

        stageManager.InitStage(StageNumber);
    }
    // 챕터 팝업창 여는 함수
    public void OpenChapter() {
        BeforeBtn.gameObject.SetActive(false);
        ActivePopup(ChapterPopup);
    }

    // 팝업 활성화 및 비활성화 처리 함수
    public void ActivePopup(GameObject obj = null)
    {
        if (obj == ChapterPopup) LoadMap("CloseMap");
        else if (obj == ClearPopup) BeforeBtn.gameObject.SetActive(false);

        // 모든 팝업 목록
        GameObject[] popups = { ClearPopup, ChapterPopup, StagePopup1, StagePopup2, StagePopup3, ExitPopup };

        // 모든 팝업을 비활성화하고, 인자로 받은 팝업만 활성화
        foreach (GameObject popup in popups)
        {
            if (obj == null)
            {
                popup.SetActive(false);  // 인자가 없으면 모든 팝업 비활성화
            }
            else
            {
                popup.SetActive(popup == obj);  // 특정 팝업만 활성화
            }
        }

    }

    // 스테이지 클리어 시 호출
    void ClearStage()
    {
        // 현재 선택된 챕터와 스테이지가 없으면 리턴
        if (_currentChapter == null || _currentStage == null) return;

        // 현재 챕터의 마지막 스테이지인지 확인
        if (Enum.TryParse(_currentChapter, out ChapterIndex currentChapterIndex))
        {
            if (_dic.ContainsKey(currentChapterIndex) && _dic[currentChapterIndex].Contains((StageIndex)Enum.Parse(typeof(StageIndex), _currentStage)))
            {
                List<StageIndex> stages = _dic[currentChapterIndex];

                // 마지막 스테이지일 경우 클리어 팝업 표시
                if (stages[stages.Count - 1].ToString() == _currentStage)
                {
                    Debug.Log("현재 스테이지는 마지막 스테이지입니다.");
                    ActivePopup(ClearPopup);
                }
                // 마지막 스테이지가 아니면 다음 스테이지로 이동
                else
                {
                    Debug.Log("마지막 스테이지가 아닙니다.");
                    NextStage(stages);
                }
            }
        }
    }

    // 다음 스테이지로 이동
    void NextStage(List<StageIndex> stages)
    {
        // 현재 스테이지를 열거형으로 변환
        StageIndex currentStageEnum;
        if (Enum.TryParse(_currentStage, out currentStageEnum))
        {
            int currentIndex = stages.IndexOf(currentStageEnum);

            // 다음 스테이지로 이동
            if (currentIndex != -1 && currentIndex < stages.Count - 1)
            {
                currentIndex++;
                _currentStage = stages[currentIndex].ToString(); // 다음 스테이지로 업데이트
                LoadMap(_currentStage);
                Debug.Log("Moved to the next stage: " + _currentStage);

                // 필요한 경우 추가 로직 (씬 로딩 등) 추가 가능
            }
            else
            {
                Debug.Log("이미 마지막 스테이지이거나 스테이지 목록에서 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.LogError("Failed to parse _currentStage to StageIndex enum.");
        }
    }

    // 마지막 스테이지 클리어 후 다음 챕터로 이동
    void LastStage()
    {       
        if (Enum.TryParse(_currentChapter, out ChapterIndex currentChapterEnum))
        {
            int currentChapterIndex = Array.IndexOf(chapterIndices, currentChapterEnum);

            // 다음 챕터로 이동 가능한지 확인
            if (currentChapterIndex != -1 && currentChapterIndex < chapterIndices.Length - 1)
            {
                ChapterIndex nextChapter = chapterIndices[currentChapterIndex + 1];

                // 다음 챕터의 첫 번째 스테이지로 이동
                if (_dic.ContainsKey(nextChapter))
                {
                    _currentChapter = nextChapter.ToString();
                    _currentStage = _dic[nextChapter][0].ToString();  // 첫 번째 스테이지로 설정
                    LoadMap(_currentStage);
                    BeforeBtn.gameObject.SetActive(true);
                    Debug.Log("다음 챕터로 이동: " + _currentChapter + ", 첫 번째 스테이지: " + _currentStage);
                    ClearPopup.SetActive(false); // 클리어 팝업 닫기
                }
                else
                {
                    Debug.Log("다음 챕터에 스테이지가 정의되지 않았습니다.");
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

    // UI 업데이트: 현재 선택된 챕터와 스테이지 표시
    void Update()
    {
        _currentChapterView.text = $"현재 챕터 : {_currentChapter}";
        _currentStageView.text = $"현재 스테이지 : {_currentStage}";
    }
}
