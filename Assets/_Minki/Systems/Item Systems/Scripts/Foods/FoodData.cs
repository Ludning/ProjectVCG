namespace ItemSystem
{
    // 완성한 음식의 종류
    public enum FoodType
    {
        Fail = default, // 실패 (기본 값; 잘못된 재료나 조리 방법을 사용하여 요리할 경우 반환되는 값)
        PotatoChip = 1, // 감자 튀김
        Pizza = 2, // 피자
    }
    
    public class FoodData
    {
        public FoodType ID { get; init; } // 식별자(ID); enum과 동기화
        public string Name { get; init; } // 이름
        public IngredientType[] RequiredIngredients { get; init; } // 요리 재료 목록
    }
}
