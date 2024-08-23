namespace ItemSystem
{
    // 요리 재료의 종류
    public enum IngredientType
    {
        None = default, // 없음 (기본 값)
        Potato, // 감자
        Tomato, // 토마토
    }
    
    public class IngredientData
    {
        public IngredientType ID { get; init; } // 식별자(ID); enum과 동기화
        public string Name { get; init; } // 이름
    }
}
