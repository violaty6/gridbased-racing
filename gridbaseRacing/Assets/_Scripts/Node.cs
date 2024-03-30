using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class Node : MonoBehaviour
{
    public float g_cost = 0;
    public float h_cost = 0;
    public float f_cost = 0;
    public Node _parent;
    public Vector3Int cords;
    public NodeType currentType;
    public Object specialSlot;
    public enum NodeType
    {
        Default,
        Obstacle,
        Void,
        Finish
    }
    private void Awake()
    {
        cords.x = Mathf.RoundToInt(transform.position.x);
        cords.y = Mathf.RoundToInt(transform.position.y);
        cords.z = Mathf.RoundToInt(transform.position.z);
    }
    
}