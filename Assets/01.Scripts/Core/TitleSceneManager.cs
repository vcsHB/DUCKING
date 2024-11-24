using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UI.TitleScene;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TitleScene
{

    public class TitleSceneManager : MonoBehaviour
    {
        [SerializeField] private FadePanel _fadePanel;

        private void Start() {
            _fadePanel.Close();
        }

        public void MoveToInGameScene()
        {
            _fadePanel.Open();
            DOVirtual.DelayedCall(1f, () => SceneManager.LoadScene("VCSScene"));
        }
    }

}