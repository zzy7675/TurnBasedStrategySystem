using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridSystemHex<TGridObject>
{

    private const float HEX_VERTICAL_OFFSET_MULTIPLIER = .75f;
    private int _width;
    private int _height;
    private float _cellSize;
    private TGridObject[,] _gridObjectArray;
    public GridSystemHex(int width, int height, float cellSize, Func<GridSystemHex<TGridObject>, GridPosition, TGridObject> createGridObject)
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;

        _gridObjectArray = new TGridObject[width, height];
        for (int x = 0; x < width; ++x)
        {
            for (int z = 0; z < height; ++z)
            {
                // Debug.DrawLine(GetWorldPosition(x, z), GetWorldPosition(x, z) + Vector3.right * .2f, Color.white, 1000);
                GridPosition gridPosition = new GridPosition(x, z);
                _gridObjectArray[x, z] = createGridObject(this, gridPosition);
            }
        }
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        if (gridPosition.z % 2 == 1)
        {
            return new Vector3(gridPosition.x, 0, 0) * _cellSize + new Vector3(0, 0, gridPosition.z) * _cellSize * HEX_VERTICAL_OFFSET_MULTIPLIER + new Vector3(1, 0, 0) * _cellSize * .5f;
        } else
        {
            return new Vector3(gridPosition.x, 0, 0) * _cellSize + new Vector3(0, 0, gridPosition.z) * _cellSize * HEX_VERTICAL_OFFSET_MULTIPLIER;
        }
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        GridPosition roughXZ = new GridPosition(Mathf.RoundToInt(worldPosition.x / _cellSize), Mathf.RoundToInt(worldPosition.z / _cellSize / HEX_VERTICAL_OFFSET_MULTIPLIER));

        bool oddRow = (roughXZ.z % 2 == 1);
        List<GridPosition> neighborGridPositionList = new List<GridPosition>
        {
            roughXZ + new GridPosition(-1, 0),
            roughXZ + new GridPosition(+1, 0),
            roughXZ + new GridPosition(0, +1),
            roughXZ + new GridPosition(0, -1),
            roughXZ + new GridPosition(oddRow ? +1 : -1, +1),
            roughXZ + new GridPosition(oddRow ? +1 : -1, -1),
        };

        GridPosition closestGridPosition = roughXZ;
        foreach(GridPosition neighborGridPosition in neighborGridPositionList)
        {
            if (Vector3.Distance(worldPosition, GetWorldPosition(neighborGridPosition)) < Vector3.Distance(worldPosition, GetWorldPosition(closestGridPosition)))
            {
                closestGridPosition = neighborGridPosition;
            }
        }
        return closestGridPosition;
    }

    public void CreateDebugObjects(GameObject debugPrefab)
    {
        for (int x = 0; x < _width; ++x)
        {
            for (int z = 0; z < _height; ++z)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                GameObject debugObject = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                GridDebugObject gridDebugObject = debugObject.GetComponent<GridDebugObject>();
                gridDebugObject.SetGridObject(GetGridObject(gridPosition));
            }
        }
    }

    public TGridObject GetGridObject(GridPosition gridPosition)
    {
        return _gridObjectArray[gridPosition.x, gridPosition.z];
    }

    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridPosition.x >= 0 &&
               gridPosition.z >= 0 &&
               gridPosition.x < _width &&
               gridPosition.z < _height;
    }

    public int GetWidth()
    {
        return _width;
    }

    public int GetHeight()
    {
        return _height;
    }

}
