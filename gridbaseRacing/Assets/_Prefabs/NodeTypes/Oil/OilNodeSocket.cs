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
    public void Interact(IObject interactOwner)
    {
        interactOwner.Move(interactOwner.lastInput);
        GameEvents.current.onOilPerformed(OilParticleFeedback,0);
    }

    public Node PredictMove(Node fromNode)
    {
        throw new System.NotImplementedException();
    }

    public void UnInteract(IObject interactOwner)
    {

    }
}
