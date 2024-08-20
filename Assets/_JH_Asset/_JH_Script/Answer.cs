using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Answer : MonoBehaviour
{
    [SerializeField] GameObject BackBoxPrefab;   // 생성할 프리팹
    [SerializeField] Transform BackPanel;        // Answer Back Panel
    [SerializeField] Transform CubeParent;  // 부모 오브젝트
    [SerializeField] float _padding = 0.12f;      // 패딩 값

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            InitAnswer(10);
        }
    }
    public void InitAnswer(int AnswerCount)
    {
        float tempValue = 0;
        float tempPaddingValue = 0;
        for (int i = 0; i < AnswerCount; i++)
        {
            // 프리팹 인스턴스 생성
            GameObject newBackBox = Instantiate(BackBoxPrefab, CubeParent);

            // 생성된 오브젝트의 위치 설정 (패딩을 기반으로 가로 방향으로 배치)
            newBackBox.transform.localPosition = new Vector3(i * _padding, 0, 0.021f);
            tempValue += BackBoxPrefab.transform.localScale.x;
            tempPaddingValue += _padding;

        }
        tempPaddingValue *= 0.1f;

        Debug.Log(tempValue);
        MovePanelPosition(AnswerCount, tempValue, tempPaddingValue);
    }
    public void MovePanelPosition(int AnswerCount, float v, float tempPaddingValue)
    {
        Vector3 tScale = BackPanel.transform.localScale;
        tScale.x = v + tempPaddingValue;
        BackPanel.transform.localScale = tScale;

        Vector3 tPosition = CubeParent.transform.position;
        tPosition.x = -(tScale.x / 2);
        CubeParent.transform.position = tPosition;
        /*    float tempBoxScaleX = BackBoxPrefab.transform.localScale.x;
            float tempBoxWidth = tempBoxScaleX * AnswerCount;
            //tempBoxWidth += (AnswerCount - 1) * 0.02f; 
            Vector3 tempScale = BackPanel.transform.localScale;
            tempScale.x = tempBoxWidth;
            BackPanel.transform.localScale = tempScale;

            Vector3 tempPosition = CubeparentTransform.transform.localPosition;
            tempPosition.x = -(tempBoxWidth / 2);
            CubeparentTransform.transform.localPosition = tempPosition;
          *//*  Vector3 tempPosition = BackPanel.transform.localPosition;
            tempPosition.x = tempBoxScaleX / 3;
            BackPanel.transform.localPosition = tempPosition;*/
    }
}
