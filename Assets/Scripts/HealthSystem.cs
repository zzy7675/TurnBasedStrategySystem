using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDead;
    [SerializeField] private int _health = 100;

    public void Damage(int damageAmount)
    {
        _health -= damageAmount;

        if (_health < 0)
        {
            _health = 0;
        }

        if (_health == 0)
        {
            Die();
        }

    }

    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }
}
