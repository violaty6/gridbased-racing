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

    [SerializeField] private int _UnitEnginePower = 1;
    [SerializeField] private GameObject nextNodeFeedbackObj; 
        
    [SerializeField] private Transform SmokeEffectSlot;
    private Sequence MoveSequence;
    private Sequence EngineFeedbackSequence;
    public void Move(Node nextNode, bool isPlayerAction)
    {
        CheckAndMove(nextNode);
    }
    public Vector2 lastInput { get; set; }
    public Node currentNode { get; set; }
    public Node previusNode;
    private Node endPoint;
    public void Crash(Node crashNode)
    {
        OnCrash(crashNode);
    }
    void Start()
    {

        MoveSequence = DOTween.Sequence();
        EngineFeedbackSequence = DOTween.Sequence();
        GameEvents.current.onMove += OnAIMove;
        currentNode = _gridManager.GetTileAt(Direction.GetCords(transform.position));
        currentNode.onNodeObject = this;
        endPoint = _gridManager.FinishLine;
        // Feedbacks
        UnitStartFeedback();
        NextNodeFeedback();
    }

    private void NextNodeFeedback()
    {
        AIPath =  _gridManager.PathNodes(currentNode,endPoint,_UnitEnginePower);
        if(AIPath == null) return;
        if(AIPath.Count<=0) return;
        nextNodeFeedbackObj.transform.position = AIPath[0].cords;
    }

    private void OnAIMove(int id)
    {
        if (isCrashed) return;
        AIPath =  _gridManager.PathNodes(currentNode,endPoint,_UnitEnginePower);
        NextNodeFeedback();
        if(AIPath == null) return;
        if(AIPath.Count<=0) return;
        Move(AIPath[0],false);
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
            previusNode = currentNode;
            currentNode.onNodeObject = null;
            
            currentNode = checkNode;
            currentNode.Interact(previusNode,currentNode,this);
        }
    }
    void UnitStartFeedback()
    {
        _top.transform.DOLocalMoveY(_top.transform.localPosition.y + 0.01f, 0.14f).SetLoops(-1,LoopType.Yoyo);
    }
    void MoveFeedBack(Node targetNode)
    {
        GameEvents.current.onSmokePerformed(SmokeEffectSlot,0);
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
        _body.GetComponent<Rigidbody>().AddRelativeTorque(-Vector2.right*3,ForceMode.Impulse);
        foreach (var wheel in _wheels)
        {
            wheel.transform.DOLocalRotate(new Vector3(-360,0,0), 1f,RotateMode.LocalAxisAdd).SetEase(Ease.OutQuart);
        }
    }
    void CrashFeedback(Node node)
    {
        VehicleFeedBack();
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
                expolionObject.AddExplosionForce(350f, _top.transform.position, 10f);
            }
            foreach (var bodyParts in _top.GetComponentsInChildren<Transform>())
            {
                if (bodyParts.gameObject == _top.gameObject) continue;
                DOTween.Kill(bodyParts);
                Rigidbody expolionObject;
                if (!bodyParts.GetComponent<Rigidbody>())
                {
                    expolionObject= bodyParts.AddComponent<Rigidbody>();
                    expolionObject.isKinematic = false;
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

                expolionObject.AddExplosionForce(350f, _top.transform.position, 10f);
            }
        });
    }
    private void OnCrash(Node _crashNode)
    {
        isCrashed = true;
        currentNode.onNodeObject = null;
        nextNodeFeedbackObj.transform.DOScale(0f, 0.1f);
        CrashFeedback(_crashNode);
    }


}
