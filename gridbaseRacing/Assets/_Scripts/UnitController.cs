using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Sequence = DG.Tweening.Sequence;

public interface IObject
{
    public Vector2 lastInput { get; set; }
    public void Move(Node nextNode);
    public void Crash(Node crashNode);
}

public class UnitController : MonoBehaviour, IObject
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
    [SerializeField] private GameObject currentNodeFeedback;
    [SerializeField] private int _UnitEnginePower;
    [SerializeField] private Transform SmokeEffectSlot;
    private UnitControls _unitControls;
    private Sequence MoveSequence;
    private Sequence EngineFeedbackSequence;

    public Vector2 lastInput { get; set; }
    
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
        currentNode.onNodeObject = this;
    }
    private void OnEnable()
    {
        _unitControls.Enable();
        _unitControls.BasicMovement.Move.performed += MoveInput;
        _unitControls.BasicMovement.Turbo.performed += CheckTurbo;
    }
    private void OnCrash(Node _crashNode)
    {
        _unitControls.Disable();
        _unitControls.BasicMovement.Move.performed -= MoveInput;
        _unitControls.BasicMovement.Turbo.performed -= CheckTurbo;
        CrashFeedback(_crashNode);
    }
    private void MoveInput(InputAction.CallbackContext ctx)
    {
        Vector2 input = ctx.ReadValue<Vector2>();
        MoveLocal(input,true);
    }

    private void MoveLocal(Vector2 input, bool selfCommand)
    {
        if (isMoving && selfCommand)return;
        isMoving = true;
        lastInput = input;
        // if (isTurbo) // TURBO
        // {
        //     Turbo(input);
        //     isTurbo = false;
        //     return;
        // }
        Vector3 moveDirection = Vector3.zero;
        if (input.x != 0 && input.y == 0)
        {
            moveDirection.x = input.x;
        }
        else if (input.y != 0 && input.x == 0)
        {
            moveDirection.z = input.y;
        }
        Vector3Int moveDirectionInt = new Vector3Int(Mathf.RoundToInt(moveDirection.x), 0, Mathf.RoundToInt(moveDirection.z));
        Vector3Int targetCord = currentNode.cords + moveDirectionInt;
        Node targetNode = _gridManager.GetTileAt(targetCord);
        CheckAndMove(targetNode);
    }
    private void CheckTurbo(InputAction.CallbackContext ctx)
    {
        if (canTurbo)
        {
            isTurbo = true;
            canTurbo = false;
        }
    }
    // private void Turbo(Vector2 input)
    // {
    //     isMoving = true;
    //     Vector3 moveDirection = Vector3.zero;
    //     if (input.x != 0 && input.y == 0)
    //     {
    //         moveDirection.x = input.x ;
    //     }
    //     else if (input.y != 0 && input.x == 0)
    //     {
    //         moveDirection.z = input.y ;
    //     }
    //     Vector3Int moveDirectionInt = new Vector3Int(Mathf.RoundToInt(moveDirection.x), 0, Mathf.RoundToInt(moveDirection.z));
    //     Node targetNode = _gridManager.OneDirectionToLast(currentNode, moveDirectionInt).Last();
    //     currentNode.UnInteract(this);
    //     currentNode.onNodeObject = null;
    //     currentNode = targetNode;
    //     currentNode.Interact(this);
    //     currentNodeFeedback.transform.position = currentNode.cords;
    //     VehicleFeedBack();
    //     MoveFeedBack(targetNode);
    // }
    private Node.NodeTag CheckNode(Node targetNode)
    {
        if (targetNode == null)
        {
            return Node.NodeTag.Void;
        }
        else
        {
            return targetNode.currentTag;
        }
    }
    private void CheckAndMove(Node checkNode)
    {
        Node.NodeTag targetNodeTag = CheckNode(checkNode); // Null ise Void ekliyor.

        if (targetNodeTag == Node.NodeTag.Void)
        {
            //Hata feedbacki gidemez move sayılmaz
        }
        else if (targetNodeTag == Node.NodeTag.Obstacle )
        {
            isCrashed = true;
            OnCrash(checkNode);
        }
        else
        {
            VehicleFeedBack();
            GameEvents.current.onMovePerformed(SmokeEffectSlot,0); // MOVE event ----------------------------------
            MoveFeedBack(checkNode);
            currentNode.UnInteract(this);
            currentNode.onNodeObject = null;
            currentNode = checkNode;
            currentNode.Interact(this);
            currentNodeFeedback.transform.position = currentNode.cords;
        }
    }
    void UnitStartFeedback()
    {
        _top.transform.DOLocalMoveY(-0.081f, 0.12f).SetLoops(-1,LoopType.Yoyo);
    }
    void MoveFeedBack(Node targetNode)
    {
        DOVirtual.DelayedCall(0.1f, () => { isMoving = false;}).SetEase(Ease.Linear);
        transform.DOMove(targetNode.cords, 1f).SetEase(Ease.OutQuart);
        transform.DOLookAt(targetNode.cords, 0.1f).SetEase(Ease.OutQuart);
        
        // Başka object varsa gideceği yerde
        if (targetNode.onNodeObject == null)
        {
            targetNode.onNodeObject = this;

        }
        else if(targetNode.onNodeObject != this)
        {
            targetNode.onNodeObject.Crash(targetNode);
            OnCrash(targetNode);
        }
    }

    void VehicleFeedBack()
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
    void CrashFeedback(Node node)
    {
        transform.DOMove(node.cords, 1f).SetEase(Ease.OutQuart);
        transform.DOLookAt(node.cords, 0.1f).SetEase(Ease.OutQuart);
        DOVirtual.DelayedCall(0.25f, () =>
        {
            MoveSequence.Kill();
            EngineFeedbackSequence.Kill();
            DOTween.Kill(_top.transform);
            node.transform.DOPunchScale(Vector3.one/10, 0.5f);
            GameEvents.current.onCrashPerformed(_top.transform,0);
            foreach (var wheels in _wheels)
            {
                Rigidbody expolionObject = wheels.AddComponent<Rigidbody>();
                expolionObject.AddExplosionForce(1000f, _top.transform.position, 10f);
            }

            foreach (var bodyParts in _body.GetComponentsInChildren<Transform>())
            {
                if (bodyParts.gameObject == _body.gameObject) continue;
                DOTween.Kill(bodyParts);
                Rigidbody expolionObject = bodyParts.AddComponent<Rigidbody>();
                expolionObject.AddExplosionForce(1000f, _top.transform.position, 10f);
            }
        });
    }
    public void Move(Node nextNode)
    {
        CheckAndMove(nextNode); 
        _unitControls.Disable();
    }

    public void Crash(Node crashNode)
    {
        OnCrash(crashNode);
    }
}