using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GeneralControls : MonoBehaviour
{
    private UnitControls _unitControls;
    private void Awake()
    {
        _unitControls = new UnitControls();
    }
    private void OnEnable()
    {
        _unitControls.Enable();
        _unitControls.GeneralKeys.Restart.performed += Restart;
    }
    private void OnDisable()
    {
        _unitControls.Disable();
        _unitControls.GeneralKeys.Restart.performed -= Restart;
    }

    private void Restart( InputAction.CallbackContext ctx)
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}
