using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectPopup : MonoBehaviour
{
    [SerializeField] private GameObject ChapterParent;
    [SerializeField] private GameObject StageParent;
    [SerializeField] private GameObject Btn_ExitPopup;
    [SerializeField] private GameObject Btn_ExitStage;

    private void OnEnable()
    {
        ChapterParent.SetActive(true);
        ChapterParent.SetActive(false);
        ChapterParent.SetActive(false);
        ChapterParent.SetActive(false);
    }
}
