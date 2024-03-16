using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class GridManager : MonoBehaviour
{
    [SerializeField]private SerializedDictionary<Vector3Int, Node> gridTileDict;
    public Node FinishLine;
    private void Awake()
    {
        gridTileDict = new SerializedDictionary<Vector3Int, Node>();
        foreach (var node in FindObjectsOfType<Node>())
        {
            gridTileDict.Add(node.cords,node);
        }
    }
    public Node GetTileAt(Vector3Int cords)
    {
        Node result = null;
        gridTileDict.TryGetValue(cords, out result);
        return result;
    }

     public List<Node> PathNodes(Node startNode , Node targetNode, int power)
     {
         List<Node> path = null;
         List<Node> openNodes = new List<Node>();
         HashSet<Node> closedNodes = new HashSet<Node>();
         openNodes.Add(startNode);
         while (openNodes.Count > 0)
         {
             Node current = openNodes[0];
             for (int i = 1; i < openNodes.Count; i++)
             {
                 if (openNodes[i].f_cost<current.f_cost || openNodes[i].f_cost  == current.f_cost && openNodes[i].h_cost < current.h_cost)
                 {
                     current = openNodes[i];
                 }
             }
             openNodes.Remove(current);
             closedNodes.Add(current);
             if (current == targetNode)
             {
                 path =  RetracePath(startNode,targetNode);
                 return path;
             }
             foreach (var neighbour in GetNeighbours(current , power))
             {
                 if (neighbour == null || closedNodes.Contains(neighbour))
                 {
                     continue;
                 }
                 float newMovementCostToNeighbour = current.g_cost + Distance(current.cords,neighbour.cords);
                 if (newMovementCostToNeighbour < neighbour.g_cost || !openNodes.Contains(neighbour))
                 {
                     neighbour.g_cost = newMovementCostToNeighbour;
                     neighbour.h_cost = Distance(neighbour.cords, targetNode.cords);
                     neighbour._parent = current;
                     if (!openNodes.Contains(neighbour))
                     {
                         openNodes.Add(neighbour);
                     }
                 }
             }
         }
         return path;
     }

     List<Node> RetracePath(Node startNode, Node targetNode)
     {
         List<Node> path = new List<Node>();
         Node currentNode = targetNode;
         while (currentNode != startNode)
         {
             path.Add(currentNode);
             currentNode.transform.GetChild(0).DOMoveY(0.1f, 0.1f);
             currentNode = currentNode._parent;
         }
         path.Reverse();
         return path;
     }

     public List<Node> OneDirectionToLast(Node startNode ,Vector3Int direction)
     {
         List<Node> result = new List<Node>{startNode};
         while (GetOneNodeOneDirection(result.Last(),direction) !=null)
         {
             result.Add(GetOneNodeOneDirection(result.Last(),direction));
         }

         return result;
     }

     private Node GetOneNodeOneDirection(Node startNode ,Vector3Int direction)
     {
         Node nextNode = GetTileAt(startNode.cords +direction);
         if (nextNode == null || nextNode.currentType == Node.NodeType.Obstacle) return null;
         return nextNode;
     }
     
     private List<Node> GetNeighbours(Node node , int power)
     {
         List<Node> neighbours = new List<Node>();
         for (int i = 0; i < 4 ; i++)
         {
             Node nextNode = GetTileAt(node.cords +( Direction.directionsOffset[i] * power));
             if (nextNode == null || nextNode.currentType == Node.NodeType.Obstacle) continue;
             neighbours.Add(nextNode);
         }
         return neighbours;
     }
     
    public static float Distance(Vector3 a, Vector3 b) {
        Vector3 vector = new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        return Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);
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
    public static Vector3Int GetCords(Vector3 pos)
    {
        Vector3Int result = new Vector3Int(0, 0, 0);
        result.x = Mathf.RoundToInt(pos.x);
        result.y = Mathf.RoundToInt(pos.y);
        result.z = Mathf.RoundToInt(pos.z);
        return result;
    }
} 