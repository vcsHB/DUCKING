using Dialog;
using InputManage;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    private string _path;
    private int _currentTutorialProgress = 0;

    [SerializeField] private DialogPlayer _dialogPlayer;
    [SerializeField] private List<DialogSO> _dialogSO;
    [SerializeField] private TutorialGoal _goal;
    private int _curIdx = 0;

    [SerializeField] private UIInputReaderSO _uiInput;
    [SerializeField] private PlayerInputSO _playerInput;
    private bool _isPlaying = false;


    private void OnEnable()
    {
        _path = Path.Combine(Application.dataPath, "Saves/Tutorial.json");

        if (!File.Exists(_path))
        {
            File.WriteAllText(_path, "1");
            PlayDialog();
        }
        else
        {
            string data = File.ReadAllText(_path);
            int.TryParse(data, out _currentTutorialProgress);

            if (_currentTutorialProgress == 0)
            {
                File.WriteAllText(_path, "1");
                PlayDialog();
            }
        }
    }

    [ContextMenu("ResetTutorial")]
    public void ResetTutorial()
    {
        File.WriteAllText(_path, "0");
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
        RemoveMoveGoal();
        PlayDialog();
    }

    private void RemoveMoveGoal() => _goal.OnMoveGoal -= OnMoveComplete;

    private void OnSpaceBar()
    {
        RemoveSpaceBarGoal();
        PlayDialog();
    }
    private void RemoveSpaceBarGoal() => _goal.OnSpaceBarGoal -= OnSpaceBar;

    private void OnBuild()
    {
        RemoveBuildGoal();
        PlayDialog();
    }

    private void RemoveBuildGoal() => _goal.OnBuildGoal -= OnBuild;
}
