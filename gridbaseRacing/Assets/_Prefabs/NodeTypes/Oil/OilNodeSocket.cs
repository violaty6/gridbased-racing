using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilNodeSocket : MonoBehaviour, INode
{
    [SerializeField] private Transform OilParticleFeedback;
    public void Init()
    {
        Debug.Log("OilInit");
    }
    public void Interact(IObject interactOwner)
    {
        Debug.Log(interactOwner.lastInput);
        interactOwner.Move(interactOwner.lastInput);
        GameEvents.current.onOilPerformed(OilParticleFeedback,0);
    }
}
