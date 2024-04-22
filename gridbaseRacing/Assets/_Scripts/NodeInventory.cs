using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
[CreateAssetMenu(fileName = "New Node Inventory")]
public class NodeInventory : ScriptableObject
{
    [SerializeField]public SerializedDictionary<int, NodeType> nodeInventory;
}
