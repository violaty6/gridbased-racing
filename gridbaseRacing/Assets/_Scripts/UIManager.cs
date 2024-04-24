using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private bool isUnitCam = true;

    private void Awake()
    {
        isUnitCam = true;
    }
    public void CameraSwitchButton()
    {
        isUnitCam = !isUnitCam;
        GameEvents.current.onCameraSwitchPerformed(0,isUnitCam);
    }
}
