using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    [SerializeField] private LayerMask _mousePlaneLayerMask;
    private static MouseWorld instance;

    void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        transform.position = MouseWorld.GetPosition();
    }

    public static Vector3 GetPosition()
    {
        // cast mouse on-screen position into world position, and cast a ray from the camera to the mouse's world position
        Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseScreenPosition());

        // The collider hit by the raycast(only hit mouse plane)
        Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, instance._mousePlaneLayerMask);
        return raycastHit.point;
    }
}
