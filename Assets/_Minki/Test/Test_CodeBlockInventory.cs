using UnityEngine;

namespace CodeBlockSystem
{
    // 코드 블록을 제공하는 객체(보관함)
    public class CodeBlockInventory : MonoBehaviour
    {
        // 변수
        [SerializeField] private CodeBlock[] codeBlocks; // 제공하는 코드 블록
        private CodeBlockExecutor _executor; // 코드 블록 실행자 객체

        // 함수
        private void Awake() // Awake()
        {
            _executor = FindAnyObjectByType<CodeBlockExecutor>(); // 실행자는 Find() 함수를 사용하여 찾습니다.

            // 각 코드 블록마다 터치 이벤트를 등록합니다.
            foreach (CodeBlock codeBlock in codeBlocks)
            {
                codeBlock.AddTouchListener(SelectCodeBlock); // 코드 블록 선택 함수를 등록합니다.
            }
        }

        // 코드 블록을 선택하는 함수
        private void SelectCodeBlock(CodeBlock codeBlock)
        {
            // 코드 블록을 옮기는 것이 아니라, 복사본을 생성하여 실행자에 전달합니다.
            CodeBlock newCodeBlock = Instantiate(codeBlock.gameObject).GetComponent<CodeBlock>();
            newCodeBlock.RemoveTouchListener(SelectCodeBlock); // 복사본의 터치 이벤트를 해제합니다.
            _executor.AddCodeBlock(newCodeBlock); // 실행자에 복사본을 전달합니다.
        }
    }
}