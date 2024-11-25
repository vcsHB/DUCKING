using BuildingManage;
using InputManage;
using System;
using UnityEngine;

public class TutorialGoal : MonoBehaviour
{
    [SerializeField] private GameObject _moveGoal;
    [SerializeField] private UIInputReaderSO _uiInput;
    [SerializeField] private BuildController _buildController;
    private Collider2D _collider;
    public Action OnMoveGoal;
    public Action OnSpaceBarGoal;
    public Action OnBuildGoal;
    private bool _spaceInput = false;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    public void SetMoveGoal()
    {
        _moveGoal.SetActive(true);
        _collider.enabled = true;
    }

    public void SetSpaceBarGoal()
    {
        _uiInput.SetControl(true);
        _uiInput.BuildEvent += OnSpaceBar;
    }

    public void SetBuildGoal()
    {
        _uiInput.SetControl(true);
        _buildController.OnBuildingChange += OnBuild;
    }

    private void OnSpaceBar()
    {
        RemoveSpaceEvt();
        _uiInput.SetControl(false);
        OnSpaceBarGoal?.Invoke();
    }

    private void OnBuild()
    {
        RemoveBuildEvt();
        OnBuildGoal?.Invoke();
    }

    private void RemoveBuildEvt()
    {
        _buildController.OnBuildingChange -= OnBuild;
    }

    private void RemoveSpaceEvt()
    {
        _uiInput.BuildEvent -= OnSpaceBar;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        OnMoveGoal?.Invoke();
        _collider.enabled = false;
        _moveGoal.SetActive(false);

    }
}
