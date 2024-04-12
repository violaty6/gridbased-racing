using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishNodeSocket : MonoBehaviour,INode
{
    public void Init()
    {
    }

    public void Interact(IObject interactOwner)
    {
        Debug.Log(interactOwner + "win");
    }
    public Node PredictMove(IObject interactOwner)
    {
        return null;
    }

    public void UnInteract(IObject interactOwner)
    {
        
    }
}
