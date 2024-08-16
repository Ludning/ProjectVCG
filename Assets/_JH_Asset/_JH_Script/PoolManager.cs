using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolObjectType
{
    Capsule,
    Cube,
    Quad
}

[Serializable]
public class PoolInfo
{
    public PoolObjectType type;
    public int amount = 0;
    public GameObject prefab;
    public GameObject container;

    [HideInInspector]    
    public List<GameObject> pool = new List<GameObject>();   // 비활성화 상태의 오브젝트
}

public class PoolManager : SingleTonMono<PoolManager>
{
    [SerializeField]
    List<PoolInfo> listOfPool;                              // 다중 오브젝트풀 가능
    private Vector3 defaultPos = new Vector3(0,0,0);        // 오브젝트 소환 위치 임의 설정

    void Awake()
    {
        for (int i = 0; i < listOfPool.Count; i++)
        {
            FillPool(listOfPool[i]);
        }
    }

    //초기화
    void FillPool(PoolInfo info)
    {
        for (int i = 0; i < info.amount; i++)
        {
            GameObject obInstance = Instantiate(info.prefab, info.container.transform);
            obInstance.gameObject.SetActive(false);
            obInstance.transform.position = defaultPos;
            
            info.pool.Add(obInstance);//생성 되면 풀에 넣음 (초기 값 비활성화)
        }
    }

    public GameObject GetPoolObject(PoolObjectType type)
    {
        PoolInfo selected = GetPoolByType(type);
        if (selected == null) return null;
        List<GameObject> pool = selected.pool;
        GameObject obInstance = null;

        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy) 
            {
                obInstance = pool[i];
                pool.Remove(obInstance);
                break;
            }
        }

        if (obInstance == null)
        {
            if (pool.Count > 0)
            {
                obInstance = pool[pool.Count - 1];  // Gets the last object in the pool
                pool.Remove(obInstance);            // Removes it from the pool
            }
            else
            {
                obInstance = Instantiate(selected.prefab, selected.container.transform);
            }
        }
        obInstance.SetActive(true);
        return obInstance;
    }


    public void CoolObject(GameObject ob, PoolObjectType type)
    {
        ob.SetActive(false);
        ob.transform.position = defaultPos;
        PoolInfo selected = GetPoolByType(type);
        if (selected == null) return;
        selected.pool.Add(ob);
    }

    private PoolInfo GetPoolByType(PoolObjectType type)
    {
        for (int i = 0; i < listOfPool.Count; i++)
        {
            if (type == listOfPool[i].type)
                return listOfPool[i];
        }
        return null;
    }
}
