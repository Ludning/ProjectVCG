using System.Linq;
using UnityEngine;

public class Answer : MonoBehaviour
{
    [SerializeField] private GameObject backBoxPrefab;   // 생성할 프리팹
    [SerializeField] private Transform backPanel;        // Answer Back Panel
    [SerializeField] private Transform cubeParent;  // 부모 오브젝트
    [SerializeField] private float boxSpacing = 0.02f;   // 박스 사이의 간격
    private float _answerBlockScale = 0.1f;      // 패딩 값
    
    public void InitializeAnswer(int answerCount)
    {
        DestroyChildren(cubeParent);
        
        // BackPanel과 CubeParent의 초기 상태를 설정
        Vector3 scaleTemp = backPanel.transform.localScale;
        Vector3 positionTemp = cubeParent.transform.position;

        // 박스들의 총 너비(박스 크기 + 간격)
        float totalWidth = (answerCount - 1) * (boxSpacing + _answerBlockScale) + _answerBlockScale;

        // CubeParent를 중앙으로 정렬하기 위해 시작 위치를 계산
        float startX = -(totalWidth / 2);

        float tempValue = startX;  // 시작 x 위치

        for (int i = 0; i < answerCount; i++)
        {
            // 새로운 박스를 생성하고 부모(CubeParent)에 할당
            GameObject newBackBox = Instantiate(backBoxPrefab, cubeParent);

            if (i != 0)
            {
                // 첫 번째 박스를 제외한 나머지 박스들은 간격을 추가하여 위치 설정
                tempValue += _answerBlockScale + boxSpacing;
            }
            else
            {
                // 첫 번째 박스는 별도로 간격 없이 설정
                tempValue += _answerBlockScale / 2;
            }

            // 박스의 위치를 설정
            newBackBox.transform.localPosition = new Vector3(tempValue, 0, 0.021f);

            // BackPanel의 크기를 박스와 간격에 맞춰 확장
            scaleTemp.x = totalWidth;
            backPanel.transform.localScale = scaleTemp;

            // CubeParent의 위치는 중앙을 유지하도록 조정
            positionTemp.x = 0;
            cubeParent.transform.position = positionTemp;
        }
    }

    private void DestroyChildren(Transform parent)
    {
        Transform[] children = parent.GetComponentsInChildren<Transform>().Skip(1).ToArray();

        foreach (Transform child in parent)
            Destroy(child);
    }
}
