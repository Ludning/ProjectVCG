using UnityEngine;

namespace ItemSystem
{
    // 요리 재료를 정의하는 최상위 클래스
    public class IngredientBase : MonoBehaviour
    {
        // 재료의 종류
        [SerializeField] private IngredientType type;
    }
}
