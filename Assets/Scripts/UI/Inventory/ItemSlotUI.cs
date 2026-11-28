using System;
using Inventory.Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Inventory
{
    public sealed class ItemSlotUI : MonoBehaviour, IPointerDownHandler
    {
        [field: Header("格子設定")]
        [field: SerializeField] private Image slotImage;
        [field: SerializeField] private Sprite selectedFrame;
        [field: SerializeField] private Sprite unselectedFrame;
        
        [field: Header("內容設定")]
        [field: SerializeField] private Image itemImage;
        [field: SerializeField] private TextMeshProUGUI itemAmount;
        [field: SerializeField] private TextMeshProUGUI slotNumber;
        
        public int Index { get; set; }
        public event Action<int, PointerEventData.InputButton> OnClicked;

        public void OnPointerDown(PointerEventData eventData)
        {
            OnClicked?.Invoke(Index, eventData.button);
        }

        public void Refresh(ItemSlotData slotData)
        {
            if (slotData.IsEmpty)
            {
                itemImage.gameObject.SetActive(false);
                itemImage.sprite = null;
                
                itemAmount.gameObject.SetActive(false);
                itemAmount.text = string.Empty;
                
                return;
            }

            itemImage.sprite = slotData.itemConfig.Icon;
            itemImage.gameObject.SetActive(true);
            
            itemAmount.text = slotData.count.ToString();
            itemAmount.gameObject.SetActive(true);
        }

        public void SetNumber(int number)
        {
            var show = number >= 0;
            slotNumber.gameObject.SetActive(show);
            if (show)
                slotNumber.text = number.ToString();
        }

        public void SetSelected(bool selected)
        {
            slotImage.sprite = selected ? selectedFrame : unselectedFrame;
        }
    }
}