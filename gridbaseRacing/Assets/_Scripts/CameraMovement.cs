using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinemachineCam;
    [SerializeField] private float _speed= 1f;
    [SerializeField] private float _smoothing = 5f;
    [SerializeField] private Vector2 _range = new Vector2(100, 100);

    [SerializeField] private Transform Center;
    private UnitControls generalActions;
    public Vector3 _targetPosition;
    private Vector3 _input;
    private GameObject unit;
    void Start()
    {
        unit = cinemachineCam.m_Follow.gameObject;
        _targetPosition = transform.position; 
        generalActions = new UnitControls();
        GameEvents.current.onCameraSwitch += SwitchCamera;
    }

    private void SwitchCamera(int id,bool isUnitCam)
    {
        if (isUnitCam) // Unitden free
        {
            generalActions.GeneralKeys.CameraMovement.Disable();
            cinemachineCam.m_Follow = unit.transform;
        }
        else // Free den UNIT
        {
            generalActions.GeneralKeys.CameraMovement.Enable();
            cinemachineCam.m_Follow = Center;
        }
    }
    void Update()
    {
        HandleMovementInput();
    }
    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector3 newTargetPosition = _targetPosition + _input * _speed;
        if (IsInBounds(newTargetPosition)) _targetPosition = newTargetPosition;
        transform.position = Vector3.Lerp(transform.position, _targetPosition, Time.deltaTime * _smoothing);
    }

    bool IsInBounds(Vector3 position)
    {
        return position.x > -_range.x &&
               position.x < _range.x &&
               position.z > -_range.y &&
               position.z < _range.y;
    }
    void HandleMovementInput()
    {
        float x = generalActions.GeneralKeys.CameraMovement.ReadValue<Vector2>().x;
        float z = generalActions.GeneralKeys.CameraMovement.ReadValue<Vector2>().y;
        // Debug.Log(x+" "+ z);

        Vector3 right = transform.right * x;
        Vector3 forward = transform.forward * z;
        _input = (forward + right).normalized;
    }
    private void OnDisable()
    {
        generalActions.GeneralKeys.CameraMovement.Disable();
        GameEvents.current.onCameraSwitch -= SwitchCamera;
    }
}
