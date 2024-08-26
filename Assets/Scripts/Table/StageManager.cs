using TilemapSystem;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Header("Manager")]
    public TableManager Table;
    public SheetManager Sheet;
    public InteractableManager InteractableManager;
    
    public PlayerController Controller;
    public PlayerInventory Inventory;
    
    public Direction playerForwardDirection = Direction.Right;
    
    public MapReader[] Reader;  // ���� ���� �����ϴ� MapReader �迭
    public Answer answer;
    Dictionary<StageIndex, List<string>> tempDic = new Dictionary<StageIndex, List<string>>();

    // Awake()
    private void Awake()
    {
        tempDic.Add(StageIndex.Serving1,new List<string> {"5"});
        tempDic.Add(StageIndex.Serving2,new List<string> {"7"});
        tempDic.Add(StageIndex.Serving3,new List<string> {"9"});
        tempDic.Add(StageIndex.Serving4,new List<string> {"11"});
        tempDic.Add(StageIndex.Serving5,new List<string> {"11"});
        tempDic.Add(StageIndex.Serving6,new List<string> {"11"});
        tempDic.Add(StageIndex.Serving7,new List<string> {"11"});
        tempDic.Add(StageIndex.Serving8,new List<string> {"11"});
    }
    public Direction playerForwardDirection = Direction.Right;

    // InitStage�� ���� StageIndex�� ���ڷ� ����
    public void InitStage(StageIndex stageIndex)
    {
        // �� Reader�� StageIndex�� ������� ���������� ����
        for (int i = 0; i < Reader.Length; i++)
        {
            if (Reader[i].name.ToString() == stageIndex.ToString())  // stageIndex ��
            {
                Reader[i].gameObject.SetActive(true);
                Reader[i].ReadMap();  // �� �ε�
                CallAnswer(stageIndex);
            }
            else
            {
                Reader[i].gameObject.SetActive(false);
            }
        }

        // �÷��̾� �ʱ�ȭ
        Controller.Init(Table.startPosition, playerForwardDirection);
        InitializeField();
    }

    // 필요한 변수들을 초기화합니다.
    private void InitializeField()
    {
        // Controller.Init(tilemap.GetStartTileCenterPosition(), playerForwardDirection);
        
        InteractableManager.Init();
        Sheet.Init();
    }

    private void CallAnswer(StageIndex idx)
    {
        if (tempDic.TryGetValue(idx, out List<string> list))
        {
            // list[0]�� int�� ��ȯ
            if (int.TryParse(list[0], out int answerCount))
            {
                answer.InitAnswer(answerCount); // ������ ������ InitAnswer ȣ��
            }
            else
            {
                Debug.LogError("list[0] ���� ��ȿ�� ���ڰ� �ƴմϴ�.");
            }
        }
    }

}
