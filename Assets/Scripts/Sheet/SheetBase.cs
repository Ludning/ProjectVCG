using Cysharp.Threading.Tasks;
using Oculus.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class SheetBase : MonoBehaviour
{
    public Transform SheetParent;
    public string SheetName;
    [ReadOnly] public SheetManager SheetManager;
    public int SheetLimit { get; set; }
    public int BlockCount { get; set; }

    public List<BlockSlot> BlockSlots = new List<BlockSlot>();
    
    public List<BlockLogicBase> _blockLogicBases = new List<BlockLogicBase>();
    private float _moveDuration = 0.3f;

    [SerializeField] private BoxCollider _boxCollider;
    
    public void Init(int sheetLimit)
    {
        Clear();
        BlockCount = 0;
        SheetLimit = sheetLimit;
        
        for (int i = 0; i < SheetLimit; i++)
        {
            Debug.Log("Instantiate blockSlot");
            GameObject slotPrefab = ResourceManager.Instance.LoadResourceWithCaching<GameObject>("BlockSlot");
            BlockSlot slot = Instantiate(slotPrefab, SheetParent).GetComponent<BlockSlot>();
            BlockSlots.Add(slot);
        }

        if (_boxCollider == null)
            return;
        float width = 0.8f + ((SheetLimit > 1) ? SheetLimit * 1.2f : 1.2f);
        float center = (this is MainSheet) ? width / 2 : -width / 2;
        _boxCollider.size = new Vector3(width, _boxCollider.size.y, _boxCollider.size.z);
        _boxCollider.center = new Vector3(center, _boxCollider.center.y, _boxCollider.center.z);
    }

    public void Clear()
    {
        foreach (var blockSlot in BlockSlots)
        {
            Debug.Log("Destroy blockSlot");
            Destroy(blockSlot.gameObject);
        }
        BlockSlots.Clear();
    }
    public void ClearBlockLogic()
    {
        BlockCount = 0;
        foreach (var blockLogic in _blockLogicBases)
        {
            Destroy(blockLogic.gameObject);
        }
        _blockLogicBases.Clear();
    }

    public void Push(BlockLogicBase blockLogic)
    {
        Debug.Log($"BlockCount : {BlockCount}");
        Debug.Log($"_blockLogicBases.Count : {_blockLogicBases.Count}");
        BlockSlots[BlockCount].SetBlockLogic(blockLogic);
        _blockLogicBases.Add(blockLogic);

        blockLogic.Init(this);
    }
    
    public void InsertBlockLogicAtIndex(BlockLogicBase blockLogic, int index)
    {
        // index 위치에 새로운 오브젝트를 삽입
        _blockLogicBases.Insert(index, blockLogic);
        
        blockLogic.Init(this);
        
        RenewalBlockPosition();
    }
    
    public void RemoveBlockLogicAtIndex(BlockLogicBase blockLogic)
    {
        int index = _blockLogicBases.IndexOf(blockLogic);
        // index 위치의 오브젝트를 제거
        if (index >= 0 && index < _blockLogicBases.Count)
        {
            _blockLogicBases.RemoveAt(index);
        }

        BlockCount--;
        RenewalBlockPosition();
    }
    /*public void RemoveBlockLogicAtIndex(BlockLogicBase blockLogic)
    {
        // index 위치의 오브젝트를 제거
        int index = _blockLogicBases.IndexOf(blockLogic);
        RemoveBlockLogicAtIndex(index);
    }
    public void RemoveBlockLogicAtIndex(int index)
    {
        // index 위치의 오브젝트를 제거
        if (index >= 0 && index < _blockLogicBases.Count)
        {
            //if(_blockLogicBases[index] != null)
            Destroy(_blockLogicBases[index].gameObject);
            _blockLogicBases.RemoveAt(index);
        }
        
        RenewalBlockPosition();
    }*/

    private void RenewalBlockPosition()
    {
        for (int i = 0; i < _blockLogicBases.Count; i++)
        {
            BlockSlots[i].SetBlockLogic(_blockLogicBases[i]);
        }
    }
    
    // 특정 위치를 제외한 나머지 오브젝트를 오른쪽으로 이동시키는 함수
    public async UniTaskVoid UniTask_InsertObject(int insertIndex, CancellationToken token)
    {
        if (_blockLogicBases.Count == 0)
            return;
        
        
        float elapsedTime = 0f;
        while (elapsedTime < _moveDuration)
        {
            foreach (var blockLogic in _blockLogicBases)
            {
                //int index = blockLogic[insertIndex];
                int index = _blockLogicBases.IndexOf(blockLogic);
                int targetIndex = (index < insertIndex) ? index : index + 1;
                blockLogic.transform.position = Vector3.Lerp(blockLogic.transform.position, BlockSlots[targetIndex].transform.position, elapsedTime / _moveDuration);
            }
            elapsedTime += Time.deltaTime;
            await UniTask.Yield(cancellationToken: token);

            if (token.IsCancellationRequested)
                break; // 취소 요청이 있을 경우 루프 종료
        }
    }
    /*public async UniTaskVoid UniTask_InsertObject(int insertIndex, CancellationToken token)
    {
        List<BlockLogicBase> moveLogics = new List<BlockLogicBase>();
        for (int i = insertIndex; i < SheetLimit; i++)
        {
            if (_blockLogicBases.Count <= i)
                break;
            
            moveLogics.Add(_blockLogicBases[i]);
        }
        if (moveLogics.Count == 0)
            return;
        
        
        float elapsedTime = 0f;
        while (elapsedTime < _moveDuration)
        {
            foreach (var moveLogic in moveLogics)
            {
                int index = moveLogics.IndexOf(moveLogic);
                moveLogic.transform.position = Vector3.Lerp(BlockSlots[index].transform.position, BlockSlots[index + 1].transform.position, elapsedTime / _moveDuration);
            }
            elapsedTime += Time.deltaTime;
            await UniTask.Yield(cancellationToken: token);

            if (token.IsCancellationRequested)
                break; // 취소 요청이 있을 경우 루프 종료
        }
    }*/
    
    // 특정 위치에 오브젝트를 제거하고 나머지 오브젝트를 왼쪽으로 이동시키는 함수
    public async UniTaskVoid UniTask_RemoveObject(BlockLogicBase removeObject, int removeIndex)
    {
        List<BlockLogicBase> moveLogics = new List<BlockLogicBase>();
        for (int i = removeIndex + 1; i < SheetLimit; i++)
        {
            if (_blockLogicBases.Count <= i)
                break;
            
            moveLogics.Add(_blockLogicBases[i]);
        }
        if (moveLogics.Count == 0)
            return;
        
        
        float elapsedTime = 0f;
        while (elapsedTime < _moveDuration)
        {
            foreach (var moveLogic in moveLogics)
            {
                int index = moveLogics.IndexOf(moveLogic);
                moveLogic.transform.position = Vector3.Lerp(BlockSlots[index].transform.position, BlockSlots[index - 1].transform.position, elapsedTime / _moveDuration);
            }
            elapsedTime += Time.deltaTime;
            await UniTask.Yield();
        }
    }

    public async UniTaskVoid UniTask_RevertPosition(CancellationToken token)
    {
        float elapsedTime = 0f;
        while (elapsedTime < _moveDuration)
        {
            foreach (var blockLogic in _blockLogicBases)
            {
                Vector3 start = blockLogic.transform.position;
                if(start == Vector3.zero)
                    continue;
                int index = _blockLogicBases.IndexOf(blockLogic);
                blockLogic.transform.position = Vector3.Lerp(start, BlockSlots[index].transform.position, elapsedTime / _moveDuration);
            }
            elapsedTime += Time.deltaTime;
            await UniTask.Yield(cancellationToken: token);
        }
    }
}
