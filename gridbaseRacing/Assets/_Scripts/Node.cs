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
    
    public IObject onNodeObject;
    public Vector3Int cords;
    public NodeTag currentTag;
    [SerializeField] private INode currentType;
    [SerializeField] private GameObject instObj;
    [SerializeField] private NodeInventory nodeInv;



    [OnValueChanged("ChangeNode")] [SerializeField][Range(0, 7)]
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
        nodeInv.nodeInventory.TryGetValue(currentNodeIndex, out result);
        KillChild(transform.gameObject);
        currentTag = result.tag;
        this.name = result.name;
#if UNITY_EDITOR
        instObj = PrefabUtility.InstantiatePrefab(result.NodeTypeGameObject,transform) as GameObject;
#endif   
        currentType = instObj.GetComponent<INode>();
    }
    public enum NodeTag
    {
        Default,
        Obstacle,
        Void,
        Limited,
    }
    private void Awake()
    {
        cords.x = Mathf.RoundToInt(transform.position.x);
        cords.y = Mathf.RoundToInt(transform.position.y);
        cords.z = Mathf.RoundToInt(transform.position.z);
        currentTypeCheck();
    }

    private void Start()
    {
        Init();
    }

    private void KillChild(GameObject socket)
    {
        Transform[] children = new Transform[socket.transform.childCount];
        for (int i = 0; i < socket.transform.childCount; i++)
        {
            children[i] = socket.transform.GetChild(i);
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
    
    
    
     // LIMIT ****************************
     bool isLimited
     {
         get
         {
             if (currentTag == NodeTag.Limited) return true;
             else return false;
         }
     }

     [ShowIf("isLimited")][SerializeField] public Vector2 limitedDir;
     [ShowIf("isLimited")][SerializeField] private GameObject LimiterSocket;
     [ShowIf("isLimited")][SerializeField] private GameObject LimiterPrefab;
     
    [ShowIf("isLimited")]
    [Button(ButtonSizes.Small)]
    private void Clear()
    {
        limitedDir = new Vector2(0, 0);
    }
    [ShowIf("isLimited")]
    [Button(ButtonSizes.Small)]
    private void LimitDirection()
    {
#if UNITY_EDITOR
        
        if (limitedDir.y ==0)
        {
            LimiterSocket.transform.eulerAngles = (new Vector3(0,-limitedDir.x*90,0));
        }
        else if (limitedDir.x == 0)
        {
            float angle = limitedDir.y * 180;
            if (angle < 0) angle = 0;
            LimiterSocket.transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        }
#endif
    }
    [ShowIf("isLimited")]
    [Button(ButtonSizes.Small)]
    private void AddLimiter()
    {
#if UNITY_EDITOR
        LimiterSocket = PrefabUtility.InstantiatePrefab(LimiterPrefab,transform) as GameObject;
#endif
    }
}