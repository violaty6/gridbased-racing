using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishNodeSocket : MonoBehaviour,INode
{
    
    public void Init()
    {
    }

    public void Interact(Node fromNode, Node toNode,IObject interactOwner)
    {
        Debug.Log(interactOwner.forward);
        Vector2 direction = new Vector2(-transform.right.x,-transform.right.z);
        Debug.Log(direction);
        if (direction == interactOwner.forward)
        {
            Debug.Log(interactOwner + "win");
        }
    }
    public Node PredictInteraction(Node fromNode, Node toNode)
    {
        return toNode;
    }

    public void UnInteract(IObject interactOwner)
    {
        
    }
}
