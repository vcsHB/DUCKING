using AgentManage;
using InputManage;
using UnityEngine;

public class Player : Agent
{
    [field: SerializeField] public PlayerInputSO PlayerInput { get; private set; }
    public PlayerMovement MovementCompo { get; protected set; }

    protected override void Awake()
    {
        base.Awake();
        MovementCompo = GetComponent<PlayerMovement>();
        
        
    }
    
    
    
}
