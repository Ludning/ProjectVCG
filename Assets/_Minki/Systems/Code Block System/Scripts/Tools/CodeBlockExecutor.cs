using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace CodeBlockSystem
{
    // 플레이어가 배치한 코드 블록을 실행하는 객체(실행자)
    public class CodeBlockExecutor : MonoBehaviour
    {
        // 변수
        // private PlayerCharacter _player; // 플레이어 객체
        private LinkedList<CodeBlockBase> _sourceCode; // 플레이어가 코드 블록을 배치하여 만든 소스 코드
        [SerializeField] private Transform[] blockTransforms; // 코드 블록이 배치될 위치
        [SerializeField] private Button executeButton; // 소스 코드를 실행하는 버튼 [TODO: 버튼을 UI의 버튼으로 구현할 것인가?]
        
        // 함수
        private void Awake() // Awake()
        {
            Initialize();
            executeButton.onClick.AddListener(ExecuteCode); // 버튼의 클릭 이벤트를 등록합니다.
        }

        // 변수를 초기화하는 함수
        private void Initialize()
        {
            // _player = FindAnyObjectByType<PlayerCharacter>(); // 플레이어는 Find() 함수를 사용하여 찾습니다.
            _sourceCode = new(); // 소스 코드의 인스턴스를 생성하고, 초기화합니다.
            _sourceCode.Clear();
        }
        
        // 소스 코드를 실행하는 함수
        private void ExecuteCode(/*LinkedList<CodeBlock> sourceCode*/)
        {
            // 소스 코드를 순회하면서, 블록의 함수를 호출합니다.
            foreach (CodeBlockBase codeBlock in _sourceCode)
            {
                // codeBlock.Execute(_player);
            }
        }

        // 소스 코드에 코드 블록을 추가하는 함수
        public void AddCodeBlock(CodeBlockBase codeBlock)
        {
            _sourceCode.AddLast(codeBlock); // 소스 코드의 마지막에 코드 블록을 추가합니다.
            codeBlock.AddTouchListener(RemoveCodeBlock); // 코드 블록에 터치 이벤트로 제거 함수를 추가합니다.
        }

        // 소스 코드에서 코드 블록을 제거하는 함수
        private void RemoveCodeBlock(CodeBlockBase codeBlock)
        {
            codeBlock.RemoveTouchListener(RemoveCodeBlock); // 터치 이벤트를 제거합니다.
            _sourceCode.Remove(codeBlock); // 소스 코드에서 선택한 코드 블록을 제거합니다.
        }

        // 등록된 코드 블록의 UI 위치를 조정하는 함수
        private void UpdateUI(CodeBlockBase codeBlock)
        {
            for (LinkedListNode<CodeBlockBase> node = _sourceCode.Find(codeBlock); node != null; node = node.Next)
            {
                
            }
        }
    }
}
