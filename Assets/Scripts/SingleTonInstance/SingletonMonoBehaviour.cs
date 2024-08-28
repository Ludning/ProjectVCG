using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// MonoBehaviour를 상속받는 싱글톤 일반화 클래스
/// </summary>
/// <typeparam name="T">상속받는 클래스</typeparam>
public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    // MonoBehaviour의 특성으로, 생성자를 사용할 수 없는 등의 제약이 있다.
    private static readonly Lazy<T> _instance = new(CreateInstance);
    public static T Instance => _instance.Value;
    
    // Lazy<T>가 인스턴스를 생성할 때 호출하는 함수
    private static T CreateInstance()
    {
        T instance = FindAnyObjectByType<T>(); // 인스턴스가 존재하지 않을 경우, Find 함수로 씬 내에서 해당 컴포넌트가 부착된 게임 오브젝트를 찾는다.
        instance ??= new GameObject(nameof(T)).AddComponent<T>(); // 여전히 존재하지 않을 경우, 새 게임 오브젝트를 생성하고, 해당 컴포넌트를 부착한다.
        DontDestroyOnLoad(instance); // DontDestroyOnLoad()를 적용한다.
        return instance;
    }

    // Awake()
    protected virtual void Awake()
    {
        // 인스턴스가 이미 생성되었으나, 현재 객체가 그 인스턴스가 아닐 경우, 이 게임 오브젝트를 파괴한다.
        if (_instance.IsValueCreated && _instance.Value != this)
            Destroy(gameObject);
    }
}
