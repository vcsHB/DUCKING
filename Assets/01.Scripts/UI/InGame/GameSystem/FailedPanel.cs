using UnityEngine;
using UnityEngine.SceneManagement;
namespace UI.InGame.GameSystem
{

    public class FailedPanel : UIPanel
    {
        
        public void HandleMoveToTitle()
        {
            SceneManager.LoadScene("TitleScene");
        }
    }
}