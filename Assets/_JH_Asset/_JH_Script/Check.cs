using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Check : MonoBehaviour
{
    private bool checkEmpty;

    // 표현식 본문을 사용한 게터와 세터
    public bool CheckEmpty
    {
        get
        {
            return checkEmpty ;
        }
        set
        {
            checkEmpty = value;
        }

    }
}
