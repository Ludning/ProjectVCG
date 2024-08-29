using Frameworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public int SelectedChapterIndex;
    public int SelectedStageIndex;

    private void Update()
    {
        Debug.Log(SelectedChapterIndex + ":: Chapter");
        Debug.Log(SelectedStageIndex + ":: Stage ");
    }
}
