using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public Dictionary<Vector3Int, Node> gridTileDict = new Dictionary<Vector3Int, Node>();
    private void Start()
    {
        foreach (var node in FindObjectsOfType<Node>())
        {
            gridTileDict[node.cords] = node;
        }
    }

    public Node GetTileAt(Vector3Int cords)
    {
        Node result = null;
        gridTileDict.TryGetValue(cords, out result);
        return result;
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