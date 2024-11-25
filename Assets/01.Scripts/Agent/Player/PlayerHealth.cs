using UnityEngine;
namespace AgentManage.PlayerManage
{

    public class PlayerHealth : CorrosiveHealth
    {
        [SerializeField] private float _damagedTime = 10f;
        [SerializeField] private int _healAmountPerSecond = 3;

        private float _recentDamagedTime = 0;
        private float _currentTime = 0;



        public override void ApplyDamage(int amount)
        {
            base.ApplyDamage(amount);
            _recentDamagedTime = Time.time;
        }

        public override void Corrode(int power)
        {
            base.Corrode(power);
            _recentDamagedTime = Time.time;
        }


        private void Update()
        {

            if (Time.time - _recentDamagedTime > _damagedTime)
            {
                _currentTime += Time.deltaTime;
                if (_currentTime > 1f)
                {
                    _currentTime = 0;
                    if (_corrosionResistance < MaxCorrosionResistance)
                    {
                        RestoreCorrosion(_healAmountPerSecond);
                    }
                    else if (CurrentHealth < MaxHealth)
                    {
                        RestoreHealth(_healAmountPerSecond);
                    }
                }
            }
        }

    }

}