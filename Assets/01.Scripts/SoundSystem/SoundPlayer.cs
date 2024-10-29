using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundManage
{

    public class SoundPlayer : MonoBehaviour
    {
        // 나중에 풀링 처리를 해주어야 함
        
        [SerializeField] private AudioMixerGroup _sfxGroup, _musicGroup;

        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }


        public void PlaySound(SoundSO data)
        {
            if (data.audioType == AudioType.SFX)
            {
                _audioSource.outputAudioMixerGroup = _sfxGroup;
            }
            else if (data.audioType == AudioType.BGM)
            {
                _audioSource.outputAudioMixerGroup = _musicGroup;
            }

            _audioSource.volume = data.volume;
            _audioSource.pitch = data.pitch;
            if (data.randomizePitch)
            {
                _audioSource.pitch += Random.Range(-data.randomPitchModifier, data.randomPitchModifier);
            }
            _audioSource.clip = data.clip;

            _audioSource.loop = data.loop;

            if (!data.loop)
            {
                float time = _audioSource.clip.length + .2f;
                StartCoroutine(DisableSoundTimer(time));
            }
            _audioSource.Play();
        }

        private IEnumerator DisableSoundTimer(float time)
        {
            yield return new WaitForSeconds(time);
            //this.Push();
        }
    }

}