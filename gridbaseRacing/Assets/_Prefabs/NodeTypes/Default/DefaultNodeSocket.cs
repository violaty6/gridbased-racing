using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultNodeSocket : MonoBehaviour, INode
{
    public void Init()
    {
    }
    public void Interact(Node fromNode, Node toNode,IObject interactOwner)
    {
    }
    public Node PredictInteraction(Node fromNode, Node toNode)
    {
        return toNode;
    }

    public void UnInteract(IObject interactOwner)
    {
    }
}