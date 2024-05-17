using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private bool isUnitCam = true;
    [SerializeField] private TextMeshProUGUI levelText;

    [SerializeField]private List<Image> Uis;
    [SerializeField]private List<TextMeshProUGUI> UItexts;

    private void Awake()
    {
        levelText.text = SceneManager.GetActiveScene().name;
        isUnitCam = true;
        UIFadeStart();
    }
    void UIFadeStart()
    {
        foreach (var image_ui in Uis)
        {
            image_ui.DOColor(new Color(1, 1, 1, 0f), 0f);
        }
        DOVirtual.DelayedCall(1.25f, () =>
        {
            foreach (var image_ui in Uis)
            {
                image_ui.DOColor(new Color(1, 1, 1, 1f), 0.45f);
            }
        });
        
        foreach (var image_ui in UItexts)
        {
            image_ui.DOColor(new Color(1, 1, 1, 0f), 0f);
        }
        DOVirtual.DelayedCall(1.25f, () =>
        {
            foreach (var image_ui in UItexts)
            {
                image_ui.DOColor(new Color(1, 1, 1, 1f), 0.45f);
            }
        });
    }
    private void Start()
    {
        GameEvents.current.onLevelComplete += UIFadeIn;
    }

    void UIFadeIn(int id)
    {
        foreach (var image_ui in Uis)
        {
            image_ui.DOColor(new Color(1, 1, 1, 0f), 0.4f);
        }
        foreach (var image_ui in UItexts)
        {
            image_ui.DOColor(new Color(1, 1, 1, 0f), 0.4f);
        }
    }
    public void CameraSwitchButton()
    {
        isUnitCam = !isUnitCam;
        GameEvents.current.onCameraSwitchPerformed(0,isUnitCam);
    }
}
