using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;
    private void Awake()
    {
        current = this;
    }

    public event Action<Transform> onMove;
    public event Action<Transform> onCrash;

    public  void onMovePerformed(Transform trans)
    {
        if (onMove != null)
        {
            onMove(trans);
        }
    }

    public void onCrashPerformed(Transform trans)
    {
        if (onCrash != null)
        {
            onCrash(trans);
        }
    }
}
