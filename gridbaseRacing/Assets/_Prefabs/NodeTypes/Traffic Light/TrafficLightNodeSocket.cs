using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class TrafficLightNodeSocket : MonoBehaviour,INode
{
    [SerializeField] private Node[] _effectedNodes;
    [SerializeField] private GameObject[] _lightBeams;
    [SerializeField] private List<MeshRenderer> _feedbackRenderers;
    [SerializeField] private Texture2D[] _emmisives;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private GameObject _feedback;
    private bool isRed = true;
    public void Init()
    {
        GameEvents.current.onMove += Change;
        ChangeFeedback(isRed);
        InitFeedbacks();
    }
    private void OnDisable()
    {
        GameEvents.current.onMove -= Change;
    }
    private void Change(int id)
    {
        isRed = !isRed;
        ChangeFeedback(isRed);
        ChangeNodes(isRed);
    }
    public void Interact(Node fromNode, Node toNode, IObject interactOwner)
    {
        
    }
    private void InitFeedbacks()
    {
        foreach (var node in _effectedNodes)
        {
            GameObject feedback = Instantiate(_feedback, node.cords, quaternion.identity, node.transform);
            _feedbackRenderers.Add(feedback.GetComponent<MeshRenderer>());
        }
    }
    private void ChangeNodes(bool isRed)
    {
        if (isRed)
        {
            foreach (var node in _effectedNodes)
            {
                node.currentTag = Node.NodeTag.Void;
            }
        }
        else
        {
            foreach (var node in _effectedNodes)
            {
                node.currentTag = Node.NodeTag.Default;
            }
        }
    }
    private void ChangeFeedback(bool isRed)
    {
        if (isRed)
        {
            _lightBeams[0].SetActive(true);
            _lightBeams[1].SetActive(false);
            _meshRenderer.material.SetTexture("_EmissionMap",_emmisives[0]);
            foreach (var F_renderer in _feedbackRenderers)
            {
                F_renderer.material.SetColor("_StripeColor",new Color(0.64f,0.39f,0.42f,1));
            }
        }
        else
        {
            _lightBeams[0].SetActive(false);
            _lightBeams[1].SetActive(true);
            _meshRenderer.material.SetTexture("_EmissionMap",_emmisives[1]);
            foreach (var F_renderer in _feedbackRenderers)
            {
                F_renderer.material.SetColor("_StripeColor",new Color(0.39f,0.64f,0.42f,1f));
            }
        }
    }
    public Node PredictInteraction(Node fromNode, Node toNode)
    {
       return toNode;
    }
    public void UnInteract(IObject interactOwner)
    {
        
    }
}
