using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dialog
{
    public partial class AnimItem : VisualElement
    {


        //private TextInfo _text;
        //private TagAnimation _tagAnim;

        //private VisualElement _animItem;
        //private MinMaxSlider _minMaxSlider;
        //private InspectorView _animFactor;
        //private Label _animNameLabel;
        //private Label _assignedText;
        //private Button _removeBtn;

        //public AnimItem() 
        //{
        //    Init();
        //}

        //public AnimItem(TextInfo text, TagAnimation tagAnim) 
        //{
        //    _text = text;
        //    _tagAnim = tagAnim;

        //    Init();
        //}

        //private void HandleMinMaxSliderValueChanged(ChangeEvent<Vector2> value)
        //{
        //    int x = Mathf.RoundToInt(value.newValue.x);
        //    int y = Mathf.RoundToInt(value.newValue.y);

        //    _minMaxSlider.value = new Vector2(x, y);

        //    string applyTxt = _tagAnim.tagType.ToString().Substring(x, (y - x));
        //    _assignedText.text = applyTxt;
        //}

        //public void RemoveAnimation(ClickEvent evt)
        //{
        //    _text.RemoveAnimation(_tagAnim);
        //    RemoveFromHierarchy();
        //}

        //private void Init()
        //{
        //    VisualTreeAsset visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Dialog/01.Scripts/Editor/AnimItem.uxml");
        //    VisualElement animItem = visualTreeAsset.Instantiate();

        //    _minMaxSlider = animItem.Q<MinMaxSlider>("min-max-slider");
        //    _animFactor = animItem.Q<InspectorView>("anim-factor");
        //    _assignedText = animItem.Q<Label>("anim-apply-txt");
        //    _animNameLabel = animItem.Q<Label>("anim-type");
        //    _removeBtn = animItem.Q<Button>("remove-anim-btn");

        //    _removeBtn.RegisterCallback<ClickEvent>(RemoveAnimation);
        //    _minMaxSlider.RegisterValueChangedCallback(HandleMinMaxSliderValueChanged);
        //    _animNameLabel.text = _tagAnim.tagType.ToString();
        //    _animFactor.UpdateSelection(_tagAnim.tagType);

        //    Add(animItem);
        //}
    }
}
