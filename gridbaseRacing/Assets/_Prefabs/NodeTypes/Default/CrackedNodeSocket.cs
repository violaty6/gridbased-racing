using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CrackedNodeSocket : MonoBehaviour,INode
{
    [SerializeField] private GameObject BreakFeedbackParticle;
    private Node ownerNode;
    public void Init()
    {
        ownerNode = transform.parent.GetComponent<Node>();
    }

    public void Interact(Node fromNode, Node toNode,IObject interactOwner)
    {
        transform.DOShakeRotation(0.5f, 5f).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
        transform.parent.GetComponent<Node>().currentTag = Node.NodeTag.Void;
    }

    public Node PredictInteraction(Node fromNode, Node toNode)
    {
        return toNode;
    }
    public void UnInteract(IObject interactOwner)
    {
        transform.parent.GetComponent<Node>().currentTag = Node.NodeTag.Void;
        BreakFeedbackParticle.transform.parent = null;
        BreakFeedbackParticle.SetActive(true);
        DOTween.Kill(transform);
        Destroy(transform.gameObject);
    }
}
