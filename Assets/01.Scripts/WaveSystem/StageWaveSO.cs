namespace WaveSystem
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "WaveSystem/StageWaveSO")]
    public class StageWaveSO : ScriptableObject
    {
        public WaveSO[] waves;
        
    }
}