using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    private Node currentNode;
    [SerializeField] private int _UnitEnginePower;
    
    private void SetNode(Node targetNode)
    {
        currentNode = targetNode;
    }
}
