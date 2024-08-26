using System.Collections.Generic;
using Frameworks;

namespace ItemSystem
{
    // 재료와 음식 간의 관계를 나타내는 레시피(Recipe)를 관리하는 매니저 클래스
    public class RecipeManager : Singleton<RecipeManager>
    {
        #region 변수(Field)
        
        // 레시피를 분석하는 클래스
        private readonly RecipeParser _parser;
        
        // 음식과 재료의 사전(Dictionary)
        public Dictionary<FoodType, FoodData> FoodDictionary { get; private set; }
        public Dictionary<IngredientType, IngredientData> IngredientDictionary { get; private set; }

        #endregion 변수(Field)
        
        #region 함수(Method)
        
        // 생성자(Constructor)
        public RecipeManager()
        {
            // 초기화가 필요한 변수들을 초기화한다.
            _parser = new RecipeParser();
            InitializeDictionaries(); // 사전들을 초기화한다.
        }

        // 사전을 초기화하는 함수
        private void InitializeDictionaries()
        {
            // 음식 사전
            FoodDictionary = new Dictionary<FoodType, FoodData>();
            FoodDictionary.Clear();
            _parser.Parse(FoodDictionary);
            
            // 재료 사전
            IngredientDictionary = new Dictionary<IngredientType, IngredientData>();
            IngredientDictionary.Clear();
            _parser.Parse(IngredientDictionary);
        }
        
        #endregion 함수(Method)
    }
}
