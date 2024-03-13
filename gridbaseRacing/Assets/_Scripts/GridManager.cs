using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class GridManager : MonoBehaviour
{
    [SerializeField]private SerializedDictionary<Vector3Int, Node> gridTileDict;
    private void Awake()
    {
        gridTileDict = new SerializedDictionary<Vector3Int, Node>();
        foreach (var node in FindObjectsOfType<Node>())
        {
            gridTileDict.Add(node.cords,node);
        }
    }

    private void Start()
    {
        Node nextNode = GetTileAt(Vector3Int.zero);
        for (int i = 0; i < 20; i++)
        {
            Node checkedNodeResult = CheckNode(nextNode, GetTileAt(new Vector3Int(0, 0, 11)));
            nextNode = checkedNodeResult;
            if (checkedNodeResult == GetTileAt(new Vector3Int(0, 0, 11))) break;
        }
       
    }

    public Node GetTileAt(Vector3Int cords)
    {
        Node result = null;
        gridTileDict.TryGetValue(cords, out result);
        return result;
    }

    // public List<Node> PathNodes(Node startNode , Node targetNode)
    // {
    //     
    // }

    private Node CheckNode(Node node , Node targetNode)
    {
        Node result = null;
        float sum = int.MaxValue;
        for (int i = 0; i < 4 ; i++)
        {
            Node nextNode = GetTileAt(node.cords + Direction.directionsOffset[i]);
            if (nextNode == null || nextNode.currentType == Node.NodeType.Obstacle) continue;
            float curNodeDistance = CalculateDistance(node.cords, nextNode.cords);
            float curtargetNodeDistance = CalculateDistance(node.cords, targetNode.cords);
            if (sum > (curNodeDistance + curtargetNodeDistance))
            {
                result = nextNode;
                sum = curNodeDistance + curtargetNodeDistance;
            }
        }

        result.transform.DOMoveY(0.2f, 0.1f);
        return result;
    }
    float CalculateDistance(Vector3Int point1, Vector3Int point2)
    {
        float a = Mathf.Abs(point2.x - point1.x);
        float b = Mathf.Abs(point2.z - point1.z);
        float distance = Mathf.Sqrt(a * a + b * b);
        return distance;
    }
}

public static class Direction
{
    public static List<Vector3Int> directionsOffset = new List<Vector3Int>
    {
        new Vector3Int(0, 0, 1), //NORTH
        new Vector3Int(1, 0, 0), //EAST
        new Vector3Int(0, 0, -1), //SOUTH
        new Vector3Int(-1, 0, 0) //WEST
    };
} 