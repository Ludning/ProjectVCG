using UnityEngine;
using UnityEngine.Pool;

namespace Frameworks
{
    public interface IObjectPoolComponent<T> where T : MonoBehaviour
    {
        void GetObjectPool(IObjectPool<T> objectPool);
    }
	
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
            // 오브젝트 풀을 초기화한다.
            InitializeObjectPool();
        }
	
        // 오브젝트 풀을 초기화한다.
        private void InitializeObjectPool()
        {
            _objectPool = new ObjectPool<T>(CreateObject, OnGetObject, OnReleaseObject, OnDestroyObject, defaultCapacity: defaultCapacity, maxSize: maxSize);
        }
	
        // 오브젝트 풀에 새 오브젝트를 생성한다.
        private T CreateObject()
        {
            if (Instantiate(objectPoolPrefab).TryGetComponent(out T newObject))
            {
                newObject.GetObjectPool(_objectPool); // T에서 GetObjectPool()을 정의하고, IObjectPool을 매개변수로 전달한다.
                return newObject;
            }
            else
            {
                Debug.LogError("오브젝트 풀에서 오브젝트를 생성하지 못했습니다. 프리팹이 필요한 컴포넌트를 부착하고 있는지 확인하세요.");
                Destroy(newObject.gameObject);
                return default;
            }
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
