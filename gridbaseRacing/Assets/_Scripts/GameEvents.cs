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

    public event Action<int> onMove;
    public event Action<Transform,int> onSmoke;
    public event Action<Transform,int> onOil;
    public event Action<Transform,int> onCrash;
    public event Action<Vector3,int> onError;
    public event Action<int,bool> onCameraSwitch;
    public event Action<int,int> onReverseSwitch;
    public event Action<int> onGasolineOut;
    public event Action<int,Vector2Int> onDirectionSwitch;

    public  void onMovePerformed(Transform trans,int id)
    {
        if (onMove != null)
        {
            onMove(id);
        }
    }
    public  void onGasolineOutPerformed(int id)
    {
        if (onGasolineOut != null)
        {
            onGasolineOut(id);
        }
    }
    
    public  void onCameraSwitchPerformed(int id,bool isUnit)
    {
        if (onCameraSwitch != null)
        {
            onCameraSwitch(id,isUnit);
        }
    }
    public  void onReverseSwitchPerformed(int id,int isUp)
    {
        if (onReverseSwitch != null)
        {
            onReverseSwitch(id,isUp);
        }
    }
    public  void onDirectionSwitchPerformed(int id,Vector2Int direction)
    {
        if (onDirectionSwitch != null)
        {
            onDirectionSwitch(id,direction);
        }
    }

    public void onSmokePerformed(Transform trans, int id)
    {
        if (onSmoke != null)
        {
            onSmoke(trans,id);
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
    public void onErrorPerformed(Vector3 trans,int id)
    {
        if (onError != null)
        {
            onError(trans,id);
        }
    }
}
