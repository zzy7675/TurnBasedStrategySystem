using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    private float _totalSpinAmount;

    private void Update()
    {
        if (!_isActive)
        {
            return;
        }

        float spinAddAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAddAmount, 0);
        _totalSpinAmount += spinAddAmount;
        if (_totalSpinAmount >= 360f)
        {
            _isActive = false;
            onActionComplete();
        }

    }
    public void Spin(Action onActionComplete)
    {
        this.onActionComplete = onActionComplete;
        _isActive = true;
        _totalSpinAmount = 0;
    }
    public override string GetActionName()
    {
        return "Spin";
    }
}
