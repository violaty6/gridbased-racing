using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Node Type")]
public class NodeType : ScriptableObject
{
    public Node.NodeTag tag;
    public string typeName;
    public GameObject NodeTypeGameObject;
}
public interface INode
{
    public void Init();
    public void Interact(Node fromNode, Node toNode,IObject interactOwner);
    public Node PredictInteraction(Node fromNode, Node toNode);
    public void UnInteract(IObject interactOwner);
}
