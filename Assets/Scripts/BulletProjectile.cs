using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private TrailRenderer _trailRenderer;
    [SerializeField] private GameObject _bulletHitVFXPrefab;
    private Vector3 _targetPosition;
    public void Setup(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }

    private void Update()
    {
        Vector3 moveDirection = (_targetPosition - transform.position).normalized;

        float distanceBeforeMoving = Vector3.Distance(_targetPosition, transform.position);
        float moveSpeed = 200f;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        float distanceAfterMoving = Vector3.Distance(_targetPosition, transform.position);

        if (distanceBeforeMoving < distanceAfterMoving)
        {
            transform.position = _targetPosition;
            _trailRenderer.transform.parent = null;
            Destroy(gameObject);
            Instantiate(_bulletHitVFXPrefab, _targetPosition, Quaternion.identity);
        }
    }
}
