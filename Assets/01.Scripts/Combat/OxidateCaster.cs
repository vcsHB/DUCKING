using AgentManage;
using UnityEngine;

namespace Combat
{

    public class OxidateCaster : MonoBehaviour, IAdditionalCaster
    {
        [SerializeField] private int _corrosivePower = 1;
        
        public void Cast(Collider2D target)
        {
            if(target.transform.TryGetComponent(out ICorrosive corrosive))
            {
                corrosive.Corrode(_corrosivePower);
            }
        }


    }

}