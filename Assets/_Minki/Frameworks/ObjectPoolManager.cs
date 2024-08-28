using UnityEngine;
using UnityEngine.Pool;

namespace Frameworks
{
    // 오브젝트 풀링에 사용할 오브젝트에 부착할 인터페이스
    public interface IObjectPoolComponent<T> where T : MonoBehaviour
    {
        // 오브젝트 풀링 인터페이스의 참조를 받아오는 함수
        void GetObjectPool(IObjectPool<T> objectPool);
    }
	
    // 오브젝트 풀링을 관리하는 클래스
    public class ObjectPoolManager<T> : MonoBehaviour where T : MonoBehaviour, IObjectPoolComponent<T>
    {
        [SerializeField] private T objectPoolPrefab; // 오브젝트 풀링에 사용할 게임 오브젝트 프리팹
        private IObjectPool<T> _objectPool; // 오브젝트 풀링 인터페이스 (UnityEngine.Pool)

        // IObjectPool<T>의 매개변수
        [SerializeField] private int defaultCapacity; // 기본 용량; 처음에 미리 생성해 둘 오브젝트의 개수를 지정한다.
        [SerializeField] private int maxSize; // 최대 용량; 생성한 오브젝트의 개수가 최대 용량을 초과할 경우, 초과량은 반환(Release)되지 않고 파괴(Destroy)된다.

        // Awake()
        private void Awake()
        {
            // 프리팹의 유효성(등록 여부)을 검사한다.
            CheckPrefabValidation();
            
            // 오브젝트 풀을 초기화한다.
            InitializeObjectPool();
        }

        // 프리팹의 유효성(등록 여부)을 검사한다.
        private void CheckPrefabValidation()
        {
            // 프리팹이 등록되지 않았을 경우, 오류 로그를 출력하고, 이 게임 오브젝트를 삭제한다.
            if (!objectPoolPrefab)
            {
                Debug.LogError("프리팹이 등록되지 않았습니다!");
                Destroy(gameObject);
            }
        }
	
        // 오브젝트 풀을 초기화한다.
        private void InitializeObjectPool()
        {
            _objectPool = new ObjectPool<T>(CreateObject, OnGetObject, OnReleaseObject, OnDestroyObject, defaultCapacity: defaultCapacity, maxSize: maxSize);
        }
	
        // 오브젝트 풀에 새 오브젝트를 생성한다.
        private T CreateObject()
        {
            T newObject = Instantiate(objectPoolPrefab);
            newObject.GetObjectPool(_objectPool); // T에서 GetObjectPool()을 정의하고, IObjectPool을 매개변수로 전달한다.
            return newObject;
        }

        // 오브젝트 풀에서 오브젝트를 가져간다.
        private void OnGetObject(T prefabComponent)
        {
            prefabComponent.gameObject.SetActive(true);
        }

        // 오브젝트 풀에 오브젝트를 돌려놓는다.
        private void OnReleaseObject(T prefabComponent)
        {
            prefabComponent.gameObject.SetActive(false);
        }

        // 오브젝트 풀에 오브젝트를 돌려놓지 않고 파괴한다.
        private void OnDestroyObject(T prefabComponent)
        {
            Destroy(prefabComponent.gameObject);
        }
    }
}

/*

※ 오브젝트 풀링에 사용하는 오브젝트의 클래스는 아래와 같이 정의하여 사용한다.

public class T : MonoBehaviour, IObjectPoolComponent<T>
{
    // 오브젝트 풀링 인터페이스
    private IObjectPool<T> _objectPool;
    
    // [IObjectPoolComponent<T>의 상속 함수] 오브젝트 풀 인터페이스의 참조를 받아서 저장하는 함수
    public void GetObjectPool(IObjectPool<T> objectPool)
    {
        _objectPool = objectPool;
    }

    // Release() 함수; 상황에 맞게 적절히 정의할 것.
    private void ReleaseToPool()
    {
        // 자기 자신을 오브젝트 풀에 반환한다.
        _objectPool.Release(this);
    }
}

*/
