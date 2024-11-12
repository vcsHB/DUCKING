using UnityEngine;
using UnityEngine.EventSystems;

namespace FeedbackSystem
{
    public abstract class Feedback : MonoBehaviour
    {
        
        public abstract void CreateFeedback();
        
        public abstract void FinishFeedback();

    }

}