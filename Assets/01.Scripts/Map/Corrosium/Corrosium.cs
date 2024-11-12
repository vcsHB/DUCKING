using AgentManage;
using UnityEngine;

public class Corrosium : MonoBehaviour
{
    [SerializeField] private int _damage = 3;
    [SerializeField] private float _damageCooltime; 
    private float _currentTime =0;
    
    
    private void Update()
    {
        _currentTime += Time.deltaTime;
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (_currentTime > _damageCooltime)
        {
            _currentTime = 0;
            if (other.transform.TryGetComponent(out ICorrosive corrosive))
            {
                corrosive.Corrode(_damage);
            }
        }
    }
}
