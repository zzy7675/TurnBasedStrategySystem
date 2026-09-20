using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }
    public event EventHandler OnSelectedUnitChanged;
    [SerializeField] private Unit _selectedUnit;
    [SerializeField] private LayerMask _unitLayerMask;

    private bool _isBusy;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one UnitActionSystem " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        if (_isBusy)
        {
            return;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            if (TryHandleUnitSelection())
                return;
            
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            if (_selectedUnit.GetMoveAction().IsValidActionGridPosition(mouseGridPosition))
            {
                SetBusy();
                _selectedUnit.GetMoveAction().Move(mouseGridPosition, ClearBusy);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            SetBusy();
            _selectedUnit.GetSpinAction().Spin(ClearBusy);
        }
    }

    private void SetBusy()
    {
        _isBusy = true;
    }

    private void ClearBusy()
    {
        _isBusy = false;
    }
    private bool TryHandleUnitSelection()
    {
        // cast mouse on-screen position into world position, and cast a ray from the camera to the mouse's world position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // The collider hit by the raycast(only hit mouse plane)
        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, _unitLayerMask))
        {
            if (raycastHit.transform.TryGetComponent<Unit>(out Unit unitClicked))
            {
                SetSelectedUnit(unitClicked);
                return true;
            }
        }
        return false;
    }
    private void SetSelectedUnit(Unit unit)
    {
        _selectedUnit = unit;
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return _selectedUnit;
    }
}
