using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AIUnit : MonoBehaviour
{
    void Start()
    {
        GameEvents.current.onMove += onPlayerMove;
    }

    private void onPlayerMove()
    {
        transform.DOMove(transform.position + Vector3.forward, 1.25f).SetEase(Ease.OutQuart);
        transform.DOLookAt(transform.position + Vector3.forward, 0.1f);
    }
    
}
