namespace AgentManage.Enemies
{

    public class EnemyAnimator : AgentAnimator
    {
        private Enemy _enemy;

        public override void Initialize(Agent agent)
        {
            base.Initialize(agent);
            _enemy = agent as Enemy;
        }
        public override void AfterInit()
        {
            base.AfterInit();
            _enemy.GetCompo<EnemyMovement>().OnMovementEvent += HandleMovement;

        }

        private void OnDestroy() {
            _enemy.GetCompo<EnemyMovement>().OnMovementEvent -= HandleMovement;
        }
    }

}