using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    [SerializeField] private Animator _unitAnimator;
    [SerializeField] private int _maxMoveDistance = 4;
    private Vector3 _targetPosition;
    private Unit _unit;
    
    private void Awake()
    {
        _unit = GetComponent<Unit>();
        _targetPosition = transform.position;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, _targetPosition) > 0.1f)
        {
            Vector3 moveDirection = (_targetPosition - transform.position).normalized;
            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            float rotateSpeed = 10f;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
            _unitAnimator.SetBool("IsWalking", true);
        } else
        {
            _unitAnimator.SetBool("IsWalking", false);
        }
    }
    public void Move(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }

    public List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = _unit.GetGridPosition();
        for (int x = -_maxMoveDistance; x <= _maxMoveDistance; ++x)
        {
            for (int z = -_maxMoveDistance; z <= _maxMoveDistance; ++z)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                Debug.Log(testGridPosition);
            }
        }
        return validGridPositionList;
    }
}
