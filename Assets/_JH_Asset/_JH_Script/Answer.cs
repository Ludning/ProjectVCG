using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Answer : MonoBehaviour
{
    [SerializeField] GameObject BackBoxPrefab;   // 생성할 프리팹
    [SerializeField] Transform BackPanel;        // Answer Back Panel
    [SerializeField] Transform CubeParent;  // 부모 오브젝트
    [SerializeField] float boxSpacing = 0.02f;   // 박스 사이의 간격
    private float _answerBlockScale = 0.1f;      // 패딩 값

   

    public void InitAnswer(int AnswerCount)
    {
        foreach (Transform child in CubeParent)
        {
            Destroy(child.gameObject);
        }
        // BackPanel과 CubeParent의 초기 상태를 설정
        Vector3 scaleTemp = BackPanel.transform.localScale;
        Vector3 positionTemp = CubeParent.transform.position;

        // 박스들의 총 너비(박스 크기 + 간격)
        float totalWidth = (AnswerCount - 1) * (boxSpacing + _answerBlockScale) + _answerBlockScale;

        // CubeParent를 중앙으로 정렬하기 위해 시작 위치를 계산
        float startX = -(totalWidth / 2);

        float tempValue = startX;  // 시작 x 위치

        for (int i = 0; i < AnswerCount; i++)
        {
            // 새로운 박스를 생성하고 부모(CubeParent)에 할당
            GameObject newBackBox = Instantiate(BackBoxPrefab, CubeParent);

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
            BackPanel.transform.localScale = scaleTemp;

            // CubeParent의 위치는 중앙을 유지하도록 조정
            positionTemp.x = 0;
            CubeParent.transform.position = positionTemp;
        }
    }
}
