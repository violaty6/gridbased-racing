using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ErrorManager : MonoBehaviour
{
    [SerializeField] private GameObject ErrorFeedbackGameOBJ;
    private Sequence ErrorSequence;
    void Start()
    {
        GameEvents.current.onError += ErrorFeedback;
        ErrorSequence = DOTween.Sequence();
    }

    void ErrorFeedback(Vector3 transform, int id)
    {
        ErrorSequence.Kill();
        ErrorSequence = DOTween.Sequence();
        ErrorFeedbackGameOBJ.transform.localScale = Vector3.one;
        ErrorSequence.Append(ErrorFeedbackGameOBJ.transform.DOShakeScale(0.1f));
        ErrorSequence.Append(ErrorFeedbackGameOBJ.transform.DOScale(0f, 0.5f).SetEase(Ease.InBack));
        ErrorSequence.Append(DOVirtual.DelayedCall(1f, () => { ErrorFeedbackGameOBJ.SetActive(false); }));
        ErrorFeedbackGameOBJ.transform.position = transform;
        ErrorFeedbackGameOBJ.SetActive(true);
    }
}
