using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : MonoBehaviour
{
    private float totalSpinAmount;
    private bool _isActive;
    private void Update()
    {
        if (!_isActive)
        {
            return;
        }

        float spinAddAmount = 360f * Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAddAmount, 0);
        totalSpinAmount += spinAddAmount;
        if (totalSpinAmount >= 360f)
        {
            _isActive = false;
        }

    }
    public void Spin()
    {
        _isActive = true;
        totalSpinAmount = 0;
    }
}
