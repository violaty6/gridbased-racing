using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
public class NodeManager : MonoBehaviour
{
    private static NodeManager instance = null;
    public static NodeManager Instance
    {
        get
        {
            if (instance == null) instance = (NodeManager)FindObjectOfType(typeof(NodeManager));
            return instance;
        }
    }
    [SerializeField]public SerializedDictionary<int, NodeType> nodeInventory;
    private void Awake()
    {
        instance = this;
    }
}