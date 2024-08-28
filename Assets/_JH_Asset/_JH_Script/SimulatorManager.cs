using Frameworks;
using System;
using UnityEngine;

public class SimulatorManager : SingletonMonoBehaviour<SimulatorManager>
{
    public Transform AnswerParent;
    public event Action<Transform> PlayingSimulator;
    
    public void CheckAnswer(bool isStart)
    {
        PlayingSimulator?.Invoke(AnswerParent);
    }
    
    public void CheckChildren(BlockLogicType type)
    {
        foreach (Transform child in AnswerParent)
        {
            if (child.childCount < 1)
            {
                Material _bt = ResourceManager.Instance.LoadResourceWithCaching<Material>(type.ToString());
                Transform answerObj = Instantiate(child, child);
                Renderer renderer = answerObj.GetComponent<Renderer>();
                answerObj.gameObject.AddComponent<Check>();

                answerObj.transform.localScale = Vector3.one;
                answerObj.transform.localRotation = Quaternion.Euler(0, 0, 180f);
                answerObj.transform.localPosition = new Vector3(0, 0, -0.3f);

                if (renderer) renderer.material = _bt; // 여기서 selectedMaterial는 적절한 Material이어야 함
                else Debug.LogWarning("Renderer not found on answerObj: " + answerObj.name);

                return;
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }
    
    private void Initialize()
    {
        GameObject cubeParent = GameObject.Find("CubeParent");
        if (cubeParent) AnswerParent = cubeParent.transform;
        else Debug.LogWarning("CubeParent is not found!");
    }
    
    public bool CheckAnswerMax()
    {
        if(!AnswerParent)
        {
            Initialize();
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
