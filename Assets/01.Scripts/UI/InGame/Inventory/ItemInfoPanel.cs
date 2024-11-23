using System.Collections;
using System.Collections.Generic;
using ItemSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace UI.InGame.Inventory
{

    public class ItemInfoPanel : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private TextMeshProUGUI _amountText;

        [SerializeField] private Button _useButton;
        [SerializeField] private Button _dropButton;
        private ItemInfoSO _currentInfo;


        public void SetItemInfo(ItemInfoSO itemInfo, Vector2 position)
        {
            _currentInfo = itemInfo;
            

        }
    }

}