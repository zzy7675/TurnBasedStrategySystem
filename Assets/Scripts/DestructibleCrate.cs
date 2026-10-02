using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DestructibleCrate : MonoBehaviour
{
    public static event EventHandler OnAnyDestroyed;

    [SerializeField] private GameObject _crateDestroyedPrefab;
    private GridPosition _gridPosistion;

    private void Start()
    {
        _gridPosistion = LevelGrid.Instance.GetGridPosition(transform.position);
    }

    public GridPosition GetGridPosition()
    {
        return _gridPosistion;
    }

    public void Damage()
    {
        GameObject crateDestroyedObject = Instantiate(_crateDestroyedPrefab, transform.position, transform.rotation);

        ApplyExplosionToChildren(crateDestroyedObject.transform, 100f, transform.position, 10f);
        Destroy(gameObject);

        OnAnyDestroyed?.Invoke(this, EventArgs.Empty);
    }

    private void ApplyExplosionToChildren(Transform root, float explosionForce, Vector3 explosionPosition, float explosionRange)
    {
        foreach (Transform child in root)
        {
            if(child.TryGetComponent<Rigidbody>(out Rigidbody childRigidBody))
            {
                childRigidBody.AddExplosionForce(explosionForce, explosionPosition, explosionForce);
            }
            ApplyExplosionToChildren(child, explosionForce, explosionPosition, explosionForce);
        }
    }
}
