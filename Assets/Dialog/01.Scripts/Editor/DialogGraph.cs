#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;


namespace Dialog
{
    public class DialogGraph : EditorWindow
    {
        [SerializeField]
        private VisualTreeAsset m_VisualTreeAsset = default;
        private VisualTreeAsset _animItemAsset;
        private VisualElement _animEditor;

        private DialogView _dialogView;
        private InspectorView _inspectorView;
        private InspectorView _conditionInspector;

        private MinMaxSlider _minMaxSlider;
        private VisualElement _txtParent;
        private VisualElement _bottomBar;
        private InspectorView _animFactor;
        private Label _animText;
        private Button _addBtn;
        private EnumField _animType;

        private VisualElement _items;
        private Label _animApplyText;

        private SerializedObject _dialogObj;
        private NodeSO _curNode;
        private TextInfo _curTextInfo;

        private VisualElement _animInserter;
        private VisualElement _animManager;

        [OnOpenAsset]
        public static bool OnOpenAsset(int instanceId, int line)
        {
            if (Selection.activeObject is DialogSO dialogSO)
            {
                ShowEditor();
                return true;
            }
            return false;
        }

        [MenuItem("Tools/DialogGenerator")]
        private static void ShowEditor()
        {
            DialogGraph wnd = GetWindow<DialogGraph>();
            wnd.titleContent = new GUIContent("JINSOON DIALOG");
        }

        public void CreateGUI()
        {
            VisualElement root = rootVisualElement;

            VisualTreeAsset animAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Dialog/01.Scripts/Editor/AnimationInserter.uxml");
            _animItemAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Dialog/01.Scripts/Editor/AnimItem.uxml");
            _animEditor = animAsset.Instantiate();

            VisualElement content = m_VisualTreeAsset.Instantiate();
            content.style.flexGrow = 1;
            root.Add(content);

            _dialogView = content.Q<DialogView>("dialog-view");
            _inspectorView = content.Q<InspectorView>("inspector-view");
            _conditionInspector = content.Q<InspectorView>("condition-view");
            _bottomBar = content.Q<VisualElement>("bottom-bar");
            _animInserter = content.Q<VisualElement>("anim-inserter");
            _animManager = content.Q<VisualElement>("anim-manager");
            _items = content.Q<VisualElement>("items");

            _dialogView.OnNodeSelected += HandleNodeSelect;
            OnSelectionChange();
        }


        private void OnSelectionChange()
        {
            var dialogSO = Selection.activeObject as DialogSO;

            if (dialogSO == null)
            {
                if (Selection.activeGameObject)
                {
                    var player = Selection.activeGameObject.GetComponent<DialogPlayer>();

                    if (player != null) dialogSO = player.dialog;
                }
            }

            if (dialogSO != null)
                _dialogObj = new SerializedObject(dialogSO);

            if (Application.isPlaying)
            {
                if (dialogSO != null)
                    _dialogView?.PopulateTree(dialogSO);
            }
            else
            {
                if (dialogSO != null && AssetDatabase.CanOpenAssetInEditor(dialogSO.GetInstanceID()))
                    _dialogView?.PopulateTree(dialogSO);
            }
        }

        private void HandleMinMaxSliderValueChanged(ChangeEvent<Vector2> value)
        {
            int x = Mathf.RoundToInt(value.newValue.x);
            int y = Mathf.RoundToInt(value.newValue.y);

            _minMaxSlider.value = new Vector2(x, y);

            string applyTxt = _animText.text.Substring(x, (y - x));
            _animApplyText.text = applyTxt;
        }

        private void HandleAnimItemAnimRangeValueChanged(ChangeEvent<Vector2> value, MinMaxSlider animRange, Label animLabel, TagAnimation tagAnim)
        {
            int x = Mathf.RoundToInt(value.newValue.x);
            int y = Mathf.RoundToInt(value.newValue.y);
            animRange.value = new Vector2(x, y);

            tagAnim.animStartPos = x;
            tagAnim.animLength = y - x;

            string applyTxt = _animText.text.Substring(x, (y - x));
            animLabel.text = applyTxt;
        }

        public void AddAnimEditor(NodeSO node)
        {
            _animInserter.Add(_animEditor);

            _txtParent = _animInserter.Q<VisualElement>("txt-parent");

            _minMaxSlider = _animInserter.Q<MinMaxSlider>("min-max-slider");
            _animFactor = _animInserter.Q<InspectorView>("anim-factor");
            _animApplyText = _animInserter.Q<Label>("anim-apply-txt");
            _animType = _animInserter.Q<EnumField>("anim-type");
            _addBtn = _animInserter.Q<Button>("add-anim-btn");
            _animText = _animInserter.Q<Label>("anim-txt");

            _minMaxSlider.RegisterValueChangedCallback(HandleMinMaxSliderValueChanged);
            _animType.RegisterValueChangedCallback(HandleAnimTypeChanged);
            _addBtn.RegisterCallback<ClickEvent>(HandleClickAdd);
        }

        private void AddAnimItems(TextInfo textInfo)
        {
            _animManager.Clear();
            textInfo.GetAnimations().ForEach(anim => AddAnimItems(anim));
        }

        public void AddAnimItems(TagAnimation anim)
        {
            VisualElement animItem = _animItemAsset.Instantiate();

            animItem.Q<Label>("anim-type-txt").text = anim.tagType.ToString();
            Label selectedAnimText = animItem.Q<Label>("selected-anim-txt");
            MinMaxSlider animRange = animItem.Q<MinMaxSlider>("anim-range");
            InspectorView animFactor = animItem.Q<InspectorView>("anim-manage-factor");
            Button removeBtn = animItem.Q<Button>("remove-anim-btn");

            animRange.highLimit = _animText.text.Length;
            selectedAnimText.text =
                _animText.text.Substring(anim.animStartPos, anim.animLength);

            animRange.value = new Vector2(anim.animStartPos,
                anim.animStartPos + anim.animLength);

            animRange.RegisterValueChangedCallback((v) =>
                    HandleAnimItemAnimRangeValueChanged(v, animRange, selectedAnimText, anim));

            animFactor.Clear();
            if (anim.tagType == TagEnum.None) return;
            animFactor.UpdateSelection(anim);

            removeBtn.RegisterCallback<ClickEvent>(evt =>
            {
                _curTextInfo.RemoveAnimation(anim);
                _animManager.Remove(animItem);
            });

            _animManager.Add(animItem);
        }


        public void SetAnimText(TextInfo text)
        {
            _animText.text = text.text;
            _animApplyText.text = text.text;
            _minMaxSlider.highLimit = text.text.Length;
            _minMaxSlider.value = new Vector2(0, text.text.Length);
            _curTextInfo = text;
        }

        private void HandleClickAdd(ClickEvent evt)
        {
            TagEnum tag = (TagEnum)_animType.value;
            if (tag == TagEnum.None) return;

            TagAnimation tagAnim = _animFactor.tagAnim;
            _curTextInfo.AddAnimation(tagAnim, (int)_minMaxSlider.minValue, (int)_minMaxSlider.maxValue);
            AddAnimItems(tagAnim);
        }

        private void HandleAnimTypeChanged(ChangeEvent<Enum> value)
        {
            if (value.newValue is TagEnum anim)
            {
                _animFactor.Clear();
                if (anim == TagEnum.None) return;
                _animFactor.UpdateSelection(anim);
            }
        }

        private void HandleNodeSelect(NodeView view)
        {
            _inspectorView.UpdateSelection(view);
            _curNode = view.nodeSO;

            _animInserter.Clear();
            _animManager.Clear();
            _items.Clear();

            if (view.nodeSO is BranchNodeSO branch)
            {
                _conditionInspector.UpdateSelection(branch.condition);
            }
            else
            {
                _conditionInspector.ClearSelection();

                if (view.nodeSO is NormalNodeSO node)
                {
                    CreateItemBtn("Content", node.contents);

                    AddAnimEditor(node);
                    SetAnimText(node.contents);
                    AddAnimItems(node.contents);
                }
                else if (view.nodeSO is OptionNodeSO option)
                {
                    for (int i = 0; i < option.options.Count; i++)
                        CreateItemBtn($"Option-{i + 1}", option.options[i].option);

                    AddAnimEditor(option);
                    if (option.options.Count > 0)
                    {
                        SetAnimText(option.options[0].option);
                        AddAnimItems(option.options[0].option);
                    }
                }
            }
        }

        private void CreateItemBtn(string btnName, TextInfo txtInfo)
        {
            Button btn = new Button();
            btn.text = btnName;
            btn.AddToClassList("item-btn");
            btn.RegisterCallback<ClickEvent>(evt => SetAnimText(txtInfo));
            _items.Add(btn);
        }
    }
}


#endif