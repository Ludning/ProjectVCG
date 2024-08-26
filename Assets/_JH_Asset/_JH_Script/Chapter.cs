using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chapter : MonoBehaviour
{
    // ���� ���õ� é�Ϳ� �������� ���� ����
    private string _currentChapter = null;
    private string _currentStage = null;
    private bool _isChapterSelectionActive;

    // UI ���
    [Header("Btn_ChapterAndStage")]
    [SerializeField] private Button[] ChapterBtn;     // é�� ���� ��ư��
    [SerializeField] private Button[] StageBtn;       // �������� ���� ��ư��

    [Header("Popup")]
    [SerializeField] private GameObject ClearPopup;   // �������� Ŭ���� �˾�
    [SerializeField] private GameObject ExitPopup;    // ���ư��� �˾�
    [SerializeField] private GameObject ChapterPopup; // é�� ���� �˾�
    [SerializeField] private GameObject StagePopup1;  // ù ��° é���� �������� �˾�
    [SerializeField] private GameObject StagePopup2;  // �� ��° é���� �������� �˾�
    [SerializeField] private GameObject StagePopup3;  // �� ��° é���� �������� �˾�

    [Header("MoveBtn")]
    [SerializeField] private Button OpenChapterBtn;   // é�� ���� �˾� ������ �ϴ� ��ư (������ �������� Ŭ���� �� ������ �˾��� �Ϻ� ��ư)
    [SerializeField] private Button NextChapterBtn;   // ���� é�ͷ� �̵� ��ư

    [Header("CloseBtn")]
    [SerializeField] private Button CloseChapterPopup;// é�� �˾� �ݱ� ��ư
    [SerializeField] private Button CloseStage1Popup; // ù ��° é���� �˾� �ݱ� ��ư
    [SerializeField] private Button CloseStage2Popup; // �� ��° é���� �˾� �ݱ� ��ư
    [SerializeField] private Button CloseStage3Popup; // �� ��° é���� �˾� �ݱ� ��ư

    [Header("OtherBtns")]
    [SerializeField] private Button chapterSelectButton;    // é�� ���� ��ư (���� ���� �� ǥ��)
    [SerializeField] private Button BeforeBtn;              // ���ư��� ��ư
    [SerializeField] private Button BeforeYesBtn;           // ���ư��� Ȯ�� �˾��� Yes ��ư
    [SerializeField] private Button BeforeNoBtn;            // ���ư��� Ȯ�� �˾��� No ��ư
    [SerializeField] private Button[] PopupChangeButton;    // ����/���� ���������� ���� �˾�â

    [Header("OtherScript")]
    [SerializeField] private StageManager stageManager;

    [Header("�׽�Ʈ��")] // �׽�Ʈ �� ���� ����
    [SerializeField] private Text _currentStageView;        // ���� �������� ǥ�ÿ� �ؽ�Ʈ (�׽�Ʈ��)
    [SerializeField] private Text _currentChapterView;      // ���� é�� ǥ�ÿ� �ؽ�Ʈ (�׽�Ʈ��)
    [SerializeField] private Button OnGameClear;            // �������� Ŭ���� �׽�Ʈ�� ��ư (�׽�Ʈ �� ����)

    // é�Ϳ� �������� ������ ����
    Dictionary<ChapterIndex, List<StageIndex>> _dic = new Dictionary<ChapterIndex, List<StageIndex>>();
    ChapterIndex[] chapterIndices = (ChapterIndex[])Enum.GetValues(typeof(ChapterIndex));

    private void Awake()
    {
        // �ʱ� ����: é�� ���� ��ư Ȱ��ȭ �� ��ųʸ� ������ ����
        if (!chapterSelectButton.gameObject.activeSelf) chapterSelectButton.gameObject.SetActive(true);

        _dic.Add(ChapterIndex.Chapter1, new List<StageIndex> { StageIndex.Serving1, StageIndex.Serving2, StageIndex.Serving3, StageIndex.Serving4 });
        _dic.Add(ChapterIndex.Chapter2, new List<StageIndex> { StageIndex.Serving5, StageIndex.Serving6, StageIndex.Serving7 });
        _dic.Add(ChapterIndex.Chapter3, new List<StageIndex> { StageIndex.Serving8 });
    }

    void Start()
    {
        #region Event
        // é�� ���� ��ư Ŭ�� �̺�Ʈ ����
        for (int i = 0; i < ChapterBtn.Length; i++)
        {
            int idx = i;
            string btnName = ChapterBtn[idx].name;
            ChapterBtn[idx].onClick.AddListener(() => SelectChapter(btnName));
        }

        // �������� ���� ��ư Ŭ�� �̺�Ʈ ����
        for (int i = 0; i < StageBtn.Length; i++)
        {
            int idx = i;
            string btnName = StageBtn[idx].name;
            StageBtn[idx].onClick.AddListener(() => SelectStage(btnName));
        }

         for (int i = 0; i < PopupChangeButton.Length; i++)
        {
            int idx = i;

            PopupChangeButton[idx].onClick.AddListener(() =>
            {

                if(PopupChangeButton[idx].name == "BtnPrev")
                {                
                    if (StagePopup2.activeSelf)
                    {
                        ActivePopup(StagePopup1);
                    } else if (StagePopup3.activeSelf)
                    {
                        ActivePopup(StagePopup2);
                    }
                } else
                {
                    if (StagePopup1.activeSelf)
                    {
                        ActivePopup(StagePopup2);
                    }
                    else if (StagePopup2.activeSelf)
                    {
                        ActivePopup(StagePopup3);
                    }
                }

            });
        }
        // é�� ���� �˾� ���� ��ư Ŭ�� �̺�Ʈ ����
        OpenChapterBtn.onClick.AddListener(() => OpenChapter());

        // ���� é�ͷ� �̵� ��ư Ŭ�� �̺�Ʈ ����
        NextChapterBtn.onClick.AddListener(() => LastStage());

        // é�� 1 �˾� �ݱ� ��ư Ŭ�� �� é�� �˾� ����
        CloseStage1Popup.onClick.AddListener(() => ActivePopup(ChapterPopup));
        CloseStage2Popup.onClick.AddListener(() => ActivePopup(ChapterPopup));
        CloseStage3Popup.onClick.AddListener(() => ActivePopup(ChapterPopup));

        // ���ư��� ��ư Ŭ�� �� ���ư��� �˾� ǥ��
        BeforeBtn.onClick.AddListener(() => ExitPopup.SetActive(true));

        // ���ư��� Ȯ�� �˾��� No ��ư Ŭ�� �� �˾� �ݱ�
        BeforeNoBtn.onClick.AddListener(() => ExitPopup.SetActive(false));

        // ���ư��� Ȯ�� �˾��� Yes ��ư Ŭ�� �� ���� ó��
        BeforeYesBtn.onClick.AddListener(() =>
        {
            BeforeBtn.gameObject.SetActive(false);
            _isChapterSelectionActive = false;
            ActivePopup(ChapterPopup);  // é�� ���� �˾� ����
        });

        // é�� ���� �˾� �ݱ� ��ư Ŭ�� �̺�Ʈ ����
        CloseChapterPopup.onClick.AddListener(() =>
        {
            chapterSelectButton.gameObject.SetActive(true);
            ActivePopup();  // ��� �˾� �ݱ�
        });

        // ���� ���� �� é�� ���� ��ư Ŭ�� �� é�� ����â ����
        chapterSelectButton.onClick.AddListener(() =>
        {
            OpenChapter();
            chapterSelectButton.gameObject.SetActive(false);
        });

        // �׽�Ʈ ������ ���� Ŭ���� ó��
        OnGameClear.onClick.AddListener(() => ClearStage());    // �׽�Ʈ �� ����
        #endregion
    }

    // é�� ���� ��ư Ŭ�� �� ȣ��
    void SelectChapter(string btnName)
    {
        /*_currentChapter = btnName;  // ���� ���õ� é�� �̸� ����*/

        // ������ é�Ϳ� �´� �������� �˾� ǥ��
        if (Enum.TryParse(btnName, out ChapterIndex selectedChapter))
        {
            switch (selectedChapter)
            {
                case ChapterIndex.Chapter1:
                    ActivePopup(StagePopup1); // ù ��° é�� �˾� ����
                    break;
                case ChapterIndex.Chapter2:
                    ActivePopup(StagePopup2); // �� ��° é�� �˾� ����
                    break;
                case ChapterIndex.Chapter3:
                    ActivePopup(StagePopup3); // �� ��° é�� �˾� ����
                    break;
            }
        }
    }

    // �������� ���� ��ư Ŭ�� �� ȣ��
    void SelectStage(string stageName)
    {
        foreach (KeyValuePair<ChapterIndex, List<StageIndex>> pair in _dic)
        {
            // �� �������� ����Ʈ ������ stageName�� ��ġ�ϴ� ���������� �ִ��� Ȯ��
            foreach (StageIndex stage in pair.Value)
            {
                if (stage.ToString() == stageName)
                {
                    _currentChapter = pair.Key.ToString();  // �ش� é�͸� ���� é�ͷ� ����
                }
            }
        }
        // é�� ���� ���� �ƴ� ���¶�� é�� ���� ��ư ��Ȱ��ȭ �� ���ư��� ��ư Ȱ��ȭ
        LoadMap(stageName);
        BeforeBtn.gameObject.SetActive(true);

        _currentStage = stageName;  // ���� ���õ� �������� �̸� ����
        ActivePopup();              // ��� �˾� �ݱ�
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

        //stageManager.InitStage(StageNumber);
    }
    // é�� �˾�â ���� �Լ�
    public void OpenChapter()
    {
        BeforeBtn.gameObject.SetActive(false);
        ActivePopup(ChapterPopup);
    }

    // �˾� Ȱ��ȭ �� ��Ȱ��ȭ ó�� �Լ�
    public void ActivePopup(GameObject obj = null)
    {
        if (obj == ChapterPopup) LoadMap("CloseMap");
        else if (obj == ClearPopup) BeforeBtn.gameObject.SetActive(false);

        // ��� �˾� ���
        GameObject[] popups = { ClearPopup, ChapterPopup, StagePopup1, StagePopup2, StagePopup3, ExitPopup };

        // ��� �˾��� ��Ȱ��ȭ�ϰ�, ���ڷ� ���� �˾��� Ȱ��ȭ
        foreach (GameObject popup in popups)
        {
            if (obj == null)
            {
                popup.SetActive(false);  // ���ڰ� ������ ��� �˾� ��Ȱ��ȭ
            }
            else
            {
                popup.SetActive(popup == obj);  // Ư�� �˾��� Ȱ��ȭ
            }
        }

    }

    // �������� Ŭ���� �� ȣ��
    void ClearStage()
    {
        // ���� ���õ� é�Ϳ� ���������� ������ ����
        if (_currentChapter == null || _currentStage == null) return;

        // ���� é���� ������ ������������ Ȯ��
        if (Enum.TryParse(_currentChapter, out ChapterIndex currentChapterIndex))
        {
            if (_dic.ContainsKey(currentChapterIndex) && _dic[currentChapterIndex].Contains((StageIndex)Enum.Parse(typeof(StageIndex), _currentStage)))
            {
                List<StageIndex> stages = _dic[currentChapterIndex];

                // ������ ���������� ��� Ŭ���� �˾� ǥ��
                if (stages[stages.Count - 1].ToString() == _currentStage)
                {
                    Debug.Log("���� ���������� ������ ���������Դϴ�.");
                    ActivePopup(ClearPopup);
                }
                // ������ ���������� �ƴϸ� ���� ���������� �̵�
                else
                {
                    Debug.Log("������ ���������� �ƴմϴ�.");
                    NextStage(stages);
                }
            }
        }
    }

    // ���� ���������� �̵�
    void NextStage(List<StageIndex> stages)
    {
        // ���� ���������� ���������� ��ȯ
        StageIndex currentStageEnum;
        if (Enum.TryParse(_currentStage, out currentStageEnum))
        {
            int currentIndex = stages.IndexOf(currentStageEnum);

            // ���� ���������� �̵�
            if (currentIndex != -1 && currentIndex < stages.Count - 1)
            {
                currentIndex++;
                _currentStage = stages[currentIndex].ToString(); // ���� ���������� ������Ʈ
                LoadMap(_currentStage);
                Debug.Log("Moved to the next stage: " + _currentStage);

                // �ʿ��� ��� �߰� ���� (�� �ε� ��) �߰� ����
            }
            else
            {
                Debug.Log("�̹� ������ ���������̰ų� �������� ��Ͽ��� ã�� �� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogError("Failed to parse _currentStage to StageIndex enum.");
        }
    }

    // ������ �������� Ŭ���� �� ���� é�ͷ� �̵�
    void LastStage()
    {
        if (Enum.TryParse(_currentChapter, out ChapterIndex currentChapterEnum))
        {
            int currentChapterIndex = Array.IndexOf(chapterIndices, currentChapterEnum);

            // ���� é�ͷ� �̵� �������� Ȯ��
            if (currentChapterIndex != -1 && currentChapterIndex < chapterIndices.Length - 1)
            {
                ChapterIndex nextChapter = chapterIndices[currentChapterIndex + 1];

                // ���� é���� ù ��° ���������� �̵�
                if (_dic.ContainsKey(nextChapter))
                {
                    _currentChapter = nextChapter.ToString();
                    _currentStage = _dic[nextChapter][0].ToString();  // ù ��° ���������� ����
                    LoadMap(_currentStage);
                    BeforeBtn.gameObject.SetActive(true);
                    Debug.Log("���� é�ͷ� �̵�: " + _currentChapter + ", ù ��° ��������: " + _currentStage);
                    ClearPopup.SetActive(false); // Ŭ���� �˾� �ݱ�
                }
                else
                {
                    Debug.Log("���� é�Ϳ� ���������� ���ǵ��� �ʾҽ��ϴ�.");
                }
            }
            else
            {
                Debug.Log("���� é�ʹ� ������ é���Դϴ�. �� �̻� ������ é�Ͱ� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogError("Failed to parse _currentChapter to ChapterIndex enum.");
        }
    }

    // UI ������Ʈ: ���� ���õ� é�Ϳ� �������� ǥ��
    void Update()
    {
        _currentChapterView.text = $"���� é�� : {_currentChapter}";
        _currentStageView.text = $"���� �������� : {_currentStage}";
    }
}
