using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(SerializeDictionary<,>))]
public class CustomSerializeDictionaryEditor : PropertyDrawer
{
    private SerializedProperty _tempKey;
    private SerializedProperty _tempValue;

    private SerializedProperty _linkedProperty;
    private SerializedProperty _linkedKeys;
    private SerializedProperty _linkedValues;

    private ListView _listView;

    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {

        _linkedProperty = property;
        _linkedKeys = property.FindPropertyRelative("SerializedKeys");
        _linkedValues = property.FindPropertyRelative("SerializedValues");

        _tempKey = property.FindPropertyRelative("TempKey");
        _tempValue = property.FindPropertyRelative("TempValue");

        VisualElement root = new VisualElement();

        #region make Edit box
        VisualElement editBox = new Foldout() { text = "Edit dictionary"} ;

        VisualElement boxContainer = new VisualElement();
        boxContainer.style.flexDirection = FlexDirection.Row;
        VisualElement tempValueContainer = new VisualElement();
        tempValueContainer.style.flexGrow = 1;
        var tempKeyField = new PropertyField(_tempKey) {label = "Key" };
        var tempValueField = new PropertyField(_tempValue) { label = "Value" };
        tempValueContainer.Add(tempKeyField);
        tempValueContainer.Add(tempValueField);
        
        var addBtn = new Button() { text = "Add" };
        addBtn.style.flexBasis = new Length(40, LengthUnit.Pixel);
        addBtn.style.flexShrink = 0;
        addBtn.clicked += AddItem;
        boxContainer.Add(tempValueContainer);
        boxContainer.Add(addBtn);

        editBox.Add(boxContainer);

        root.Add(editBox);
        #endregion


        _listView = new ListView()
        {
            showAddRemoveFooter = false,
            showBorder = true,
            showAlternatingRowBackgrounds = AlternatingRowBackground.All,
            showFoldoutHeader = true,
            showBoundCollectionSize = false,
            reorderable = false,
            virtualizationMethod = CollectionVirtualizationMethod.DynamicHeight,
            headerTitle = property.displayName,
            bindingPath = _linkedKeys.propertyPath,
            bindItem = BindItemDraw,
        };

        VisualElement btnRow = new VisualElement();
        btnRow.style.flexDirection = FlexDirection.Row;

        var removeBtn = new Button() { text = "Remove item" };
        removeBtn.clicked += RemoveItem;

        btnRow.Add(removeBtn);

        root.Add(_listView);
        root.Add(btnRow);

        Button removeDuplicateBtn = new Button() { text = "Remove duplicate" };
        removeDuplicateBtn.clicked += OnRemoveDuplicateClick;

        root.Add(removeDuplicateBtn);

        return root;
    }

    private void OnRemoveDuplicateClick()
    {
        List<int> indexToRemove = new List<int>();

        for(int i = 0; i < _linkedKeys.arraySize; i++)
        {
            var first = _linkedKeys.GetArrayElementAtIndex(i);

            for(int j = i + 1; j < _linkedKeys.arraySize; j++)
            {
                var other = _linkedKeys.GetArrayElementAtIndex(j);

                if (indexToRemove.Contains(j)) continue;

                if(first.boxedValue != null && first.boxedValue.Equals(other.boxedValue))
                {
                    indexToRemove.Add(j);
                    continue;
                }

                if(first.boxedValue == null && other.boxedValue == null)
                {
                    indexToRemove.Add(j);
                }
            }
        }

        //뒤에서부터 삭제해야 문제가 없음.
        for(int i = indexToRemove.Count - 1; i >= 0; i--)
        {
            int idx = indexToRemove[i];
            _linkedKeys.DeleteArrayElementAtIndex(idx);
            _linkedValues.DeleteArrayElementAtIndex(idx); //키 밸류를 한꺼번에서 삭제
        }

        _linkedProperty.serializedObject.ApplyModifiedProperties();
    }

    private void RemoveItem()
    {
        int idx = _listView.selectedIndex;

        if(_linkedKeys.arraySize > 0 && idx >= 0 && idx < _linkedKeys.arraySize)
        {
            _linkedKeys.DeleteArrayElementAtIndex(idx);
            _linkedValues.DeleteArrayElementAtIndex(idx);
            _linkedProperty.serializedObject.ApplyModifiedProperties();
        }
    }

    private void AddItem()
    {
        if(_tempKey.boxedValue == null || IsDuplicated(_tempKey, -1))
        {
            Debug.LogWarning( "Duplicated key or null");
            return;
        }

        _linkedKeys.InsertArrayElementAtIndex(_linkedKeys.arraySize);
        _linkedKeys.GetArrayElementAtIndex(_linkedKeys.arraySize -1).boxedValue = _tempKey.boxedValue;
        _linkedValues.InsertArrayElementAtIndex(_linkedValues.arraySize);
        _linkedValues.GetArrayElementAtIndex(_linkedValues.arraySize - 1).boxedValue = _tempValue.boxedValue;


        _linkedProperty.serializedObject.ApplyModifiedProperties();
    }

    private void BindItemDraw(VisualElement itemUI, int index)
    {
        itemUI.Clear();
        itemUI.Unbind();

        var keyProp = _linkedKeys.GetArrayElementAtIndex(index);
        var valueProp = _linkedValues.GetArrayElementAtIndex(index);

        var keyUI = new PropertyField(keyProp) { label = "Key" };
        var valueUI = new PropertyField(valueProp) { label = "Value" };

        itemUI.Add(keyUI);
        itemUI.Add(valueUI);

        var warningUI = new Label("<b>Error: Duplicated key!</b>");
        itemUI.Add(warningUI);

        warningUI.visible = IsDuplicated(keyProp, index);

        itemUI.TrackPropertyValue(keyProp, prop =>
        {
            //bool before = warningUI.visible;
            bool next = IsDuplicated(prop, index);
            warningUI.visible = next;

            //if(before != next)
            //    _listView.RefreshItems();
        });


        itemUI.Bind(_linkedProperty.serializedObject);
    }

    private bool IsDuplicated(SerializedProperty keyProp, int index)
    {
        for(int i = 0; i < _linkedKeys.arraySize; i++)
        {
            if (i == index) continue;

            SerializedProperty otherKey = _linkedKeys.GetArrayElementAtIndex(i);

            if (otherKey.boxedValue == null && keyProp.boxedValue == null)
                return true;
            if (otherKey.boxedValue != null && otherKey.boxedValue.Equals(keyProp.boxedValue))
                return true;
        }
        return false;
    }
}
