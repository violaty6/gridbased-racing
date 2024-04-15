using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;

[SelectionBase]
public class Node : MonoBehaviour,INode
{
    public float g_cost = 0;
    public float h_cost = 0;
    public float f_cost = 0;
    public Node _parent;
    
    [OdinSerialize]
    public IObject onNodeObject;
    public Vector3Int cords;
    public NodeTag currentTag;
    [SerializeField] private INode currentType;
    [SerializeField] private GameObject instObj;

    [OnValueChanged("ChangeNode")] [SerializeField][Range(0, 5)]
    private int currentNodeIndex;
    
    private int currentRot = 0;
    [Button(ButtonSizes.Gigantic)]
    private void Rotate90Degree()
    {
        currentRot++;
        if (currentRot == 4) currentRot = 0;
        transform.eulerAngles = new Vector3(0, 90*currentRot,0);
    }
    private void ChangeNode()
    {
        NodeType result = null;
        NodeManager.Instance.nodeInventory.TryGetValue(currentNodeIndex, out result);
        KillChild();
        currentTag = result.tag;
        instObj = PrefabUtility.InstantiatePrefab(result.NodeTypeGameObject,transform) as GameObject;
        currentType = instObj.GetComponent<INode>();
    }
    public enum NodeTag
    {
        Default,
        Obstacle,
        Void,
    }
    private void Awake()
    {
        cords.x = Mathf.RoundToInt(transform.position.x);
        cords.y = Mathf.RoundToInt(transform.position.y);
        cords.z = Mathf.RoundToInt(transform.position.z);
        currentTypeCheck();
    }
    private void KillChild()
    {
        Transform[] children = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            children[i] = transform.GetChild(i);
        }
        foreach (Transform child in children)
        {
            DestroyImmediate(child.gameObject);
        }
    }

    private void currentTypeCheck()
    {
        currentType = instObj.GetComponent<INode>();
    }

    public void Init()
    {
        currentType.Init();
    }
    public void Interact(Node fromNode, Node toNode,IObject interactOwner)
    {
        currentType.Interact(fromNode,toNode,interactOwner);
    }

    public Node PredictInteraction(Node fromNode, Node toNode)
    {
        return currentType.PredictInteraction(fromNode, toNode);
    }

    public void UnInteract(IObject interactOwner)
    {
        currentType.UnInteract(interactOwner);
    }
}