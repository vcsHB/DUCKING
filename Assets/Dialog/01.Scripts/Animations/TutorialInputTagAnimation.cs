using InputManage;
using System;
using UnityEngine;

 namespace Dialog
{
    public class TutorialInputTagAnimation : TagAnimation
    {
        [SerializeField] private PlayerInputSO _playerInput;

        [SerializeField] private InputType InputType;
        [SerializeField] private bool _state;

        public TutorialInputTagAnimation()
        {
            _timing = AnimTiming.Start;
            tagType = TagEnum.TutorialInput;
            _checkEndPos = false;
        }

        public override void Init()
        {
            base.Init();
            _playerInput.SetControl(_state);
        }

        public override void Complete()
        {

        }

        public override void Play()
        {

        }

        public override bool SetParameter()
        {
            return true;
        }
    }

    [Serializable]
    public enum InputType
    {
        Movement,
        //¹¹ ´õ Ãß°¡ ÇÒ¼öµµ ÀÖÀÝ¾Æ?
    }
}