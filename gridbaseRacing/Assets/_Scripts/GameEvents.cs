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

    public event Action<Transform,int> onMove;
    public event Action<Transform,int> onCrash;

    public  void onMovePerformed(Transform trans,int id)
    {
        if (onMove != null)
        {
            onMove(trans,id);
        }
    }

    public void onCrashPerformed(Transform trans,int id)
    {
        if (onCrash != null)
        {
            onCrash(trans,id);
        }
    }
}
