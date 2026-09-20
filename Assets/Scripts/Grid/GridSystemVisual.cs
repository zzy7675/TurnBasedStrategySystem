using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance { get; private set; }

    [SerializeField] private GameObject _gridSytemVisualSinglePrefab;
    private GridSystemVisualSingle[,] _gridSystemVisualSingleArray;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one GridSystemVisual " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        _gridSystemVisualSingleArray = new GridSystemVisualSingle[
            LevelGrid.Instance.GetWidth(),
            LevelGrid.Instance.GetHeight()
        ];
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); ++x)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); ++z)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                GameObject gridSystemVisualSingle = Instantiate(_gridSytemVisualSinglePrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);
                _gridSystemVisualSingleArray[x, z] = gridSystemVisualSingle.GetComponent<GridSystemVisualSingle>();
            }
        }
    }

    private void Update()
    {
        UpdateGridVisual();
    }

    public void HideAllGridPosition()
    {
        foreach(GridSystemVisualSingle gridSystemVisualSingle in _gridSystemVisualSingleArray)
        {
            gridSystemVisualSingle.Hide();
        }
    }

    public void ShowGridPositionList(List<GridPosition> gridPositionList)
    {
        foreach (GridPosition gridPosition in gridPositionList)
        {
            _gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].Show();
        }
    }

    public void UpdateGridVisual()
    {
            GridSystemVisual.Instance.HideAllGridPosition();

            Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
            GridSystemVisual.Instance.ShowGridPositionList(selectedUnit.GetMoveAction().GetValidActionGridPositionList());
    }
}
