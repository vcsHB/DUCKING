using AgentManage;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IAgentComponent
{
    private Rigidbody2D _rigid;
    private Player _player;

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
