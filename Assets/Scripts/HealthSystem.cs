using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDead;
    public event EventHandler OnDamaged;
    [SerializeField] private int _health = 100;
    private int _healthMax;

    private void Awake()
    {
        _healthMax = _health;
    }

    public void Damage(int damageAmount)
    {
        _health -= damageAmount;

        if (_health < 0)
        {
            _health = 0;
        }
        OnDamaged?.Invoke(this, EventArgs.Empty);
        if (_health == 0)
        {
            Die();
        }

    }

    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormalized()
    {
        return (float)_health / _healthMax;
    }
}
