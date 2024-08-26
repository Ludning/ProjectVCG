using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;

namespace ItemSystem
{
    // 레시피 데이터를 분석하는 클래스
	// [주의]: 아래의 코드는 '정해진 방법'으로 변환한 XML 파일 형식을 기준으로 한다.
    public class RecipeParser
    {
        // 레시피를 정리한 엑셀 파일의 절대 경로 (Assets/Databases/Recipes)
        private readonly string _recipePath = Path.Combine(Application.dataPath, "Databases", "Recipes");

        // 데이터를 분석한다.
        public bool Parse<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
			// 사전의 종류에 맞는 분석 함수를 가져와 호출한다.
	        var parser = GetParserByType(dictionary);
			return parser.Invoke();
		}
		
		// 사전의 종류에 맞는 분석 함수를 반환한다.
        private Func<bool> GetParserByType<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
            Func<bool> parsingFunc = typeof(TKey) switch
            {
                _ when typeof(TKey) == typeof(FoodType) => () => ParseFoodDB(dictionary as Dictionary<FoodType, FoodData>),
                _ when typeof(TKey) == typeof(IngredientType) => () => ParseIngredientDB(dictionary as Dictionary<IngredientType, IngredientData>),
                _ => default,
            };
			
            return parsingFunc;
        }
		
		// 음식 데이터를 분석한다.
		private bool ParseFoodDB(Dictionary<FoodType, FoodData> foodDic)
		{
			// FoodDB.xml 파일을 지정한다.
			string filePath = Path.Combine(_recipePath, "FoodDB.xml");
			
			// XML 파일을 불러온다. 실패했을 경우, false를 반환하여 즉시 종료한다.
            if (!LoadXDocument(filePath, out XDocument xDocument)) return false;

			// "data" 영역을 추출한다.
            var dataElements = xDocument.Descendants("data");

            // 추출 영역을 순회하면서, 데이터를 삽입한다.
            foreach (XElement element in dataElements)
            {
                // 데이터를 추출한다.
                var foodID = Enum.IsDefined(typeof(FoodType), int.TryParse(element.Attribute("ID")?.Value, out int id) ? id : -1) ? (FoodType)id : default;
                var foodName = element.Attribute("Name")?.Value ?? string.Empty;
                var requiredIngredients = element.Attribute("Required Ingredient")?.Value.Split(',').Select(int.Parse).Select(i => (IngredientType)i).ToArray();

                // 추출한 데이터가 잘못되었을 경우, 오류를 보여주고 다음 데이터로 넘어간다.
                if (foodID == default || string.IsNullOrEmpty(foodName) || requiredIngredients == null)
                {
                    Debug.LogError("음식 데이터를 불러오는 중에 잘못된 데이터를 발견했습니다!");
                    return false;
                }
                
                // 추출한 데이터를 기반으로, FoodData를 생성한다.
                FoodData newFood = new()
                {
                    ID = foodID,
                    Name = foodName,
                    RequiredIngredients = requiredIngredients,
                };

                // 생성한 음식 데이터를 사전에 등록한다. 실패할 경우, false를 반환하여 즉시 종료한다.
                if (!foodDic.TryAdd(foodID, newFood))
				{
					Debug.LogError("추출한 데이터를 사전에 넣지 못했습니다!");
					return false;
				}
            }
			
			// 위의 코드를 모두 실행했다면, true를 반환한다.
			return true;
		}
		
		// 재료 데이터를 분석한다.
		private bool ParseIngredientDB(Dictionary<IngredientType, IngredientData> ingredientDic)
		{
			// IngredientDB.xml 파일을 지정한다.
			string filePath = Path.Combine(_recipePath, "IngredientDB.xml");
			
			// XML 파일을 불러온다. 실패했을 경우, false를 반환하여 즉시 종료한다.
            if (!LoadXDocument(filePath, out XDocument xDocument)) return false;

			// "data" 영역을 추출한다.
            var dataElements = xDocument.Descendants("data");

            // 추출 영역을 순회하면서, 데이터를 삽입한다.
            foreach (XElement element in dataElements)
            {
                // 데이터를 추출한다.
                var ingredientID = Enum.IsDefined(typeof(IngredientType), int.TryParse(element.Attribute("ID")?.Value, out int id) ? id : -1) ? (IngredientType)id : default;
                var ingredientName = element.Attribute("Name")?.Value ?? string.Empty;

                // 추출한 데이터가 잘못되었을 경우, 오류를 보여주고 다음 데이터로 넘어간다.
                if (ingredientID == default || string.IsNullOrEmpty(ingredientName))
                {
                    Debug.LogError("재료 데이터를 불러오는 중에 잘못된 데이터를 발견했습니다!");
                    return false;
                }
                
                // 추출한 데이터를 기반으로, FoodData를 생성한다.
                IngredientData newIngredient = new()
                {
                    ID = ingredientID,
                    Name = ingredientName,
                };

                // 생성한 재료 데이터를 사전에 등록한다. 실패할 경우, false를 반환하여 즉시 종료한다.
                if (!ingredientDic.TryAdd(ingredientID, newIngredient))
				{
					Debug.LogError("추출한 데이터를 사전에 넣지 못했습니다!");
					return false;
				}
            }
			
			// 위의 코드를 모두 실행했다면, true를 반환한다.
			return true;
		}
		
		// XML 파일을 불러오는 함수; 예외 처리를 적용한다.
        private bool LoadXDocument(string path, out XDocument xDocument)
        {
            try // XML 파일을 불러온다.
            {
                xDocument = XDocument.Load(path);
                return true; // 정상적으로 불러왔을 경우, true를 반환하여 함수를 끝낸다.
            }
            catch (Exception e) // 예외가 발생할 경우, 오류 로그를 출력하고, false를 반환하여 함수를 끝낸다.
            {
                xDocument = default; // = xDocument = null;
                Debug.LogError($"XML 파일을 불러오는 중에 예외가 발생했습니다! 예외 정보: {e.Message}");
                return default; // = return false;
            }
        }
    }
}
