using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitController : MonoBehaviour
{
    [SerializeField]private bool isCrashed = false;
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
        Vector3Int targetGrid = new Vector3Int(0,0,0);
        List<Node> checkNodes = new List<Node>();
        List<Node.NodeType> checkNodeType = new List<Node.NodeType>();
        for (int i = 1; i <= _UnitEnginePower; i++)
        {
            if (input.x != 0 && input.y == 0)
            {
                moveDirection.x = input.x *i ;
            }
            else if (input.y != 0 && input.x == 0)
            {
                moveDirection.z = input.y *i ;
            }
            Vector3Int moveDirectionInt = new Vector3Int(Mathf.RoundToInt(moveDirection.x), 0, Mathf.RoundToInt(moveDirection.z));
            Vector3Int targetCord = currentNode.cords + moveDirectionInt;
            Node targetNode = _gridManager.GetTileAt(targetCord);
            targetGrid = targetCord;
            CheckNode(targetNode,checkNodes, checkNodeType);
        }
        CheckAndMove(checkNodes, checkNodeType,targetGrid);
    }
    private void CheckNode(Node targetNode,List<Node> checkNodes,List<Node.NodeType> checkNodeType)
    {
        checkNodes.Add(targetNode);
        if (targetNode == null)
        {
            checkNodeType.Add(Node.NodeType.Void);
        }
        else
        {
            checkNodeType.Add(targetNode.currentType);
        }
    }

    private void CheckAndMove(List<Node> checkNodes,List<Node.NodeType> checkNodeType,Vector3Int targetGrid)
    {
        if (!checkNodes.Contains(null) && !checkNodeType.Contains(Node.NodeType.Obstacle) && !checkNodeType.Contains(Node.NodeType.Void))
        {
            MoveFeedBack(checkNodes.Last());
            currentNode = checkNodes.Last();
        }
        else
        {
            isCrashed = true;
        }
    }
    void CrashFeedback(Vector3Int crashGridCords)
    {
        // transform.DOMove(targetNode.cords, 1.25f).SetEase(Ease.OutQuart);
        // transform.DOLookAt(targetNode.cords, 0.1f);
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