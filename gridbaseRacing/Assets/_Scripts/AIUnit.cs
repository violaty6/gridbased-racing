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
    [SerializeField] private Node currentNode;
        
    [SerializeField] private Transform SmokeEffectSlot;
    private Sequence MoveSequence;
    private Sequence EngineFeedbackSequence;
    public void Move(Node nextNode)
    {
        CheckAndMove(nextNode);
    }
    public void Crash(Node crashNode)
    {
        OnCrash(crashNode);
    }
    void Start()
    {

        MoveSequence = DOTween.Sequence();
        EngineFeedbackSequence = DOTween.Sequence();
        GameEvents.current.onMove += OnPlayerMove;
        startPoint = _gridManager.GetTileAt(Direction.GetCords(transform.position));
        startPoint.onNodeObject = this;
        endPoint = _gridManager.FinishLine;
        // Feedbacks
        UnitStartFeedback();
        NextNodeFeedback();
    }

    private void NextNodeFeedback()
    {
        AIPath =  _gridManager.PathNodes(startPoint,endPoint,_UnitEnginePower);
        if(AIPath.Count >0)        nextNodeFeedbackObj.transform.position = AIPath[0].cords;
    }

    private void OnPlayerMove(Transform objTrans, int id)
    {
        if (isCrashed) return;
        AIPath =  _gridManager.PathNodes(startPoint,endPoint,_UnitEnginePower);
        if(AIPath.Count<=0 || AIPath == null) return;
        startPoint.onNodeObject = null;
        lastInput = _gridManager.GetDirectionNodeToNode(startPoint, AIPath[0]);
        startPoint.UnInteract(this);
        startPoint = AIPath[0];
        VehicleFeedBack();
        MoveFeedBack(AIPath[0]);
        AIPath[0].Interact(this);
    }


    private void MoveLocal(Vector2 input)
    {
        if (isMoving)return;
        isMoving = true;
        lastInput = input;
        Node targetNode = _gridManager.GetOneNodeOneDirection(currentNode, input);
        CheckAndMove(targetNode);
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
            MoveFeedBack(checkNode);
            currentNode.UnInteract(this);
            currentNode.onNodeObject = null;
            currentNode = checkNode;
            currentNode.Interact(this);
        }
    }
    void UnitStartFeedback()
    {
        _top.transform.DOLocalMoveY(-0.081f, 0.12f).SetLoops(-1,LoopType.Yoyo);
    }
    void MoveFeedBack(Node targetNode)
    {
        GameEvents.current.onMovePerformed(SmokeEffectSlot,0);
        DOVirtual.DelayedCall(0.1f, () => { isMoving = false;}).SetEase(Ease.Linear).OnComplete(()=> {NextNodeFeedback();});
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
    private void OnCrash(Node _crashNode)
    {
        isCrashed = true;
        CrashFeedback(_crashNode);
    }
    public Vector2 lastInput { get; set; }

}
