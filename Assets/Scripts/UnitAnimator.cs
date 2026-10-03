using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _bulletProjectilePrefab;
    [SerializeField] private GameObject _shootPoint;
    [SerializeField] private GameObject _riflePrefab;
    [SerializeField] private GameObject _swordPrefab;

    private void Awake()
    {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }
        if (TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.OnShoot += ShootAction_OnShoot;
        }

        if (TryGetComponent<SwordAction>(out SwordAction swordAction))
        {
            swordAction.OnSwordActionStarted += SwordAction_OnSwordActionStarted; 
            swordAction.OnSwordActionCompleted += SwordAction_OnSwordActionCompleted; 
        }
    }

    private void Start()
    {
        EquipRifle();
    }
    private void SwordAction_OnSwordActionStarted(object sender, EventArgs e)
    {
        EquipSword();
        _animator.SetTrigger("SwordSlash");
    }

    private void SwordAction_OnSwordActionCompleted(object sender, EventArgs e)
    {
        EquipRifle();
    }


    private void ShootAction_OnShoot(object sender, ShootAction.ShootEventArgs e)
    {
        _animator.SetTrigger("Shoot");
        GameObject bulletProjectileGameObject = Instantiate(_bulletProjectilePrefab, _shootPoint.transform.position, Quaternion.identity);
        BulletProjectile bulletProjectile = bulletProjectileGameObject.GetComponent<BulletProjectile>();

        Vector3 targetUnitShootAtPosition = e.targetUnit.GetWorldPosition();
        targetUnitShootAtPosition.y = _shootPoint.transform.position.y;
        bulletProjectile.Setup(targetUnitShootAtPosition);
    }

    private void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        _animator.SetBool("IsWalking", true);
    }

    private void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        _animator.SetBool("IsWalking", false);
    }

    private void EquipSword()
    {
        _swordPrefab.SetActive(true);
        _riflePrefab.SetActive(false);
    }

    private void EquipRifle()
    {
        _swordPrefab.SetActive(false);
        _riflePrefab.SetActive(true);
    }
}
