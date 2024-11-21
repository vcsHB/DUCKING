using AgentManage;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using DG.Tweening;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Transform _pivot;
    [SerializeField] private Transform _subpivot;
    [SerializeField] private Health _health;

    private Sequence _seq;

    public void OnDamaged()
    {
        float targetRatio = (float)_health.CurrentHealth / (float)_health.MaxHealth;

        _seq = DOTween.Sequence();

        if(_seq != null && !_seq.active)
            _seq.Kill();

        _seq.Append(_pivot.DOScaleX(targetRatio, 0.1f))
            .Append(_subpivot.DOScaleX(targetRatio, 0.2f));
    }
}
