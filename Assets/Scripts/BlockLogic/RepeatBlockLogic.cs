using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBlockLogic : BlockLogicBase
{
    private List<BlockLogicBase> _elementLogics;
    private int _repeatCount;
    private int _currentRepeatCount;
    private int _currentLogicCount;
    private bool _isChecked;

    //FunctionBlock이 작동되는 시작조건을 충족하지 않을 경우 (아마 바로 true 반환하면 될듯?)
    public override ErrorType IsExecutable(StageManager owner)
    {
        _currentRepeatCount = 0;
        _currentLogicCount = 0;
        _isChecked = false;
        return ErrorType.NoError;
    }

    public override LogicState Execute(StageManager owner)
    {
        while (_currentRepeatCount < _repeatCount)
        {
            if (_isChecked == false)
            {
                if (_elementLogics == null || _elementLogics.Count == 0)
                    return LogicState.Failure;
                if (_elementLogics[_currentLogicCount].IsExecutable(owner) != ErrorType.NoError)
                    return LogicState.Failure;
                _isChecked = true;
            }

            switch (_elementLogics[_currentLogicCount].Execute(owner))
            {
                case LogicState.Success:
                    if (_currentLogicCount == _elementLogics.Count)
                        return LogicState.Success;
                    else
                    {
                        _isChecked = false;
                        _currentLogicCount++;
                        return LogicState.Running;
                    }
                case LogicState.Running:
                    return LogicState.Running;
                default:
                    return LogicState.Failure;
            }
        }
        return LogicState.Failure;
    }
}
