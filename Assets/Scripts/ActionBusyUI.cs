using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBusyUI : MonoBehaviour
{
    [SerializeField] private GameObject _actionBusyUI;

    private void Start()
    {
        UnitActionSystem.Instance.OnBusyChanged += UnitActionSystem_OnBusyChanged;
        Hide();
    }

    private void Show()
    {
        _actionBusyUI.SetActive(true);
    }

    private void Hide()
    {
        _actionBusyUI.SetActive(false);
    }

    private void UnitActionSystem_OnBusyChanged(object sender, bool isBusy)
    {
        if (isBusy)
        {
            Show();
        } else
        {
            Hide();
        }
    }
}
