using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RepeatCountController : MonoBehaviour
{
    [SerializeField] private RepeatSheet RepeatSheet;
    [SerializeField] private TextMeshProUGUI DisplayRepeatCount;
    public void OnClickSetRepeatCount(int count)
    {
        RepeatSheet.SetRepeatCount(count);
        DisplayRepeatCount.text = $"X{count}";
        gameObject.SetActive(false);
    }
}
