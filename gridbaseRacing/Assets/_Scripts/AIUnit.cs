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
        GameEvents.current.onMove += OnPlayerMove;
        startPoint = _gridManager.GetTileAt(Direction.GetCords(transform.position));
        startPoint.onNodeObject++;
        endPoint = _gridManager.FinishLine;
    }

    private void OnPlayerMove(Transform objTrans, int id)
    {
        AIPath =  _gridManager.PathNodes(startPoint,endPoint,_UnitEnginePower);
        if(AIPath.Count<=0) return;
        startPoint.onNodeObject--;
        startPoint = AIPath[0];
        transform.DOMove( AIPath[0].cords, 1.25f).SetEase(Ease.OutQuart);
        transform.DOLookAt(AIPath[0].cords, 0.1f);
        AIPath[0].onNodeObject++;
    }
    
}
