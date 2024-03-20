using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Sequence = DG.Tweening.Sequence;

public class UnitController : MonoBehaviour
{

    [SerializeField] private List<GameObject> _wheels;
    [SerializeField] private GameObject _top;
    [SerializeField] private GameObject _body;
    
    [SerializeField] private bool isCrashed = false;
    [SerializeField] private bool isMoving = false;
    
    [SerializeField] private bool canTurbo = true;
    [SerializeField] private bool isTurbo = false;
    
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private Node currentNode;
    [SerializeField] private int _UnitEnginePower;
    [SerializeField] private Transform SmokeEffectSlot;
    private UnitControls _unitControls;
    private Sequence MoveSequence;
    private Sequence EngineFeedbackSequence;
    
    
    
    private void Awake()
    {
        _unitControls = new UnitControls();
        _gridManager = FindObjectOfType<GridManager>();
    }
    private void Start()
    {
        UnitStartFeedback();
        MoveSequence = DOTween.Sequence();
        EngineFeedbackSequence = DOTween.Sequence();
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
        CrashFeedback();
    }
    private void Move(InputAction.CallbackContext ctx)
    {
        if (isMoving)return;
        isMoving = true;
        Vector2 input = ctx.ReadValue<Vector2>();
        Debug.Log(input);
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
        CheckAndMove(checkNodes, checkNodeType,input);
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
        VehicleFeedBack(input);
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

    private void CheckAndMove(List<Node> checkNodes,List<Node.NodeType> checkNodeType , Vector2 input)
    {
        if (!checkNodes.Contains(null) && !checkNodeType.Contains(Node.NodeType.Obstacle) && !checkNodeType.Contains(Node.NodeType.Void))
        {
            VehicleFeedBack(input);
            MoveFeedBack(checkNodes.Last());
            currentNode = checkNodes.Last();
        }
        else
        {
            isCrashed = true;
            OnCrash();
        }
    }

    void UnitStartFeedback()
    {
        _top.transform.DOLocalMoveY(-0.081f, 0.12f).SetLoops(-1,LoopType.Yoyo);
    }
    void MoveFeedBack(Node targetNode)
    {
        GameEvents.current.onMovePerformed(SmokeEffectSlot);
        DOVirtual.DelayedCall(0.1f, () => { isMoving = false;}).SetEase(Ease.Linear);
        transform.DOMove(targetNode.cords, 1.25f).SetEase(Ease.OutQuart);
        transform.DOLookAt(targetNode.cords, 0.1f);
    }

    void VehicleFeedBack(Vector2 input)
    {
        MoveSequence.Kill();
        MoveSequence = DOTween.Sequence();
        MoveSequence.Append(_top.transform.DOLocalMoveZ(-0.02f, 0.1f));
        MoveSequence.Append(        
            _top.transform.DOLocalRotate(new Vector3(-5, 0, 0), 0.5f).SetEase(Ease.OutExpo).OnComplete(() =>
        {
            _top.transform.DOLocalMoveZ(0.02f, 0.2f).SetEase(Ease.InBack);
            _top.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.2f).SetEase(Ease.InBack);
        }
            )
        );
        foreach (var wheel in _wheels)
        {
            wheel.transform.DOLocalRotate(new Vector3(0,-360,0), 1.25f,RotateMode.LocalAxisAdd).SetEase(Ease.OutQuart);
        }
    }
    void CrashFeedback()
    {
        MoveSequence.Kill();
        EngineFeedbackSequence.Kill();
        DOTween.Kill(_top.transform);
        
        GameEvents.current.onCrashPerformed(_top.transform);
        foreach (var wheels in _wheels)
        {
           Rigidbody expolionObject =  wheels.AddComponent<Rigidbody>();
           expolionObject.AddExplosionForce(1000f,_top.transform.position,10f);
        }
        foreach (var bodyParts in _body.GetComponentsInChildren<Transform>())
        {
            if (bodyParts.gameObject == _body.gameObject)continue;
            DOTween.Kill(bodyParts);
            Rigidbody expolionObject =  bodyParts.AddComponent<Rigidbody>();
            expolionObject.AddExplosionForce(1000f,_top.transform.position,10f);
        }
    }
}