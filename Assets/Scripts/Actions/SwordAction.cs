using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAction : BaseAction
{
    public static event EventHandler OnAnySwordHit;
    public event EventHandler OnSwordActionStarted;
    public event EventHandler OnSwordActionCompleted;
    private enum State
    {
        SwingingSwordBeforeHit,
        SwingingSwordAfterHit
    };
    private int _maxSwordDistance = 1;
    private State _state;
    private float _stateTimer;
    private Unit _targetUnit;

    private void Update()
    {
        if (!_isActive)
        {
            return;
        }
        _stateTimer -= Time.deltaTime;
        switch (_state)
        {
            case State.SwingingSwordBeforeHit:
                Vector3 aimDirection = (_targetUnit.GetWorldPosition() - _unit.GetWorldPosition()).normalized;
                float rotationSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, aimDirection, rotationSpeed * Time.deltaTime);
                break;
            case State.SwingingSwordAfterHit:
                break;
        }
        if (_stateTimer <= 0f)
        {
            NextState();
        }

    }

    private void NextState()
    {
        switch (_state)
        {
            case State.SwingingSwordBeforeHit:
                _state = State.SwingingSwordAfterHit;
                float afterHitStateTime = .5f;
                _stateTimer = afterHitStateTime;
                _targetUnit.Damage(100);
                OnAnySwordHit?.Invoke(this, EventArgs.Empty);
                break;
            case State.SwingingSwordAfterHit:
                OnSwordActionCompleted?.Invoke(this, EventArgs.Empty);
                ActionComplete();
                break;
        }
    }

    public override string GetActionName()
    {
        return "Sword";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            _gridPosition = gridPosition,
            _actionValue = 200
        };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = _unit.GetGridPosition();
        for (int x = -_maxSwordDistance; x <= _maxSwordDistance; ++x)
        {
            for (int z = -_maxSwordDistance; z <= _maxSwordDistance; ++z)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    continue;
                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
                {
                    // Grid position already occupied with another unit
                    continue;
                }

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                
                if (targetUnit.IsEnemy() == _unit.IsEnemy())
                {
                    continue;
                }
                validGridPositionList.Add(testGridPosition);

            }
        }
        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        _targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        _state = State.SwingingSwordBeforeHit;
        float beforeHitStateTime = .7f;
        _stateTimer = beforeHitStateTime;
        OnSwordActionStarted?.Invoke(this, EventArgs.Empty);
        ActionStart(onActionComplete);
    }

    public int GetMaxSwordDistance()
    {
        return _maxSwordDistance;
    }
}
