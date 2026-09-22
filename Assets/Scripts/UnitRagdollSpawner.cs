using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRagdollSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _unitRagdollPrefab;
    [SerializeField] private Transform _originalRagdollRootBone;
    private HealthSystem _healthSystem;

    private void Awake()
    {
        _healthSystem = GetComponent<HealthSystem>();

        _healthSystem.OnDead += HealthSystem_OnDead;
    }

    private void HealthSystem_OnDead(object sender, EventArgs e)
    {
        GameObject unitRagdollGameObject = Instantiate(_unitRagdollPrefab, transform.position, Quaternion.identity);
        UnitRagdoll unitRagdoll = unitRagdollGameObject.GetComponent<UnitRagdoll>();
        unitRagdoll.Setup(_originalRagdollRootBone);
    }
}
