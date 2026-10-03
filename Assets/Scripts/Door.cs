using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private bool _isOpen;
    private GridPosition _gridPosition;
    private Animator _animator;
    private Action _onInteractionComplete;
    private bool _isActive;
    private float _timer;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.SetInteractableAtGridPosition(_gridPosition, this);

        if (_isOpen)
        {
            OpenDoor();
        } else
        {
            CloseDoor();
        }
    }

    private void Update()
    {
        if (!_isActive)
        {
            return;
        }
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            _isActive = false;
            _onInteractionComplete();
        }
    }
    public void Interact(Action onInteractionComplete)
    {
        _onInteractionComplete = onInteractionComplete;
        _isActive = true;
        _timer = 0.5f;
        if (_isOpen)
        {
            CloseDoor();
        } else
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        _isOpen = true;
        _animator.SetBool("IsOpen", _isOpen);
        Pathfinding.Instance.SetIsWalkableGridPosition(_gridPosition, true);
    }

    private void CloseDoor()
    {
        _isOpen = false;
        _animator.SetBool("IsOpen", _isOpen);
        Pathfinding.Instance.SetIsWalkableGridPosition(_gridPosition, false);
    } 
}
