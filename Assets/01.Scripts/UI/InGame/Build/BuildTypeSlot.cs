using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.InGame.Build
{
    public class BuildTypeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private BuildingCategoryType _type;
        [SerializeField] private BuildCategory _category;
        private BuildTypeGroupPanel _ownerGroupPanel;
        [SerializeField] private Image _categoryIconImage;
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(HandleClickCategorySelectButton);
        }

        public void Initialize(BuildTypeGroupPanel groupPanel, BuildingCategoryType type, BuildCategory category)
        {
            _ownerGroupPanel = groupPanel;
            _categoryIconImage.sprite = category.categoryIconSprite;
            _type = type;
            _category = category;
            
        }

        private void HandleClickCategorySelectButton()
        {
            _ownerGroupPanel.HandleSelectCategory(_category);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _ownerGroupPanel.HandleSetDescriptionActive(true);
            _ownerGroupPanel.HandleOnPointerDescription(_category);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _ownerGroupPanel.HandleSetDescriptionActive(false);
        }
    }
}