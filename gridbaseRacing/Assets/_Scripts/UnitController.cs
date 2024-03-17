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
    [SerializeField] private List<GameObject> _wheels;
    [SerializeField] private GameObject _top;
    
    [SerializeField] private bool isCrashed = false;
    [SerializeField] private bool isMoving = false;
    
    [SerializeField] private bool canTurbo = true;
    [SerializeField] private bool isTurbo = false;
    
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private Node currentNode;
    [SerializeField] private int _UnitEnginePower;
    private UnitControls _unitControls;
    
    
    
    private void Awake()
    {
        _unitControls = new UnitControls();
        _gridManager = FindObjectOfType<GridManager>();
    }
    private void Start()
    {
        currentNode = _gridManager.GetTileAt(Direction.GetCords(transform.position));
    }
    private void OnEnable()
    {
        _unitControls.Enable();
        _unitControls.BasicMovement.Move.performed += Move;
        _unitControls.BasicMovement.Turbo.performed += CheckTurbo;
    }
    private void OnCrash()
    {
        _unitControls.Disable();
        _unitControls.BasicMovement.Move.performed -= Move;
        _unitControls.BasicMovement.Turbo.performed -= CheckTurbo;
    }
    private void Move(InputAction.CallbackContext ctx)
    {
        if (isMoving)return;
        isMoving = true;
        Vector2 input = ctx.ReadValue<Vector2>();
        if (isTurbo)
        {
            Turbo(input);
            isTurbo = false;
            return;
        }
        Vector3 moveDirection = Vector3.zero;
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
            CheckNode(targetNode,checkNodes, checkNodeType);
        }
        CheckAndMove(checkNodes, checkNodeType);
    }
    private void CheckTurbo(InputAction.CallbackContext ctx)
    {
        if (canTurbo)
        {
            isTurbo = true;
            canTurbo = false;
        }
    }
    private void Turbo(Vector2 input)
    {
        isMoving = true;
        Vector3 moveDirection = Vector3.zero;
        if (input.x != 0 && input.y == 0)
        {
            moveDirection.x = input.x ;
        }
        else if (input.y != 0 && input.x == 0)
        {
            moveDirection.z = input.y ;
        }
        Vector3Int moveDirectionInt = new Vector3Int(Mathf.RoundToInt(moveDirection.x), 0, Mathf.RoundToInt(moveDirection.z));
        Node targetNode = _gridManager.OneDirectionToLast(currentNode, moveDirectionInt).Last();
        currentNode = targetNode;
        MoveFeedBack(targetNode);
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

    private void CheckAndMove(List<Node> checkNodes,List<Node.NodeType> checkNodeType)
    {
        if (!checkNodes.Contains(null) && !checkNodeType.Contains(Node.NodeType.Obstacle) && !checkNodeType.Contains(Node.NodeType.Void))
        {
            MoveFeedBack(checkNodes.Last());
            currentNode = checkNodes.Last();
        }
        else
        {
            isCrashed = true;
            OnCrash();
        }
    }
    void MoveFeedBack(Node targetNode)
    {
        GameEvents.current.onMovePerformed();
        DOVirtual.DelayedCall(0.1f, () => { isMoving = false;}).SetEase(Ease.Linear);
        transform.DOMove(targetNode.cords, 1.25f).SetEase(Ease.OutQuart);
        transform.DOLookAt(targetNode.cords, 0.1f);
    }
    void CrashFeedback(Vector3Int crashGridCords)
    {
        
    }
    
}