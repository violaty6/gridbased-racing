using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class GasolineUI : MonoBehaviour
{
    [SerializeField] private Image fillSprite;
    [SerializeField] private TextMeshProUGUI gasolineIntText;
    [SerializeField]private float dividedMove;
    public int gasolineInt;
    private bool isGasolineOut = false;
    private float currentFloat = 1f;
    

    private void Start()
    {
        DivideToList();
        GameEvents.current.onMove +=useGasoline;
    }

    private void useGasoline( int id)
    {
        if (isGasolineOut)return;
        gasolineInt--;
        currentFloat= currentFloat - dividedMove;
        fillSprite.DOFillAmount(currentFloat, 0.6f).SetEase(Ease.OutQuart);
        gasolineIntText.text = gasolineInt.ToString()+" L";
        if (gasolineInt == 0) isGasolineOut = true;
    }

    private void DivideToList()
    {
        dividedMove = 1f / gasolineInt;
        gasolineIntText.text = gasolineInt.ToString()+" L";
    }
}
