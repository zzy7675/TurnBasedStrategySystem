using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }
    private List<Unit> _unitList;
    private List<Unit> _friendlyUnitList;
    private List<Unit> _enemyUnitList;

    private void Awake()
    {
        _unitList = new List<Unit>();
        _friendlyUnitList = new List<Unit>();
        _enemyUnitList = new List<Unit>();
        if (Instance != null)
        {
            Debug.LogError("There's more than one UnitManager " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
    }

    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;
        if (unit.IsEnemy())
        {
            _enemyUnitList.Add(unit);
        } else
        {
            _friendlyUnitList.Add(unit);
        }
        _unitList.Add(unit);
    }
    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;
        if (unit.IsEnemy())
        {
            _enemyUnitList.Remove(unit);
        } else
        {
            _friendlyUnitList.Remove(unit);
        }
        _unitList.Remove(unit);
    }

    public List<Unit> GetUnitList()
    {
        return _unitList;
    }

    public List<Unit> GetFriendlyUnitList()
    {
        return _friendlyUnitList;
    }

    public List<Unit> GetEnemyUnitList()
    {
        return _enemyUnitList;
    }


}
