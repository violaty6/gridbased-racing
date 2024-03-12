using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class GridManager : MonoBehaviour
{
    [SerializeField]private SerializedDictionary<Vector3Int, Node> gridTileDict;
    // public List<Vector3Int> objectscord;
    // public List<Node> objects;
    private void Awake()
    {
        gridTileDict = new SerializedDictionary<Vector3Int, Node>();
        foreach (var node in FindObjectsOfType<Node>())
        {
            gridTileDict.Add(node.cords,node);
        }
        // objects = gridTileDict.Values.ToList();
        // objectscord = gridTileDict.Keys.ToList();
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