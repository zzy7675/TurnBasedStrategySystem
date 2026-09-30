using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    public static event EventHandler OnAnyGrenadeExploded;

    [SerializeField] private GameObject _grenadeExplodeVfxPrefab;
    [SerializeField] private TrailRenderer _trailRenderer;
    [SerializeField] private AnimationCurve _arcYAnimationCurve;
    private float _totalDistance;
    private Vector3 _positionXZ;

    private Vector3 _targetPosition;
    private Action _onGrenadeBehaviorComplete;

    private void Update()
    {
        Vector3 moveDirection = (_targetPosition - _positionXZ).normalized;
        float moveSpeed = 15f;
        _positionXZ += moveDirection * moveSpeed * Time.deltaTime;

        float distance = Vector3.Distance(_positionXZ, _targetPosition);
        float distanceNormalized = 1 - distance / _totalDistance;

        float maxHeight = _totalDistance / 4f;
        float positionY = _arcYAnimationCurve.Evaluate(distanceNormalized) * maxHeight;
        transform.position = new Vector3(_positionXZ.x, positionY, _positionXZ.z);
        float reachedTargetDistance = .2f;
        if (Vector3.Distance(_positionXZ, _targetPosition) < reachedTargetDistance)
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
            OnAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);

            _trailRenderer.transform.parent = null;
            Instantiate(_grenadeExplodeVfxPrefab, _targetPosition + Vector3.up * 1f, Quaternion.identity);
            Destroy(gameObject);
            _onGrenadeBehaviorComplete();
        }
    }
    public void Setup(GridPosition targetGridPosition, Action onGrenadeBehaviorComplete)
    {
        _onGrenadeBehaviorComplete = onGrenadeBehaviorComplete;
        _targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);

        _positionXZ = transform.position;
        _positionXZ.y = 0;
        _totalDistance = Vector3.Distance(_positionXZ, _targetPosition);
    }

}
