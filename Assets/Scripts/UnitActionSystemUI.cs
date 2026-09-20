using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private GameObject actionButtonPrefab;
    [SerializeField] private GameObject actionButtonContainer;

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        CreateUnitActionButtons();
    }
    private void CreateUnitActionButtons()
    {
        foreach (Transform buttonTransform in actionButtonContainer.transform)
        {
            Destroy(buttonTransform.gameObject);
        }
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray())
        {
            GameObject actionButton = Instantiate(actionButtonPrefab, actionButtonContainer.transform);
            ActionButtonUI actionButtonUI = actionButton.GetComponent<ActionButtonUI>();

            actionButtonUI.SetBaseAction(baseAction);
        }
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        CreateUnitActionButtons();
    }
}
