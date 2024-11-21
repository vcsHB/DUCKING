using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace UI.TitleScene
{
    public class StageSelectPanel : MovePanel
    {

        [SerializeField] private TextMeshProUGUI _stageNameText;
        [SerializeField] private TextMeshProUGUI _stageDescriptionText;
        [SerializeField] private TextMeshProUGUI _stageOxidationText;
        


        public void HandleStartStage()
        {
            SceneManager.LoadScene("VCSScene");

        }

    }
}