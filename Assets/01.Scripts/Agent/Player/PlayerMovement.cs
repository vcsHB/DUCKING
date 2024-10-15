using System;
using System.Collections;
using System.Collections.Generic;
using AgentManage;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IAgentComponent
{
    private Rigidbody2D _rigid;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        
    }

    public void Initialize(Agent agent)
    {
        
    }

    public void AfterInit()
    {
    }

    public void Dispose()
    {
    }
}
