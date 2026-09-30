using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    private Vector3 _targetPosition;
    private Action _onGrenadeBehaviorComplete;

    private void Update()
    {
        Vector3 moveDirection = (_targetPosition - transform.position).normalized;
        float moveSpeed = 15f;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        float reachedTargetDistance = .2f;
        if (Vector3.Distance(transform.position, _targetPosition) < reachedTargetDistance)
        {
            float damageRadius = 4f;
            Collider[] colliderArray = Physics.OverlapSphere(_targetPosition, damageRadius);

            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent<Unit>(out Unit targetUnit))
                {
                    targetUnit.Damage(30);
                }
            }
            Destroy(gameObject);
            _onGrenadeBehaviorComplete();
        }
    }
    public void Setup(GridPosition targetGridPosition, Action onGrenadeBehaviorComplete)
    {
        _onGrenadeBehaviorComplete = onGrenadeBehaviorComplete;
        _targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
    }

}
