using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
public class GridManager : MonoBehaviour
{
    public static GridManager Instance { get; private set; }

    [SerializeField]
    private SerializedDictionary<Vector3Int, Node> gridTileDict;
    public Node FinishLine;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        gridTileDict = new SerializedDictionary<Vector3Int, Node>();
        foreach (var node in FindObjectsOfType<Node>())
        {
            if (!gridTileDict.ContainsKey(node.cords))
            {
                gridTileDict.Add(node.cords, node);
            }
            else
            {
                Debug.LogWarning("Duplicate node coordinates found: " + node.cords.ToString());
            }
        }
    }

    private void OnDestroy()
    {
        gridTileDict.Clear();
    }

    public Node GetTileAt(Vector3Int cords)
    {
        Node result = null;
        gridTileDict.TryGetValue(cords, out result);
        return result;
    }
    public Vector2 GetDirectionNodeToNode(Node fromNode, Node toNode)
    {
        return new Vector2((toNode.cords - fromNode.cords ).x,(toNode.cords - fromNode.cords).z);
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
             foreach (var neighbour in GetNeighbours(current , openNodes.Count))
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
             currentNode = currentNode._parent;
         }
         path.Reverse();
         return path;
     }
     public Node PredictCheck(Node fromNode,Node targetNode)
     {
         bool isFinish = false;
         Node target = targetNode;
         Node from = fromNode;
         Node result = target.PredictInteraction(from,target);
         isFinish = true;
         while (target != result && isFinish)
         {
             isFinish = false;
             from = target;
             target = result;
             result = result.PredictInteraction(from, target);
             isFinish = true;
         }
         return result;
     }

     public bool CanUnitMove(IObject unit, Node curNode,Node targetNode,Vector2 unitDirection)
     { 
         if (targetNode.limitedDir == unitDirection || curNode.limitedDir == -unitDirection) {return false;}
         else if (targetNode.currentTag == Node.NodeTag.Obstacle) return true;
         else if (targetNode.currentTag == Node.NodeTag.Void) return false;
         else return true;
     }

      public List<Node> OneDirectionToLast(Node startNode ,Vector2 direction)
     {
          List<Node> resultList = new List<Node>{};
          Node from = startNode;
          Node target = GetOneNodeOneDirection(startNode, direction);
          if (target == null || target.currentTag == Node.NodeTag.Obstacle || target.currentTag == Node.NodeTag.Void || target.onNodeObject !=null) return resultList;
          Node result =  PredictCheck(from, target);
          if (result != target)
          {
              resultList.Add(target);
              return resultList;
          }
          resultList.Add(result);
          while (target != null || result==target)
          {
            from = target;
            target = GetOneNodeOneDirection(from, direction);
            if (target == null|| target.currentTag == Node.NodeTag.Obstacle || target.currentTag == Node.NodeTag.Void || target.onNodeObject !=null) break;
            result = PredictCheck(from, target);
            if (result != target)
            {
                resultList.Add(target);
                break;
            }
            resultList.Add(result);
          }
          return resultList;
      }

     public Node GetOneNodeOneDirection(Node startNode ,Vector2 moveDirection)
     {
         Vector3Int moveDirectionInt = new Vector3Int(Mathf.RoundToInt(moveDirection.x), 0, Mathf.RoundToInt(moveDirection.y));
         Vector3Int targetCord = startNode.cords + moveDirectionInt;
         Node nextNode = GetTileAt(targetCord);
         return nextNode;
     }
     
     private List<Node> GetNeighbours(Node node , int index)
     {
         Debug.Log(index);
         List<Node> neighbours = new List<Node>();
         for (int i = 0; i < 4 ; i++)
         {
             Node nextNode = GetTileAt(node.cords +( Direction.directionsOffset[i]));
             if (nextNode == null) continue;
             nextNode = PredictCheck(node, nextNode);
             if (nextNode == null || nextNode.currentTag == Node.NodeTag.Obstacle || nextNode.currentTag == Node.NodeTag.Void || (nextNode.onNodeObject !=null && index !=1 )) continue;
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