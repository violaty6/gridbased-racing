using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CrackedNodeSocket : MonoBehaviour,INode
{
    [SerializeField] private GameObject BreakFeedbackParticle;
    public void Init()
    {
        
    }

    public void Interact(IObject interactOwner)
    {
        transform.DOShakeRotation(0.5f, 5f).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }
    public Node PredictMove(Node fromNode)
    {
        return null;
    }

    public void UnInteract(IObject interactOwner)
    {
        transform.parent.GetComponent<Node>().currentTag = Node.NodeTag.Void;
        BreakFeedbackParticle.transform.parent = null;
        BreakFeedbackParticle.SetActive(true);
        Destroy(transform.gameObject);
    }
}
