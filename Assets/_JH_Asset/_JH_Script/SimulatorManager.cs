using System;
using UnityEngine;

public class SimulatorManager : SingleTonMono<SimulatorManager>
{
    /*CheckChildren(AnserObj.transform, mT);*/
    public Transform AnswerParent;
    public event Action<Transform> PlayingSimulator;


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
                Material _bt = ResourceManager.Instance.LoadResourceWithCaching<Material>(type.ToString());
                Transform answerObj = Instantiate(child, child);
                Renderer renderer = answerObj.GetComponent<Renderer>();
                answerObj.gameObject.AddComponent<Check>();

                answerObj.transform.localScale = Vector3.one;
                answerObj.transform.localRotation = Quaternion.Euler(0, 0, 180f);
                answerObj.transform.localPosition = new Vector3(0, 0, -0.3f);

                if (renderer != null) renderer.material = _bt; // ���⼭ selectedMaterial�� ������ Material�̾�� ��
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
                return false;  // ��� �Լ��� ����ǰ� false ��ȯ
            }
        }
        return isCheck;
    }

}
 
/*
if (CheckAnswerMax(AnserObj.transform)) return;
}*/
