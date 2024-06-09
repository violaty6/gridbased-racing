using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class FinishNodeSocket : MonoBehaviour,INode
{
    [SerializeField] private SpriteRenderer ParkIcon;
    public void Init()
    {
        ParkIcon.transform.DOLocalMoveY(ParkIcon.transform.localPosition.y + 0.15f, 4f).SetLoops(-1,LoopType.Yoyo).SetEase(Ease.InOutSine);
    }

    public void Interact(Node fromNode, Node toNode,IObject interactOwner)
    {
        Vector2 direction = new Vector2(-transform.right.x,-transform.right.z);
        if (direction == interactOwner.forward && !interactOwner.isMove)
        {
            if (interactOwner.gameObject.GetComponent<UnitController>() !=null)
            {
                interactOwner.gameObject.GetComponent<UnitController>().isCrashed = true;
                DOVirtual.DelayedCall(0.4f, () =>
                {
                    ParkIcon.DOColor(new Color(0, 1, 0, 1), 1f).SetEase(Ease.InCirc).OnComplete(() =>
                    {
                        GameEvents.current.onLevelCompletePerformed(0);
                    });
                    ParkIcon.transform.DOMoveY(4f,5f).SetEase(Ease.InCirc);
                });
            }
        }
        else
        {
            ParkIcon.DOColor(new Color(1, 0, 0, 1), 0.25f).SetEase(Ease.InCirc);
        }
    }
    public Node PredictInteraction(Node fromNode, Node toNode)
    {
        return toNode;
    }

    public void UnInteract(IObject interactOwner)
    {
        ParkIcon.DOColor(new Color(1, 1, 1, 1), 0.25f).SetEase(Ease.InCirc);
    }
}
