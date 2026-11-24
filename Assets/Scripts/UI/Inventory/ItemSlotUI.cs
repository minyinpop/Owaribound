using Inventory.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory
{
    public sealed class ItemSlotUI : MonoBehaviour
    {
        [field: Header("格子設定")]
        [field: SerializeField] private Image slotImage;
        [field: SerializeField] private Sprite selectedFrame;
        [field: SerializeField] private Sprite unselectedFrame;
        
        [field: Header("內容設定")]
        [field: SerializeField] private Image itemImage;
        [field: SerializeField] private TextMeshProUGUI itemAmount;
        [field: SerializeField] private TextMeshProUGUI slotNumber;

        public void Refresh(ItemSlot slot)
        {
            if (slot.IsEmpty)
            {
                itemImage.gameObject.SetActive(false);
                itemImage.sprite = null;
                
                itemAmount.gameObject.SetActive(false);
                itemAmount.text = string.Empty;
                
                return;
            }

            itemImage.sprite = slot.item.Icon;
            itemImage.gameObject.SetActive(true);
            
            itemAmount.text = slot.count.ToString();
            itemAmount.gameObject.SetActive(true);
        }

        public void SetNumber(int number)
        {
            slotNumber.text = number.ToString();
        }

        public void SetSelected(bool selected)
        {
            slotImage.sprite = selected ? selectedFrame : unselectedFrame;
        }
    }
}