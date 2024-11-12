using ObjectPooling;
using SoundManage;
using UnityEngine;
using UnityEngine.Events;
namespace FeedbackSystem
{

    public class SoundFeedback : Feedback
    {

        [SerializeField] private SoundSO _soundSO;

        public override void CreateFeedback()
        {
            //PoolManager.Instance.Pop(PoolingType.)

        }

        public override void FinishFeedback()
        {
        }
    }
}