using System;
using Input;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory
{
    public sealed class HeldItemUI : MonoBehaviour
    {
        [field: Header("自身組件")]
        [field: SerializeField] private Image itemImage;
        [field: SerializeField] private TextMeshProUGUI itemAmount;
        
        // ================================
        
        [field: Header("參數設定")]
        [field: Tooltip("決定物品托拽顯示要在多少的偏移")]
        [field: SerializeField] private Vector2 offset = new Vector2(12, -12);
        
        // ================================
        
        [field: Header("其它組件")]
        [field: SerializeField] private InventoryController inventoryController;
        [field: SerializeField] private InputHandler inputHandler;

        private void OnEnable()
        {
            inventoryController.OnHeldChanged += Refresh;
            Refresh();
        }

        private void Update()
        {
            transform.position = inputHandler.MousePos + offset;
        }

        private void OnDisable()
        {
            inventoryController.OnHeldChanged -= Refresh;
        }

        private void Refresh()
        {
            var held = inventoryController.HeldSlot;
            var show = !held.IsEmpty;
            
            itemImage.gameObject.SetActive(show);
            itemAmount.gameObject.SetActive(show && held.count > 1);
            if (!show) return;

            itemImage.sprite = held.itemConfig.Icon;
            itemAmount.text = held.count.ToString();
        }
    }
}