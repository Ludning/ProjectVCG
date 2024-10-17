using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Serialization;

public class GrapBlock : MonoBehaviour
{
    public SheetBase TargetSheet = null;
    public int BlockIndex = -1;
    
    private int prevIndex = -1;
    
    private CancellationTokenSource cancelToken_InsertObject;

    private void Update()
    {
        if (TargetSheet == null)
            return;

        BlockIndex = GetBlockLogicBetween(this.gameObject);

        if (BlockIndex != -1 && prevIndex != BlockIndex)
        {
            cancelToken_InsertObject?.Cancel();
            cancelToken_InsertObject = new CancellationTokenSource();
            prevIndex = BlockIndex;
            TargetSheet.UniTask_InsertObject(BlockIndex, cancelToken_InsertObject.Token).Forget();
        }
    }
    
    public int GetBlockLogicBetween(GameObject targetObject)
    {
        // targetObject의 위치
        Vector3 targetPosition = targetObject.transform.position;

        int count = TargetSheet._blockLogicBases.Count;

        // _blockLogicBases가 비어있는 경우
        if (count == 0)
            return 0;

        // _blockLogicBases가 하나일 때
        if (count == 1)
        {
            Vector3 singlePosition = TargetSheet._blockLogicBases[0].transform.position;
            // 타겟이 왼쪽에 있으면 0 반환, 오른쪽에 있으면 1 반환
            return targetPosition.x < singlePosition.x ? 0 : 1;
        }

        // _blockLogicBases가 두 개 이상일 때
        for (int i = 0; i < count - 1; i++)
        {
            // 두 BlockLogicBase의 위치
            Vector3 pos1 = TargetSheet._blockLogicBases[i].transform.position;
            Vector3 pos2 = TargetSheet._blockLogicBases[i + 1].transform.position;

            // targetPosition이 pos1과 pos2 사이에 있는지 확인
            if (IsBetween(targetPosition, pos1, pos2))
            {
                return i + 1; // pos1과 pos2 사이에 있는 경우, 첫 번째 BlockLogicBase의 인덱스 + 1 반환
            }
        }

        // 타겟이 가장 왼쪽에 있을 때
        Vector3 leftmostPosition = TargetSheet._blockLogicBases[0].transform.position;
        if (targetPosition.x < leftmostPosition.x)
        {
            return 0;
        }

        // 타겟이 가장 오른쪽에 있을 때
        Vector3 rightmostPosition = TargetSheet._blockLogicBases[count - 1].transform.position;
        if (targetPosition.x > rightmostPosition.x)
        {
            return count;
        }

        return -1; // 어떤 사이에도 없으면 -1 반환
    }

    private bool IsBetween(Vector3 target, Vector3 pointA, Vector3 pointB)
    {
        // pointA와 pointB 사이의 벡터
        Vector3 AB = pointB - pointA;
        Vector3 AT = target - pointA;

        // 내적을 이용해 target이 pointA와 pointB 사이에 있는지 검사
        float dotProduct = Vector3.Dot(AB.normalized, AT.normalized);

        // target이 점 A와 점 B 사이에 있는지 판단하기 위해서는
        // 점 T가 점 A와 동일한 방향에 있어야 하며, 점 B를 넘지 않아야 합니다.
        bool result = dotProduct > 0 && AT.magnitude <= AB.magnitude;
        Debug.Log($"result {result}");
        return result;
    }

    private void OnTriggerEnter(Collider other)
    {
        SheetBase sheet = other.GetComponent<SheetBase>();
        if (sheet == null)
            return;

        TargetSheet = sheet;
    }

    private void OnTriggerExit(Collider other)
    {
        SheetBase sheet = other.GetComponent<SheetBase>();
        if (sheet == null)
            return;
        
        TargetSheet = null;
        BlockIndex = -1;
        prevIndex = -1;
    }
}
