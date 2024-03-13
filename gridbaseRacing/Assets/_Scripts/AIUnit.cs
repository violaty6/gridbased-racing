using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIUnit : MonoBehaviour
{
    
    void Start()
    {
        GameEvents.current.onMove += onPlayerMove;
    }

    private void onPlayerMove()
    {
        
    }
}
