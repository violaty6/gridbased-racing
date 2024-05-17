using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class WorldSpaceUI : MonoBehaviour
{

    void LateUpdate()
    {
        var rotation = Camera.main.transform.rotation;
        transform.LookAt(transform.position + rotation * Vector3.forward, rotation * Vector3.up);
    }
}
