using Frameworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public int SelectedChapterIndex;
    public int SelectedStageIndex;
    public string StageIndex;

    public List<StageData> FilteredList;
    public StageData Stage;
}
