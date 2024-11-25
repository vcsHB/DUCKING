using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.InGame.GameSystem
{
    public class SuccessPanel : UIPanel
    {
        
        public void HandleMoveToTitle()
        {
            SceneManager.LoadScene("TitleScene");
        }
    }


}