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
        forwardDirection = new Vector2Int(Mathf.RoundToInt(transform.forward.x), Mathf.RoundToInt(transform.forward.z));
        rightDirection =new Vector2Int(Mathf.RoundToInt(transform.right.x), Mathf.RoundToInt(transform.right.z));
        movePath = new List<Vector3>();
    }
    private void OnEnable()
    {
        _unitControls.Enable();
        _unitControls.BasicMovement.Move.performed += MoveInput;
        _unitControls.BasicMovement.Reverse.performed += reverseGear;
    }
    private void OnDisable()
    {
        _unitControls.Disable();
        _unitControls.BasicMovement.Move.performed -= MoveInput;
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
    private void MoveInput(InputAction.CallbackContext ctx)
    {
        Vector2 inputNotRounded = ctx.ReadValue<Vector2>();
        Vector2Int input = new Vector2Int(Mathf.RoundToInt(inputNotRounded.x), Mathf.RoundToInt(inputNotRounded.y));
        lastInput = input;
        if (input == new Vector2Int(0,1)) 
        {
            MoveLocal(forwardDirection,true,false);
        }
        else if (input == new Vector2Int(-1, 0) || input == new Vector2Int(1, 0))
        {
            MoveLocalTurn(forwardDirection,rightDirection*input.x,true,true);
        }
    }

    private void reverseGear(InputAction.CallbackContext ctx)
    {
        isReverse = isReverse*-1;
        forwardDirection = -forwardDirection;
        rightDirection = -rightDirection;
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
        Vector3Int targetCord2 = targetCord + moveDirectionInt2;
        Node targetNode = _gridManager.GetTileAt(targetCord);
        Node targetNode2 = _gridManager.GetTileAt(targetCord2);
        if (targetNode == null)
        {
            GameEvents.current.onErrorPerformed(targetCord,0);
            return;
        }
        if (targetNode2 == null)
        {
            GameEvents.current.onErrorPerformed(targetCord2,0);
            return;
        }
        isMoving = true;
        CheckAndMove(targetNode,isPlayerAction,false);
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
                forwardDirection = (rightDirection * lastInput.x);
                rightDirection =new Vector2Int(forwardDirection.y, -forwardDirection.x);
            }
            currentNode.UnInteract(this);
            previusNode = currentNode;
            currentNode.onNodeObject = null;
            //------------
            Debug.Log("bob");
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
        // MoveSequence.Insert(5f,transform.DOMove(targetNode.cords, 1f).OnComplete((() => {if(!isPlayerAction)_unitControls.Enable();})));
        // transform.DOLookAt(targetNode.cords, 0f);
        
        //TİRES
        _body.GetComponent<Rigidbody>().AddRelativeTorque(-Vector2.right*3,ForceMode.Impulse);
        foreach (var wheel in _wheels)
        {
            wheel.transform.DOLocalRotate(new Vector3(-360*isReverse,0,0), 1.25f,RotateMode.LocalAxisAdd).SetEase(Ease.OutQuart);
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
        CheckAndMove(nextNode,isPlayerAction,false);
        isMoving = true;
    }
    public void Crash(Node crashNode)
    {
        OnCrash(crashNode);
    }
}