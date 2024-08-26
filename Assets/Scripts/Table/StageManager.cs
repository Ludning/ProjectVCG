using TilemapSystem;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Header("Manager")]
    public TableManager Table;
    public SheetManager Sheet;
    public InteractableManager InteractableManager;
    
    public PlayerController Controller;
    public PlayerInventory Inventory;
    
    public Direction playerForwardDirection = Direction.Right;

    // Awake()
    private void Awake()
    {
        InitializeField();
    }

    // 필요한 변수들을 초기화합니다.
    private void InitializeField()
    {
        // Controller.Init(tilemap.GetStartTileCenterPosition(), playerForwardDirection);
        
        InteractableManager.Init();
        Sheet.Init();
    }
}
