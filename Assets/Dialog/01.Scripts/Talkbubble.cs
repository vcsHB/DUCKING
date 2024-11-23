using TMPro;
using UnityEngine;

namespace Dialog
{
    [RequireComponent(typeof(AnimationPlayer))]
    public class Talkbubble : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _tmp;
        [SerializeField] private GameObject _talkBubbleObj;

        private AnimationPlayer _player;
        private DialogPlayer _dialogPlayer;
        private NodeSO _node;


        private void LateUpdate()
        {
            if (_node == null) return;
            _player.PlayAnimation(_tmp, _node.GetAllAnimations());
        }

        public void SetTalkbubble(NodeSO node)
        {
            if (node is NormalNodeSO normalNode)
            {
                _node = node;
                _tmp.SetText(normalNode.GetContents());
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        public void TurnOffTalkbubble()
        {
            gameObject.SetActive(false);
            _node = null;
            _tmp.SetText("");
        }

        public void Init(DialogPlayer player)
        {
            _dialogPlayer = player;

            _player = GetComponent<AnimationPlayer>();
            _player.Init(player);
        }
    }
}
