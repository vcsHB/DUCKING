using UnityEngine;
namespace UI.InGame.GameSystem
{

    public class UIManager : MonoSingleton<UIManager>
    {
        [SerializeField] private SuccessPanel _successPanel;
        [SerializeField] private FailedPanel _failedPanel;


        public void ShowSuccessPanel()
        {
            _successPanel.Open();
        }

        public void ShowFailedPanel()
        {
            _failedPanel.Open();
        }
        
    }
}