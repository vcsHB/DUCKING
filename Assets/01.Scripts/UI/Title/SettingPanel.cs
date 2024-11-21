using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace UI.TitleScene
{

    public class SettingPanel : MovePanel
    {
        [SerializeField] private AudioMixer _audioMixer;
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

            _audioMixer.SetFloat("Volume_BGM", volume);
        }

        private void HandleSFXValueChanged(float volume)
        {
            if(volume <= -40) volume = -80f;

            _audioMixer.SetFloat("Volume_SFX", volume);

        }

        private void HandleSaveSoundData()
        {
            // 나중에 저장 시스템을 만들어서 
            
        }
    }

}