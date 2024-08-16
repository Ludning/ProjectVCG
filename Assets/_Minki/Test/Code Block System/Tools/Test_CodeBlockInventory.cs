using UnityEngine;

namespace CodeBlockSystem
{
    // 코드 블록을 제공하는 객체(보관함)
    public class CodeBlockInventory : MonoBehaviour
    {
        // 변수
        [SerializeField] private BaseCodeBlock[] codeBlocks; // 제공하는 코드 블록
        [SerializeField] private Transform[] blockTransforms; // 코드 블록이 생성될 위치
        private CodeBlockExecutor _executor; // 코드 블록 실행자 객체

        // 함수
        private void Awake() // Awake()
        {
            // 실행자를 초기화합니다.
            _executor = FindAnyObjectByType<CodeBlockExecutor>(); // 실행자는 Find() 함수를 사용하여 찾습니다.

            // 미리 지정한 코드 블록들을 생성합니다.
            CreateCodeBlock(codeBlocks, blockTransforms);
            
            // 각 코드 블록마다 터치 이벤트를 등록합니다.
            foreach (BaseCodeBlock codeBlock in codeBlocks)
            {
                codeBlock.AddTouchListener(SelectCodeBlock); // 코드 블록 선택 함수를 등록합니다.
            }
        }

        // 코드 블록을 생성하는 함수
        private void CreateCodeBlock(BaseCodeBlock[] codeBlock, Transform[] blockTransform)
        {
            for (int i = 0; i < codeBlock.Length; i++) // 지정한 코드 블록의 개수만큼,
            {
                Instantiate(codeBlock[i], blockTransform[i]).AddTouchListener(SelectCodeBlock); // 지정한 위치에 생성하고, 터치 이벤트를 등록합니다.
            }
        }

        // 코드 블록을 선택하는 함수
        private void SelectCodeBlock(BaseCodeBlock codeBlock)
        {
            // 코드 블록을 옮기는 것이 아니라, 복사본을 생성하여 실행자에 전달합니다.
            BaseCodeBlock selectedCodeBlock = Instantiate(codeBlock);
            selectedCodeBlock.RemoveTouchListener(SelectCodeBlock); // 복사본의 터치 이벤트를 해제합니다.
            _executor.AddCodeBlock(selectedCodeBlock); // 실행자에 복사본을 전달합니다.
        }
    }
}
