using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;
using Sequence = DG.Tweening.Sequence;

public class AIUnit : MonoBehaviour,IObject
{
    
    [SerializeField] private List<GameObject> _wheels;
    [SerializeField] private GameObject _top;
    [SerializeField] private GameObject _body;
    
    [SerializeField] private bool isCrashed = false;
    [SerializeField] private bool isMoving = false;
    
    [SerializeField] private GridManager _gridManager;
    private List<Node> AIPath = new List<Node>();
    private Node startPoint, endPoint;
    [SerializeField] private int _UnitEnginePower = 1;
    [SerializeField] private GameObject nextNodeFeedbackObj;

    
    [SerializeField] private Transform SmokeEffectSlot;
    private Sequence MoveSequence;
    private Sequence EngineFeedbackSequence;
    public void Move(Vector2 input)
    {
        MoveLocal(lastInput,false);
    }
    public void Crash(Node crashNode)
    {
        OnCrash(crashNode);
    }
    void Start()
    {
        GameEvents.current.onMove += OnPlayerMove;
        startPoint = _gridManager.GetTileAt(Direction.GetCords(transform.position));
        startPoint.onNodeObject = this;
        endPoint = _gridManager.FinishLine;
        UnitStartFeedback();
        MoveSequence = DOTween.Sequence();
        EngineFeedbackSequence = DOTween.Sequence();
        nextNodeFeedback();
    }

    private void nextNodeFeedback()
    {
        AIPath =  _gridManager.PathNodes(startPoint,endPoint,_UnitEnginePower);
        nextNodeFeedbackObj.transform.position = AIPath[0].cords;
    }

    private void OnPlayerMove(Transform objTrans, int id , bool selfCommand)
    {
        if (!selfCommand || isCrashed) return;
        AIPath =  _gridManager.PathNodes(startPoint,endPoint,_UnitEnginePower);
        Debug.Log(AIPath);
        if(AIPath.Count<=0 || AIPath == null) return;
        startPoint.onNodeObject = null;
        lastInput = new Vector2(( AIPath[0].cords - startPoint.cords ).x,(AIPath[0].cords - startPoint.cords ).z);
        startPoint = AIPath[0];
        VehicleFeedBack(lastInput);
        MoveFeedBack(AIPath[0],false);
        AIPath[0].Interact(this);
    }
    private void MoveLocal(Vector2 input, bool selfCommand)
    {
        if (isMoving && selfCommand || isCrashed)return;
        isMoving = true;
        Vector3 moveDirection = Vector3.zero;
        List<Node> checkNodes = new List<Node>();
        List<Node.NodeTag> checkNodeType = new List<Node.NodeTag>();
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
            Vector3Int targetCord = startPoint.cords + moveDirectionInt;
            Node targetNode = _gridManager.GetTileAt(targetCord);
            CheckNode(targetNode,checkNodes, checkNodeType);
        }
        CheckAndMove(checkNodes, checkNodeType,input,selfCommand);
    }
        private void CheckNode(Node targetNode,List<Node> checkNodes,List<Node.NodeTag> checkNodeType)
    {
        checkNodes.Add(targetNode);
        if (targetNode == null)
        {
            checkNodeType.Add(Node.NodeTag.Void);
        }
        else
        {
            checkNodeType.Add(targetNode.currentTag);
        }
    }
    private void CheckAndMove(List<Node> checkNodes,List<Node.NodeTag> checkNodeType , Vector2 input , bool selfCommand)
    {
        Node firstMatchingNode = null;
        for (int i = 0; i < checkNodes.Count; i++)
        {
            if (checkNodes[i] == null || checkNodeType[i] == Node.NodeTag.Obstacle || checkNodeType[i] == Node.NodeTag.Void)
            {
                firstMatchingNode = checkNodes[i];
                break;
            }
        }
        if (firstMatchingNode == null)
        {
            VehicleFeedBack(input);
            MoveFeedBack(checkNodes.Last(),selfCommand);
            startPoint.onNodeObject = null;
            startPoint = checkNodes.Last();
            startPoint.Interact(this);
        }
        else
        {
            isCrashed = true;
            OnCrash(firstMatchingNode);
        }
    }
    void UnitStartFeedback()
    {
        _top.transform.DOLocalMoveY(-0.081f, 0.12f).SetLoops(-1,LoopType.Yoyo);
    }
    void MoveFeedBack(Node targetNode,bool selfCommand)
    {
        GameEvents.current.onMovePerformed(SmokeEffectSlot,0,selfCommand);
        DOVirtual.DelayedCall(0.1f, () => { isMoving = false;}).SetEase(Ease.Linear).OnComplete(()=> {nextNodeFeedback();});
        DOTween.Kill(this.transform);
        transform.DOMove(targetNode.cords, 1f).SetEase(Ease.OutQuart);
        transform.DOLookAt(targetNode.cords, 0.1f).SetEase(Ease.OutQuart);
        if (targetNode.onNodeObject == null)
        {
            targetNode.onNodeObject = this;

        }
        else
        {
            targetNode.onNodeObject.Crash(targetNode);
            OnCrash(targetNode);
        }
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
    private void OnCrash(Node _crashNode)
    {
        isCrashed = true;
        CrashFeedback(_crashNode);
    }
    public Vector2 lastInput { get; set; }

}
