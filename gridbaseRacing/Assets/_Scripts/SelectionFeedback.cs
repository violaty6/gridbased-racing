using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SelectionFeedback : MonoBehaviour
{
    void Start()
    {
        transform.DOScale(0.45f,1f).SetLoops(-1,LoopType.Yoyo);
    }
}
