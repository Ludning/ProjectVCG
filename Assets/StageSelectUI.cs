using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectUI : MonoBehaviour
{
    public StageManager StageManager;
    public GameObject GameTable;
    public int _selectedStageIndex;
    
    public void OnSelectStage(int stageIndex)
    {
        _selectedStageIndex = stageIndex;
    }
    public void OnClick_OK()
    {
        if (_selectedStageIndex == 0)
            return;
        //게임판을 활성화
        GameTable.SetActive(true);
        
        StageManager.InitStage();
        
        
        this.gameObject.SetActive(false);
    }
    public void OnClick_Back()
    {
        this.gameObject.SetActive(false);
    }
}
