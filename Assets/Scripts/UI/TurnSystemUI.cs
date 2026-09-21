using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _turnNumberText;
    [SerializeField] private Button _endTurnButton;
    [SerializeField] private GameObject _enemyTurnVisualGameObject;


    private void Start()
    {
        _endTurnButton.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });

        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

        UpdateTurnText();
        UpdateEnemeyTurnVisual();
        UpdateEndTurnButtonVisibility();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateTurnText();
        UpdateEnemeyTurnVisual();
        UpdateEndTurnButtonVisibility();
    }

    private void UpdateTurnText()
    {
        _turnNumberText.text = "TURN " + TurnSystem.Instance.GetTurn();
    }

    private void UpdateEnemeyTurnVisual()
    {
        _enemyTurnVisualGameObject.SetActive(!TurnSystem.Instance.IsPlayerTurn());
    }

    private void UpdateEndTurnButtonVisibility()
    {
        _endTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
    }

}
