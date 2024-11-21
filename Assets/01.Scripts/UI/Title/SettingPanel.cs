using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UI.TitleScene
{

    public class SettingPanel : MovePanel
    {
        [SerializeField] private AudioMixer _auduoMixer;
        [SerializeField] private Slider _bgmSlider;
        [SerializeField] private Slider _sfxSlider;

        protected override void Awake()
        {
            base.Awake();
            _bgmSlider.onValueChanged.AddListener(HandleBGMValueChanged);
            _sfxSlider.onValueChanged.AddListener(HandleSFXValueChanged);
        }

        private void HandleBGMValueChanged(float volume)
        {
            // input=> 0 ~ -40    -40은 -80으로 강제조정
            if(volume <= -40) volume = -80f;
        }

        private void HandleSFXValueChanged(float volume)
        {
            if(volume <= -40) volume = -80f;

        }
    }

}