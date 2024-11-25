using System.IO;
using SaveSystem;
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

        private string _path;

        protected override void Awake()
        {
            base.Awake();
            _bgmSlider.onValueChanged.AddListener(HandleBGMValueChanged);
            _sfxSlider.onValueChanged.AddListener(HandleSFXValueChanged);

            Load();
        }

        private void HandleBGMValueChanged(float volume)
        {
            // input=> 0 ~ -40    -40은 -80으로 강제조정
            if (volume <= -40) volume = -80f;

            _audioMixer.SetFloat("Volume_BGM", volume);
        }

        private void HandleSFXValueChanged(float volume)
        {
            if (volume <= -40) volume = -80f;

            _audioMixer.SetFloat("Volume_SFX", volume);

        }

        public void Save()
        {
            AudioSave save = new AudioSave();
            save.bgm_volume = _bgmSlider.value;
            save.sfx_volume = _sfxSlider.value;

            string json = JsonUtility.ToJson(save, true);
            File.WriteAllText(_path, json);
        }

        public void Load()
        {
            _path = Path.Combine(Application.dataPath, "Saves/AudioSetting.json");
            //_path = Application.dataPath + "/Saves/AudioSetting.json";

            if (!File.Exists(_path))
            {
                Save();
                return;
            }

            string json = File.ReadAllText(_path);
            AudioSave save = JsonUtility.FromJson<AudioSave>(json);

            HandleBGMValueChanged(save.bgm_volume);
            _bgmSlider.value = save.bgm_volume;

            HandleSFXValueChanged(save.bgm_volume);
            _sfxSlider.value = save.sfx_volume;



        }
    }

}