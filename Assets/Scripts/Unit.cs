using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Animator _unitAnimator;
    private Vector3 _targetPosition;

    private void Update()
    {
        if (Vector3.Distance(transform.position, _targetPosition) > Mathf.Epsilon)
        {
            Vector3 moveDirection = (_targetPosition - transform.position).normalized;
            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            _unitAnimator.SetBool("IsWalking", true);
        } else
        {
            _unitAnimator.SetBool("IsWalking", false);
            
        }

        if (Input.GetMouseButtonDown(0))
        {
            Move(MouseWorld.GetPosition());
        }
    }

    private void Move(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }
}
