using Unity.VisualScripting;
using UnityEngine;
namespace AgentManage.Enemys
{


    public class EnemyState 
    {
        protected Enemy _owner;

        public EnemyState(Enemy enemy)
        {

        }

        public virtual void Enter()
        {
            
        }

        public virtual void UpdateState()
        {

        }

        public virtual void Exit()
        {

        }

    }

}