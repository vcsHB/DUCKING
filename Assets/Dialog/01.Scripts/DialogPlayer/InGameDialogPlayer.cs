using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Dialog
{
    [RequireComponent(typeof(AnimationPlayer))]
    public class InGameDialogPlayer : DialogPlayer
    {
        private AnimationPlayer _animPlayer;    //�ִϸ��̼��� ���� ������ ģ��
        private TMP_TextInfo _txtInfo;          //�ִϸ����� �� �ؽ�Ʈ

        [SerializeField] private RectTransform _optionParent;
        [SerializeField] private List<IngameCharacterStruct> characters;

        private IngameCharacterStruct _curCharacter;

        private bool _optionSelected = false;
        private List<OptionButton> _optionBtns;
        private NodeSO _nextNode;

        private void Awake()
        {
            _animPlayer = GetComponent<AnimationPlayer>();
            _animPlayer.Init(this);
            characters.ForEach(c => c.talkbubble.Init(this));
        }

        private void Update()
        {
            //����׿�
            if (Input.GetKeyDown(KeyCode.Q))
            {
                StartDialog();
            }
        }

        #region DialogRead

        public override void StartDialog()
        {
            if (_isReadingDialog) Debug.Log("�̹� �������ε�~\n��~�� ��");

            _isReadingDialog = true;
            _curReadingNode = dialog.nodes[0];
            ReadSingleLine();
        }

        public override void EndDialog()
        {
            characters.ForEach((c) => c.talkbubble.TurnOffTalkbubble());
            _isReadingDialog = false;
        }

        public override void ReadSingleLine()
        {
            if (_curReadingNode == null)
            {
                EndDialog();
                return;
            }

            //�ش� ��带 �湮�ߴٰ� Ȯ������
            DialogConditionManager.Instance.CountVisit(_curReadingNode.guid);

            if (_curReadingNode is NormalNodeSO node)
            {
                characters.ForEach(c =>
                {
                    Debug.Log(c.name);
                    if (c.name == node.GetReaderName())
                    {
                        Debug.Log("��ȣ~");
                        _curCharacter = c;
                        _curCharacter.talkbubble.SetTalkbubble(_curReadingNode);
                        //_curCharacter.talkBubbleObj.SetActive(true);
                    }
                });

                _readingNodeRoutine = StartCoroutine(ReadingNormalNodeRoutine(node));
            }
            else if (_curReadingNode is OptionNodeSO option)
            {
                ReadingOptionNodeRoutine(option);
            }
            else if (_curReadingNode is BranchNodeSO branch)
            {
                JudgementCondition(branch);
            }
        }

        #endregion


        #region ReadingRoutines

        private IEnumerator ReadingNormalNodeRoutine(NormalNodeSO node)
        {
            //�ؽ�Ʈ �������ִ� ����
            TextMeshProUGUI tmp = _curCharacter.contentTxt;

            tmp.SetText(node.GetContents());
            tmp.maxVisibleCharacters = 0;
            InitNodeAnim(node);

            _isReadingDialog = true;

            while (tmp.maxVisibleCharacters < tmp.text.Length)
            {
                //������ �ٷ� �Ѱ�
                if (tmp.text[tmp.maxVisibleCharacters++] == ' ') continue;

                yield return new WaitForSeconds(_textOutDelay);
                //�ؽ�Ʈ ����� ����Ѱ���
                yield return new WaitUntil(() => stopReading == false);
            }

            //���� ��带 �����ϰ� �Ѱ��ִ°���
            _nextNode = node.nextNode;
            StartCoroutine(WaitNodeRoutine(
                () => GetInput(), _curCharacter.talkbubble.TurnOffTalkbubble));
        }


        private void ReadingOptionNodeRoutine(OptionNodeSO node)
        {
            _optionSelected = false;
            _optionBtns = new List<OptionButton>();
            _optionParent.gameObject.SetActive(true);
            InitNodeAnim(node);

            for (int i = 0; i < node.options.Count; i++)
            {
                OptionButton optionButton = Instantiate(node.optionPf, _optionParent);
                optionButton.SetOption(node.options[i], _animPlayer);
                optionButton.OnClcickEvent += OnSelectOption;

                _optionBtns.Add(optionButton);
            }

            StartCoroutine(WaitNodeRoutine(
                () => _optionSelected,
                () =>
                {
                    _optionParent.gameObject.SetActive(false);
                    _optionBtns.ForEach(option => Destroy(option.gameObject));
                    _optionBtns.Clear();
                }));
        }

        private void OnSelectOption(NodeSO node)
        {
            _optionSelected = true;
            _nextNode = node;
        }

        private IEnumerator WaitNodeRoutine(Func<bool> waitPredict, Action endAction)
        {
            yield return new WaitForSeconds(0.1f);
            yield return new WaitUntil(waitPredict);

            CompleteNodeAnim(_curReadingNode);
            _playingEndAnimation = true;
            yield return new WaitUntil(() => !_playingEndAnimation);

            endAction?.Invoke();
            _curReadingNode = _nextNode;
            _isReadingDialog = false;

            yield return new WaitForSeconds(_nextNodeDealy);
            ReadSingleLine();
        }

        private void JudgementCondition(BranchNodeSO branch)
        {
            bool decision = branch.condition.Decision();
            _curReadingNode = branch.nextNodes[decision ? 0 : 1];
            ReadSingleLine();
        }

        #endregion


        private void InitNodeAnim(NodeSO node)
        {
            List<TagAnimation> anims = node.GetAllAnimations();

            anims.ForEach((anim) =>
            {
                anim.Init();

                if (anim is SpriteAnimation srAnim)
                    srAnim.Init(_curCharacter.spriteRenderer);

                if (anim is StopReadingAnimation stopAnim)
                    stopAnim.Init(this);
            });

        }

        private void CompleteNodeAnim(NodeSO node)
        {
            List<TagAnimation> anims = node.GetAllAnimations();
            anims.ForEach((anim) => anim.Complete());
        }
    }

    [Serializable]
    public struct IngameCharacterStruct
    {
        public string name;
        public Talkbubble talkbubble;
        public TextMeshProUGUI contentTxt;
        public SpriteRenderer spriteRenderer;
        //public GameObject talkBubbleObj;
    }
}
