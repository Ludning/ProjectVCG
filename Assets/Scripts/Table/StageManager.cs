using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public PlayerController Controller;
    public PlayerInventory Inventory;
    public TableManager Table;
    public MapReader[] Reader;

    public Direction playerForwardDirection = Direction.Right;
    public void InitStage(int num)
    {
        // 모든 Reader를 순회하면서 활성화/비활성화 상태를 설정합니다.
        for (int i = 0; i < Reader.Length; i++)
        {
            if (i == num)
            {
                Reader[i].gameObject.SetActive(true); // 선택한 스테이지 활성화
                Reader[i].ReadMap();                  // 맵 로드
            }
            else
            {
                Reader[i].gameObject.SetActive(false); // 다른 스테이지 비활성화
            }
        }

        // 스테이지 초기화
        Controller.Init(Table.startPosition, playerForwardDirection);
    }

}
