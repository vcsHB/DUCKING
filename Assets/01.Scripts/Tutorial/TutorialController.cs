using Dialog;
using InputManage;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private DialogPlayer _dialogPlayer;
    [SerializeField] private List<DialogSO> _dialogSO;
    [SerializeField] private TutorialGoal _goal;
    private int _curIdx = 0;

    [SerializeField] private UIInputReaderSO _uiInput;
    [SerializeField] private PlayerInputSO _playerInput;
    private bool _isPlaying = false;

    //�ϴ� �ӽ÷� �ϴ� ����
    private void OnEnable()
    {
        if (!PlayerPrefs.HasKey("Tutorial"))
        {
            PlayDialog();
            PlayerPrefs.SetInt("Tutorial", 0);
        }
    }

    [ContextMenu("ResetTutorial")]
    public void ResetTutorial()
    {
        PlayerPrefs.DeleteKey("Tutorial");
    }

    public void PlayDialog()
    {
        if (_isPlaying) return;

        _isPlaying = true;
        _playerInput.SetControl(false);
        _uiInput.SetControl(false);
        _dialogPlayer.OnCompleteDialog += OnCompleteDialog;

        _dialogPlayer.dialog = _dialogSO[_curIdx++];
        _dialogPlayer.StartDialog();
    }

    private void OnCompleteDialog()
    {
        _playerInput.SetControl(true);
        _isPlaying = false;

        switch (_curIdx)
        {
            case 1:
                _goal.SetMoveGoal();
                _goal.OnMoveGoal += OnMoveComplete;
                break;
            case 2:
                _goal.SetSpaceBarGoal();
                _goal.OnSpaceBarGoal += OnSpaceBar;
                break;
            case 3:
                _goal.SetBuildGoal();
                _goal.OnBuildGoal += OnBuild;
                break;

            case 4:
                _uiInput.SetControl(true);
                _playerInput.SetControl(true);
                break;
        }
    }

    private void OnMoveComplete()
    {
        _goal.OnMoveGoal -= OnMoveComplete;
        PlayDialog();
    }

    private void OnSpaceBar()
    {
        _goal.OnSpaceBarGoal -= OnSpaceBar;
        PlayDialog();
    }

    private void OnBuild()
    {
        _goal.OnBuildGoal -= OnBuild;
        PlayDialog();
    }
}
