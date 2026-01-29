using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MFarm.Inventory
{
    public class SlotUI : MonoBehaviour,IPointerClickHandler,IBeginDragHandler,IDragHandler,IEndDragHandler
    {
        [Header("组件获取")]
        [SerializeField] private Image slotImage;
        [SerializeField] private TextMeshProUGUI amountText;
        [SerializeField] public Image slotHightlight;
        [SerializeField] private Button button;
        [Header("格子类型")]
        public SlotType slotType;
        public bool isSelected;

        public ItemDetails itemDetails;
        public int itemAmount;
        public int slotIndex;

        private InventoryUI inventoryUI =>GetComponentInParent<InventoryUI>();
        private void Start()
        {
            isSelected = false;
            if (itemDetails.itemID == 0) UpdateEmptySlot();
        }

        /// <summary>
        /// 更新格子UI
        /// </summary>
        /// <param name="item"></param>
        /// <param name="amount"></param>
        public void UpdateSlot(ItemDetails item, int amount)
        {
            itemDetails = item;
            slotImage.sprite = item.itemIcon;
            itemAmount = amount;
            amountText.text = amount.ToString();
            button.interactable = true;
            slotImage.enabled = true;
        }

        /// <summary>
        /// 将Slot置空
        /// </summary>
        public void UpdateEmptySlot()
        {
            if (isSelected)
            {
                isSelected = false;
            }
            slotImage.enabled = false;
            amountText.text = string.Empty;
            button.interactable = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (itemAmount == 0) return;
            isSelected=!isSelected;
            slotHightlight.gameObject.SetActive(isSelected);
            inventoryUI.UpdateSlotHightlight(slotIndex);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if(itemAmount==0) return;
            inventoryUI.dragItem.enabled=true;
            inventoryUI.dragItem.sprite = slotImage.sprite;

            isSelected = true;
            inventoryUI.UpdateSlotHightlight(slotIndex);
        }

        public void OnDrag(PointerEventData eventData)
        {
            inventoryUI.dragItem.transform.position=Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            inventoryUI.dragItem.enabled = false;
        }
    }
}

