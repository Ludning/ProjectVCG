using System;
using UnityEngine;

public class GameInstaller : MonoBehaviour
{
    [SerializeField] private StageManager _stageManager;
    [SerializeField] private TableManager _tableManager;
    [SerializeField] private SheetManager _sheetManager;
    [SerializeField] private InteractableManager _interactableManager;
    [SerializeField] private LevelManager _levelManager;
    [SerializeField] private NPCManager _npcManager;
    [SerializeField] private UIContainer _uiContainer;

    private void Start()
    {
        GameManager.Instance.Init();

        _uiContainer.InitStageSelect();
    }
}
