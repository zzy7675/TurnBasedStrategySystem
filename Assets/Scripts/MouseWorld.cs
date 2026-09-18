using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    private void Update()
    {
        // Input.mousePosition

        // 鼠标的屏幕位置转换成Scene里的位置，并从Camera向鼠标Scene里的位置射出射线
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray);
    }
}
