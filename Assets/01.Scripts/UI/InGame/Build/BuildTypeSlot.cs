using UnityEngine;
using UnityEngine.UI;

namespace UI.InGame.Build
{
    public class BuildTypeSlot : MonoBehaviour
    {
        [SerializeField] private BuildCategory _category;
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(HandleClickCategorySelectButton);
        }

        private void HandleClickCategorySelectButton()
        {
            
        }
    }
}