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

    public event Action<Transform,int,bool> onMove;
    public event Action<Transform,int> onOil;
    public event Action<Transform,int> onCrash;

    public  void onMovePerformed(Transform trans,int id,bool selfCommand)
    {
        if (onMove != null)
        {
            onMove(trans,id,selfCommand);
        }
    }
    public void onOilPerformed(Transform trans,int id)
    {
        if (onOil != null)
        {
            onOil(trans,id);
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
