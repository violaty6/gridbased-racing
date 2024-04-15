using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OilNodeSocket : MonoBehaviour, INode
{
    [SerializeField] private Transform OilParticleFeedback;
    public void Init()
    {
        Debug.Log("OilInit");
    }
    public void Interact(Node fromNode, Node toNode,IObject interactOwner)
    {
        Vector2 direction = GridManager.Instance.GetDirectionNodeToNode(fromNode, toNode);
        Node targetNode = GridManager.Instance.GetOneNodeOneDirection(toNode,direction);
        interactOwner.Move(targetNode,false);
        GameEvents.current.onOilPerformed(OilParticleFeedback,0);
    }

    public Node PredictInteraction(Node fromNode, Node toNode)
    {
        Vector2 direction = GridManager.Instance.GetDirectionNodeToNode(fromNode, toNode);
        return GridManager.Instance.GetOneNodeOneDirection(toNode,direction);
    }
    
    public void UnInteract(IObject interactOwner)
    {

    }
}
