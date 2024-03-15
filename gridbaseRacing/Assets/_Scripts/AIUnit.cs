using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AIUnit : MonoBehaviour
{
    [SerializeField] private GridManager _gridManager;
    private List<Node> AIPath = new List<Node>();
    private Node startPoint, endPoint;
    [SerializeField] private int _UnitEnginePower = 1;
    void Start()
    {
        GameEvents.current.onMove += onPlayerMove;
        startPoint = _gridManager.GetTileAt(Direction.GetCords(transform.position));
        endPoint = _gridManager.FinishLine;
    }

    private void onPlayerMove()
    {
        AIPath =  _gridManager.PathNodes(startPoint,endPoint,_UnitEnginePower);
        if(AIPath.Count<=0) return;
        startPoint = AIPath[0];
        transform.DOMove( AIPath[0].cords, 1.25f).SetEase(Ease.OutQuart);
        transform.DOLookAt(AIPath[0].cords, 0.1f);
    }
    
}
