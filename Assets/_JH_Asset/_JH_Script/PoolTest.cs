using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class PoolTest : MonoBehaviour
{
    // 테스트용 코드
    void Start()
    {
        CoroutineGo();
    }

    // 테스트용 코드
    private void CoroutineGo()
    {
        StartCoroutine(AddObject()); 
    }

    // 테스트용 코드
    private IEnumerator AddObject()
    {
        var countLenth = 0;
        while (countLenth < 15)
        {
            GameObject obj = PoolManager.Instance.GetPoolObject(PoolObjectType.Capsule); // 풀매이저를 통해 생성 된 오브젝트를 활성화 하는 방법

            yield return new WaitForSeconds(1f);                                         // 1초 단위로 활성화

          
            StartCoroutine(CoolObject(obj, PoolObjectType.Capsule));                    
            countLenth++;
        }
    }

    private IEnumerator CoolObject(GameObject obj, PoolObjectType type)
    {
        yield return new WaitForSeconds(5f);                                            // 5초 단위로 반환
        if (obj != null && obj.activeInHierarchy)
        {
            PoolManager.Instance.CoolObject(obj, type);                                 // 풀매이저를 통해 생성 된 오브젝트를 반환 하는 방법
        }
    }
}
