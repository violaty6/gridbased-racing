using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitController : MonoBehaviour
{
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private Node currentNode;
    [SerializeField] private int _UnitEnginePower;
    private UnitControls _unitControls;


    private void Awake()
    {
        _unitControls = new UnitControls();
        _gridManager = FindObjectOfType<GridManager>();
    }

    Vector3Int GetCords()
    {
        Vector3Int result = new Vector3Int(0, 0, 0);
        result.x = Mathf.RoundToInt(transform.position.x);
        result.y = Mathf.RoundToInt(transform.position.z);
        result.z = Mathf.RoundToInt(transform.position.y);
        return result;
    }

    private void Start()
    {
        currentNode = _gridManager.GetTileAt(GetCords());
    }

    private void OnEnable()
    {
        _unitControls.Enable();
        _unitControls.BasicMovement.Move.performed += Move;
    }

    private void OnDisable()
    {
        _unitControls.Disable();
        _unitControls.BasicMovement.Move.performed -= Move;
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        Vector2 input = ctx.ReadValue<Vector2>();
        Vector3 moveDirection = Vector3.zero;
        if (input.x != 0 && input.y == 0)
        {
            moveDirection.x = input.x * _UnitEnginePower;
        }
        else if (input.y != 0 && input.x == 0)
        {
            moveDirection.z = input.y * _UnitEnginePower;
        }
        Vector3Int moveDirectionInt = new Vector3Int(Mathf.RoundToInt(moveDirection.x), 0, Mathf.RoundToInt(moveDirection.z));
        Vector3Int targetCord = currentNode.cords + moveDirectionInt;
        Node targetNode = _gridManager.GetTileAt(targetCord);
        if (targetNode != null)
        {
            MoveFeedBack(targetNode);
            currentNode = targetNode;
        }
    }

    void CheckBlocks(Vector3Int start, Vector3Int end)
    {
        // for (int i = 0; i < UPPER; i++)
        // {
        //                 
        // }
    }
    
    void MoveFeedBack(Node targetNode)
    {
        transform.DOMove(targetNode.cords, 1.25f).SetEase(Ease.OutQuart);
        transform.DOLookAt(targetNode.cords, 0.1f);
    }

    private void SetNode(Node targetNode)
    {
        currentNode = targetNode;
    }
}