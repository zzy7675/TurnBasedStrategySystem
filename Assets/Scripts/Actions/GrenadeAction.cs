using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAction : BaseAction
{

    [SerializeField] private GameObject _grenadeProjectilePrefab;
    private int _maxThrowDistance = 7;

    private void Update()
    {
        if (!_isActive)
        {
            return;
        }
        ActionComplete();
    }

    public override string GetActionName()
    {
        return "Grenade";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction
        {
            _gridPosition = gridPosition,
            _actionValue = 0,
        };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = _unit.GetGridPosition();
        for (int x = -_maxThrowDistance; x <= _maxThrowDistance; ++x)
        {
            for (int z = -_maxThrowDistance; z <= _maxThrowDistance; ++z)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    continue;
                

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);

                if (testDistance > _maxThrowDistance)
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
        GameObject grenadeProjectileObject = Instantiate(_grenadeProjectilePrefab, _unit.GetWorldPosition(), Quaternion.identity);
        GrenadeProjectile grenadeProjectile = grenadeProjectileObject.GetComponent<GrenadeProjectile>();
        grenadeProjectile.Setup(gridPosition, OnGrenadeBehaviorComplete);
        ActionStart(onActionComplete);
    }
    private void OnGrenadeBehaviorComplete()
    {
        ActionComplete();
    }
}
