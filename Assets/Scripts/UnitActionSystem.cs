using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }
    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler OnSelectedActionChanged;
    public event EventHandler<bool> OnBusyChanged;
    public event EventHandler OnActionStarted;
    [SerializeField] private Unit _selectedUnit;
    [SerializeField] private LayerMask _unitLayerMask;
    private BaseAction _selectedAction;

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

    private void Start()
    {
        SetSelectedUnit(_selectedUnit);

    }

    private void Update()
    {
        if (_isBusy)
        {
            return;
        }

        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }
        
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (TryHandleUnitSelection())
        {
            return; 
        }            

        HandleSelectedAction();
    }
    private void HandleSelectedAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            if (!_selectedAction.IsValidActionGridPosition(mouseGridPosition))
            {
                return;
            }
            if (!_selectedUnit.TrySpendActionPointsToTakeAction(_selectedAction))
            {
                return;
            }
            SetBusy();
            
            _selectedAction.TakeAction(mouseGridPosition, ClearBusy);
            OnActionStarted?.Invoke(this, EventArgs.Empty);
        }
    }
    private void SetBusy()
    {
        _isBusy = true;
        OnBusyChanged?.Invoke(this, _isBusy);
    }

    private void ClearBusy()
    {
        _isBusy = false;
        OnBusyChanged?.Invoke(this, _isBusy);
    }
    private bool TryHandleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // cast mouse on-screen position into world position, and cast a ray from the camera to the mouse's world position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // The collider hit by the raycast(only hit mouse plane)
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, _unitLayerMask))
            {
                if (raycastHit.transform.TryGetComponent<Unit>(out Unit unitClicked))
                {
                    if (unitClicked == _selectedUnit)
                    {
                        return false;
                    }

                    if (unitClicked.IsEnemy())
                    {
                        return false;
                    }
                    SetSelectedUnit(unitClicked);
                    return true;
                }
            }            
        }
        return false;
    }
    private void SetSelectedUnit(Unit unit)
    {
        _selectedUnit = unit;
        SetSelectedAction(unit.GetAction<MoveAction>());
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetSelectedAction(BaseAction baseAction)
    {
        _selectedAction = baseAction;
        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetSelectedUnit()
    {
        return _selectedUnit;
    }

    public BaseAction GetSelectedAction()
    {
        return _selectedAction;
    }
}
