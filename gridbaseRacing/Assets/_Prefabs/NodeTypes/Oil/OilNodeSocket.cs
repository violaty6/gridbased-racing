using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OilNodeSocket : MonoBehaviour, INode
{
    [SerializeField] private Transform OilParticleFeedback;
    [SerializeField] private GameObject Oil;
    [SerializeField]private bool isInteracted = false;
    public void Init()
    {
        Debug.Log("OilInit");
    }
    public void Interact(Node fromNode, Node toNode,IObject interactOwner)
    {
        if(isInteracted) return;
        Vector2 direction = GridManager.Instance.GetDirectionNodeToNode(fromNode, toNode);
        Node targetNode = GridManager.Instance.GetOneNodeOneDirection(toNode,direction);
        if (targetNode == null) return;
        if (GridManager.Instance.PredictCheck(toNode, targetNode) == null)return;
        else
        {
            interactOwner.Move(targetNode,false);
            GameEvents.current.onOilPerformed(OilParticleFeedback,0);
            Oil.transform.DOScale(0.5f, 0.4f).SetEase(Ease.InBack).OnComplete(()=>{Oil.SetActive(false);});
            isInteracted = true;
        }
    }

    public Node PredictInteraction(Node fromNode, Node toNode)
    {
        if (isInteracted) return toNode;
        Vector2 direction = GridManager.Instance.GetDirectionNodeToNode(fromNode, toNode);
        return GridManager.Instance.GetOneNodeOneDirection(toNode,direction);
    }
    
    public void UnInteract(IObject interactOwner)
    {

    }
}
