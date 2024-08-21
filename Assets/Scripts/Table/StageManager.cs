using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public PlayerController Controller;
    public PlayerInventory Inventory;
    public TableManager Table;
    public MapReader[] Reader;  // 여러 맵을 관리하는 MapReader 배열
    public Answer answer;
    Dictionary<StageIndex, List<string>> tempDic = new Dictionary<StageIndex, List<string>>();

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

    // InitStage는 이제 StageIndex를 인자로 받음
    public void InitStage(StageIndex stageIndex)
    {
        // 각 Reader가 StageIndex를 기반으로 스테이지를 설정
        for (int i = 0; i < Reader.Length; i++)
        {
            if (Reader[i].name.ToString() == stageIndex.ToString())  // stageIndex 비교
            {
                Reader[i].gameObject.SetActive(true);
                Reader[i].ReadMap();  // 맵 로드
                CallAnswer(stageIndex);
            }
            else
            {
                Reader[i].gameObject.SetActive(false);
            }
        }

        // 플레이어 초기화
        Controller.Init(Table.startPosition, playerForwardDirection);
    }

    private void CallAnswer(StageIndex idx)
    {
        if (tempDic.TryGetValue(idx, out List<string> list))
        {
            // list[0]을 int로 변환
            if (int.TryParse(list[0], out int answerCount))
            {
                answer.InitAnswer(answerCount); // 정수형 값으로 InitAnswer 호출
            }
            else
            {
                Debug.LogError("list[0] 값이 유효한 숫자가 아닙니다.");
            }
        }
    }

}
