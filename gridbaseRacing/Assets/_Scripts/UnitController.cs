using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using MoreMountains.Feedbacks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Sequence = DG.Tweening.Sequence;

public interface IObject
{
    public Node currentNode { get; set; }
    public Vector2 forward { get;}
    public void Move(Node nextNode,bool isPlayerAction);
    public void Crash(Node crashNode);
}

public class UnitController : MonoBehaviour, IObject
{

    [SerializeField] private List<GameObject> _wheels;
    [SerializeField] private GameObject _top;
    [SerializeField] private GameObject _body;
    
    [SerializeField] private bool isCrashed = false;
    [SerializeField] private bool isMoving = false;
    
    [SerializeField] private int isReverse = 1;
    
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private GameObject currentNodeFeedback;
    [SerializeField] private int _UnitEnginePower;
    [SerializeField] private Transform SmokeEffectSlot;
    [SerializeField] private float ExplosionForce = 2000f;
    private UnitControls _unitControls;
    private Sequence MoveSequence;
    private Sequence EngineFeedbackSequence;
    [SerializeField] private MMFeedbacks FeelCrashFeedback;

    public Vector2 forward
    {
        get { return forwardDirection*-isReverse; }
    }
    public Vector2Int forwardDirection;
    public Vector2Int rightDirection;
    public Vector2Int lastInput;
    public Vector2Int curInput;

    private List<Vector3> movePath;

    public Node currentNode { get; set; }
    public Node previusNode;
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
        GameEvents.current.onCameraSwitch += CheckInputs;
        forwardDirection = new Vector2Int(Mathf.RoundToInt(transform.forward.x), Mathf.RoundToInt(transform.forward.z)) ;
        rightDirection =new Vector2Int(Mathf.RoundToInt(transform.right.x), Mathf.RoundToInt(transform.right.z));
        curInput = forwardDirection;
        movePath = new List<Vector3>();
    }
    private void OnEnable()
    {
        _unitControls.Enable();
        _unitControls.BasicMovement.Gas.performed += Gas;
        _unitControls.BasicMovement.Move.started += DirectionInput;
        _unitControls.BasicMovement.Move.canceled += ResetDirection;
        _unitControls.BasicMovement.Reverse.performed += reverseGear;
    }
    private void OnDisable()
    {
        _unitControls.Disable();
        _unitControls.BasicMovement.Gas.performed -= Gas;
        _unitControls.BasicMovement.Move.started -= DirectionInput;
        _unitControls.BasicMovement.Move.canceled -= ResetDirection;
        _unitControls.BasicMovement.Reverse.performed -= reverseGear;
        DOTween.KillAll();
    }

    private void CheckInputs(int id,bool switched)
    {
        if (switched)
        {
            _unitControls.Enable();
        }
        else
        {
            _unitControls.Disable();
        }
    }
    private void OnCrash(Node _crashNode)
    {
        isCrashed = true;
        _unitControls.Disable();
        _unitControls.BasicMovement.Move.performed -= MoveInput;
        CrashFeedback(_crashNode);
    }

    private void Gas(InputAction.CallbackContext ctx)
    {
        if (curInput == new Vector2Int(0,1))
        {
            MoveLocal(forwardDirection,true,false);
        }
        if  (curInput == new Vector2Int(-1, 0) || curInput == new Vector2Int(1, 0) )
        {
            MoveLocalTurn(forwardDirection,rightDirection*curInput.x,true,true);
        }
    }

    private void DirectionInput(InputAction.CallbackContext ctx)
    {
        Vector2 inputNotRounded = ctx.ReadValue<Vector2>();
        Vector2Int input = new Vector2Int(Mathf.RoundToInt(inputNotRounded.x), Mathf.RoundToInt(inputNotRounded.y));
        if (input == new Vector2Int(-1, 0) || input == new Vector2Int(1, 0))
        {
            curInput = input;
            WheelsRotateFeedback(20); 
        }
        GameEvents.current.onDirectionSwitchPerformed(0,curInput);
    }
    
    private void MoveInput(InputAction.CallbackContext ctx)
    {
        Vector2 inputNotRounded = ctx.ReadValue<Vector2>();
        Vector2Int input = new Vector2Int(Mathf.RoundToInt(inputNotRounded.x), Mathf.RoundToInt(inputNotRounded.y));
        lastInput = input;
        // if (input == new Vector2Int(0,1))
        // {
        //     if (isReverse ==-1)
        //     {
        //         reverseGear(new InputAction.CallbackContext());
        //         return;
        //     }
        //     curInput = input;
        //     WheelsRotateFeedback(0);
        // }
        // if (input == new Vector2Int(0, -1))
        // {
        //     if (isReverse ==1)
        //     {
        //         reverseGear(new InputAction.CallbackContext());
        //     }
        // }
    }
    private void reverseGear(InputAction.CallbackContext ctx)
    {
        isReverse = isReverse*-1;
        forwardDirection = -forwardDirection;
        GameEvents.current.onReverseSwitchPerformed(0,isReverse);
        // rightDirection = -rightDirection;
    }
    private void MoveLocal(Vector2 input, bool isPlayerAction,bool isTurn)
    {
        if(isCrashed)return;
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        Vector3Int moveDirectionInt = new Vector3Int(Mathf.RoundToInt(moveDirection.x), 0, Mathf.RoundToInt(moveDirection.z));
        Vector3Int targetCord = currentNode.cords + moveDirectionInt;
        Node targetNode = _gridManager.GetTileAt(targetCord);
        if (targetNode == null)
        {
            GameEvents.current.onErrorPerformed(targetCord,0);
            return;
        }
        isMoving = true;
        CheckAndMove(targetNode,isPlayerAction,isTurn);
    }
    private void MoveLocalTurn(Vector2 input1,Vector2 input2, bool isPlayerAction,bool isTurn)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input1.x;
        moveDirection.z = input1.y;
        Vector3Int moveDirectionInt = new Vector3Int(Mathf.RoundToInt(moveDirection.x), 0, Mathf.RoundToInt(moveDirection.z));
        Vector3Int moveDirectionInt2 = new Vector3Int(Mathf.RoundToInt(input2.x), 0, Mathf.RoundToInt(input2.y));

        Vector3Int targetCord = currentNode.cords + moveDirectionInt;
        Node targetNode = _gridManager.GetTileAt(targetCord);
        if (targetNode == null)
        {
            GameEvents.current.onErrorPerformed(targetCord,0);
            return;
        }
        Vector3Int targetCord2 = targetNode.cords + moveDirectionInt2;
        Node targetNode2 = _gridManager.GetTileAt(targetCord2);


        if (targetNode2 == null)
        {
            GameEvents.current.onErrorPerformed(targetCord2,0);
            return;
        }
        isMoving = true;
        CheckAndMove(targetNode,isPlayerAction,false);
        targetCord2 = currentNode.cords + moveDirectionInt2;
        targetNode2 = _gridManager.GetTileAt(targetCord2);
        if (targetNode2 == null)
        {
            GameEvents.current.onErrorPerformed(targetCord2,0);
            return;
        }
        CheckAndMove(targetNode2,isPlayerAction,isTurn);
    }
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
    private void CheckAndMove(Node checkNode,bool isPlayerAction,bool isTurn)
    {
        if (isCrashed)return;
        
        Node.NodeTag targetNodeTag = CheckNode(checkNode); // Null ise Void ekliyor.

        if (targetNodeTag == Node.NodeTag.Void)
        {
            isMoving = false;
            GameEvents.current.onErrorPerformed(checkNode.cords,0);
        }
        else if (targetNodeTag == Node.NodeTag.Obstacle)
        {
            OnCrash(checkNode);
            currentNodeFeedback.transform.position = checkNode.cords;
        }
        else
        {
            if(isPlayerAction)GameEvents.current.onMovePerformed(SmokeEffectSlot,0); // MOVE event ----------------------------------
            GameEvents.current.onSmokePerformed(SmokeEffectSlot,0); // SMOKE event ----------------------------------
            MoveFeedBack(checkNode,isPlayerAction);
            //------------
            if (isTurn)
            {
                Debug.Log("dam");
                forwardDirection = (rightDirection * curInput.x);
                rightDirection =new Vector2Int(forwardDirection.y, -forwardDirection.x)*isReverse;
            }
            currentNode.UnInteract(this);
            previusNode = currentNode;
            currentNode.onNodeObject = null;
            //------------
            currentNode = checkNode;
            currentNode.Interact(previusNode,currentNode,this);
            currentNodeFeedback.transform.position = currentNode.cords;
        }
    }
    void UnitStartFeedback()
    {
        _top.transform.DOLocalMoveY(_top.transform.localPosition.y + 0.01f, 0.12f).SetLoops(-1,LoopType.Yoyo);
    }
    
    //  SetEase(Ease.OutQuart)
    void MoveFeedBack(Node targetNode,bool isPlayerAction)
    {
        movePath.Clear();
        movePath.Add((currentNode.cords.ConvertTo<Vector3>()+(currentNode.cords+targetNode.cords).ConvertTo<Vector3>()/2)/2);
        movePath.Add(targetNode.cords);
        transform.DOPath(movePath.ToArray(), 1.25f, PathType.CatmullRom).SetLookAt(1.25f,Vector3.forward*isReverse).SetEase(Ease.OutQuart);
        // DOVirtual.DelayedCall(0.1f, () => { isMoving = false;}).SetEase(Ease.Linear);
        // MoveSequence.Insert(5f,transform.DOMove(targetNode.cords, 1f).OnComplete((() => {if(!isPlayerAction)_unitControls.Enab3le();})));
        // transform.DOLookAt(targetNode.cords, 0f);
        
        //TİRES
        _body.GetComponent<Rigidbody>().AddRelativeTorque(-Vector2.right*3,ForceMode.Impulse);
        foreach (var wheel in _wheels)
        {
            wheel.transform.GetChild(0).DOLocalRotate(new Vector3(0,0,360*isReverse), 1.25f,RotateMode.LocalAxisAdd).SetEase(Ease.OutQuart);
        }
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
        _body.GetComponent<Rigidbody>().AddRelativeTorque(-Vector2.right*3,ForceMode.Impulse);
        foreach (var wheel in _wheels)
        {
            wheel.transform.DOLocalRotate(new Vector3(-360,0,0), 1f,RotateMode.LocalAxisAdd).SetEase(Ease.OutQuart);
        }
    }

    void WheelsRotateFeedback(float targetfloat)
    {
        _wheels[1].transform.DOLocalRotate(new Vector3(0, targetfloat*curInput.x, 0), 0.6f).SetEase(Ease.OutCubic);
        _wheels[3].transform.DOLocalRotate(new Vector3(0, targetfloat*curInput.x, 0), 0.6f).SetEase(Ease.OutCubic);
    }

    void ResetDirection(InputAction.CallbackContext ctx)
    {
        curInput = new Vector2Int(0,1);
        GameEvents.current.onDirectionSwitchPerformed(0,curInput);
        WheelsRotateFeedback(0);
    }
    void CrashFeedback(Node node)
    {
        VehicleFeedBack();
        currentNode.UnInteract(this);
        movePath.Clear();
        movePath.Add((currentNode.cords.ConvertTo<Vector3>()+(currentNode.cords+node.cords).ConvertTo<Vector3>()/2)/2);
        movePath.Add(node.cords);
        transform.DOPath(movePath.ToArray(), 1.25f, PathType.CatmullRom).SetLookAt(1.25f,Vector3.forward*isReverse).SetEase(Ease.OutQuart);
        DOVirtual.DelayedCall(0.5f, () =>
        {
            FeelCrashFeedback.PlayFeedbacks();
            MoveSequence.Kill();
            EngineFeedbackSequence.Kill();
            DOTween.Kill(_top.transform);
            node.transform.DOPunchScale(Vector3.one/10, 0.5f);
            GameEvents.current.onCrashPerformed(_top.transform,0);
            foreach (var wheels in _wheels)
            {
                Rigidbody expolionObject = wheels.AddComponent<Rigidbody>();
                expolionObject.AddExplosionForce(ExplosionForce, _top.transform.position, 10f);
            }

            foreach (var bodyParts in _top.GetComponentsInChildren<Transform>())
            {
                if (bodyParts.gameObject == _top.gameObject) continue;
                DOTween.Kill(bodyParts);
                Rigidbody expolionObject;
                if (!bodyParts.GetComponent<Rigidbody>())
                {
                    expolionObject= bodyParts.AddComponent<Rigidbody>();
                }
                else
                {
                    expolionObject = bodyParts.GetComponent<Rigidbody>();
                    if (expolionObject.GetComponent<HingeJoint>())
                    {
                        Destroy(expolionObject.GetComponent<HingeJoint>());
                        expolionObject.GetComponent<MeshCollider>().enabled = true;
                    }
                    expolionObject.isKinematic = false;
                }

                expolionObject.AddExplosionForce(ExplosionForce, _top.transform.position, 10f);
            }
        });
    }
    public void Move(Node nextNode,bool isPlayerAction)
    {
        Debug.Log("bum");

        CheckAndMove(nextNode,isPlayerAction,false);
        isMoving = true;
    }
    public void Crash(Node crashNode)
    {
        OnCrash(crashNode);
    }
}