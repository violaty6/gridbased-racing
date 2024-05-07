using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SteeringWheelUI : MonoBehaviour
{
    private void Start()
    {
        GameEvents.current.onDirectionSwitch += SteerFeedback;
    }

    private void SteerFeedback(int id , Vector2Int direction)
    {
        if (direction == new Vector2Int(0,1))
        {
            transform.DOLocalRotate(new Vector3(0, 0, 0), 1f).SetEase(Ease.OutQuart);
        }
        if (direction == new Vector2Int(1,0) || direction == new Vector2Int(-1,0))
        {
           transform.DOLocalRotate(new Vector3(0, 0, -89 * direction.x), 1f).SetEase(Ease.OutQuart);
        }
    }

    private void OnDestroy()
    {
        GameEvents.current.onDirectionSwitch -= SteerFeedback;
    }
}
