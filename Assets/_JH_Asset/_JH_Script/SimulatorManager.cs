using System;
using UnityEngine;

public class SimulatorManager : SingleTonMono<SimulatorManager>
{
    /*CheckChildren(AnserObj.transform, mT);*/
    public Transform AnswerParent;
    public event Action<Transform> PlayingSimulator;

    public Material SetBlockMaterial(BlockLogicType type)
    {
        switch (type)
        {
            case BlockLogicType.Cook:
                return DataManager.Instance.Cook;
            case BlockLogicType.Move:
                return DataManager.Instance.Move;
            case BlockLogicType.PushItem:
                return DataManager.Instance.Push;
            case BlockLogicType.RotateLeft:
                return DataManager.Instance.Turn_Left;
            case BlockLogicType.RotateRight:
                return DataManager.Instance.Turn_Right;
            case BlockLogicType.SetItem:
                return DataManager.Instance.Pop;
            default:
                Debug.LogWarning("Unknown BlockLogicType: " + type);
                return DataManager.Instance.NullMat;  // 기본값 반환 (NullMat)
        }
    }
    
   

    public void CheckAnswer(bool isStart)
    {

        PlayingSimulator?.Invoke(AnswerParent);
    }
    public void CheckChildren( BlockLogicType type)
    {
        foreach (Transform child in AnswerParent)
        {
            if (child.childCount < 1)
            {
                Material _bt = SetBlockMaterial(type);
                Transform answerObj = Instantiate(child, child);
                Renderer renderer = answerObj.GetComponent<Renderer>();
                answerObj.gameObject.AddComponent<Check>();

                answerObj.transform.localScale = Vector3.one;
                answerObj.transform.localRotation = Quaternion.Euler(0, 0, 180f);
                answerObj.transform.localPosition = new Vector3(0, 0, -0.3f);

                if (renderer != null) renderer.material = _bt; // 여기서 selectedMaterial는 적절한 Material이어야 함
                else Debug.LogWarning("Renderer not found on answerObj: " + answerObj.name);

                return;
            }
        }
    }

    protected override void Init()
    {
        GameObject cubeParent = GameObject.Find("CubeParent");
        if (cubeParent != null) AnswerParent = cubeParent.transform;
        else Debug.LogWarning("CubeParnet not found!");
    }

    public bool CheckAnswerMax()
    {
        if(AnswerParent == null)
        {
            Init();
        }
        bool isCheck = true;
        foreach (Transform child in AnswerParent)
        {
            if (child.childCount < 1)
            {
                isCheck = false;
                return false;  // 즉시 함수가 종료되고 false 반환
            }
        }
        return isCheck;
    }

}
 
/*
if (CheckAnswerMax(AnserObj.transform)) return;
}*/
