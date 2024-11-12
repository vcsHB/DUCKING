using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FeedbackSystem
{

    public class FeedbackPlayer : MonoBehaviour
    {
        private Feedback[] _feedbacks;
        private void Awake() {
            _feedbacks = GetComponents<Feedback>();
        }

        public void CreateFeedback()
        {
            for (int i = 0; i < _feedbacks.Length; i++)
            {
                _feedbacks[i].CreateFeedback();
            }
        }

        public void StopFeedback()
        {
            for (int i = 0; i < _feedbacks.Length; i++)
            {
                _feedbacks[i].FinishFeedback();
            }
        }
    }

}
