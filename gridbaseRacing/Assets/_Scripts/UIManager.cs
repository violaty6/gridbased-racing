using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private bool isUnitCam = true;
    [SerializeField] private TextMeshProUGUI levelText;

    private void Awake()
    {
        levelText.text = "LEVEL "+(SceneManager.GetActiveScene().buildIndex+1).ToString();
        isUnitCam = true;
    }
    public void CameraSwitchButton()
    {
        isUnitCam = !isUnitCam;
        GameEvents.current.onCameraSwitchPerformed(0,isUnitCam);
    }
}
