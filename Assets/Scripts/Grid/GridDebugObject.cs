using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridDebugObject : MonoBehaviour
{
    [SerializeField] TextMeshPro _textMeshPro;
    private GridObject _gridObject;

    public void SetGridObject(GridObject gridObject)
    {
        _gridObject = gridObject;
    }
    private void Update()
    {
        _textMeshPro.text = _gridObject.ToString();
    }
}
