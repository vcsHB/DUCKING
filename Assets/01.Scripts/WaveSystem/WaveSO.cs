using ObjectPooling;
using UnityEngine;

namespace WaveSystem
{

    [CreateAssetMenu(menuName = "WaveSystem/WaveSO")]
    public class WaveSO : ScriptableObject
    {
        
        public WaveInfo[] waveInfos;

    }
}
