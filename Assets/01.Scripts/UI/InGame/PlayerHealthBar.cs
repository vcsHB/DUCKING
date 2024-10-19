using System;
using System.Collections;
using System.Collections.Generic;
using AgentManage;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame
{
    
    public class PlayerHealthBar : MonoBehaviour
    {
        [SerializeField] private Player _player;
        private Health _health;
        
        [SerializeField] private Image _fillImage;


        private void Start()
        {
            _health = _player.HealthCompo;
            _health.OnHealthChangeEvent += HandleHealthChange;
            HandleHealthChange(1,1);
        }

        private void HandleHealthChange(int currentValue, int maxValue)
        {
            _fillImage.fillAmount = ((float)currentValue / maxValue);

        }
    }

}