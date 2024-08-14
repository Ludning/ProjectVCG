using System.Collections;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    
    [SerializeField] private Transform targetTransform;

    private TestCodeBlock[] _codeBlocks;

    private void Start()
    {
        _codeBlocks = new TestCodeBlock[10000];

        for (int i = 0; i < 10000; i++)
        {
            _codeBlocks[i] = new();
        }
    }
    
    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position = Vector3.MoveTowards(current: transform.position, target: targetTransform.position, maxDistanceDelta: moveSpeed * Time.deltaTime);
            transform.forward = Vector3.RotateTowards(current: transform.forward, target: targetTransform.position - transform.position, maxRadiansDelta: rotateSpeed * Time.deltaTime, maxMagnitudeDelta: rotateSpeed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(ExecuteBlocks());
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            ExecuteBlock();
        }
    }

    private void ExecuteBlock()
    {
        foreach (TestCodeBlock codeBlock in _codeBlocks)
        {
            codeBlock.Execute();
        }
    }

    private IEnumerator ExecuteBlocks()
    {
        foreach (TestCodeBlock codeBlock in _codeBlocks)
        {
            Debug.Log("기다리고 있어?");
            yield return StartCoroutine(codeBlock.Execute(this)); // Expensive Method Invocation?
        }
    }
}

public class TestCodeBlock
{
    public void Execute()
    {
        Debug.Log("TestCodeBlock이 실행되었습니다.");
    }
    
    public IEnumerator Execute(TestScript test)
    {
        yield return new WaitForSeconds(1.0f);
        Debug.Log("TestCodeBlock이 실행되었습니다.");
    }
}