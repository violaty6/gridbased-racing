using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CrackedNodeSocket : MonoBehaviour,INode
{
    public void Init()
    {
        
    }

    public void Interact(IObject interactOwner)
    {
        transform.DOShakeRotation(0.5f, 5f).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }

    public void UnInteract(IObject interactOwner)
    {
        transform.parent.GetComponent<Node>().currentTag = Node.NodeTag.Void;
        Destroy(transform.gameObject);
    }
}
