using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node: MonoBehaviour
{
    public Vector3Int cords;
    public enum NodeType
    {
        Default,
        Obstacle,
        Finish
    }
    private void Awake()
    {
        cords.x = Mathf.RoundToInt(transform.position.x);
        cords.y = Mathf.RoundToInt(transform.position.y);
        cords.z = Mathf.RoundToInt(transform.position.z);
    }
}
