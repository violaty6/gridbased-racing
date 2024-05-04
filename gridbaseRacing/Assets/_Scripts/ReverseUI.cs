using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ReverseUI : MonoBehaviour
{
    [SerializeField] private GameObject GearOBJ;
    [SerializeField] private Image GearImage;
    [SerializeField] private Sprite[] GearSprites;
    [SerializeField] private Transform[] GearPoints;
    private void Start()
    {
        GameEvents.current.onReverseSwitch += GearChangeFeedback;
    }
    private void OnDestroy()
    {
        GameEvents.current.onReverseSwitch -= GearChangeFeedback;
    }
    private void GearChangeFeedback(int id, int isUp)
    {
        Debug.Log("buldm.mğ4");
        if (isUp == 1)
        {
            GearOBJ.transform.DOMove(GearPoints[0].position,0.25f).SetEase(Ease.OutExpo);
            GearImage.sprite = GearSprites[0];
        }
        else
        {
            GearOBJ.transform.DOMove(GearPoints[1].position,0.25f).SetEase(Ease.OutExpo);
            GearImage.sprite = GearSprites[1];
        }
        
    }

}
