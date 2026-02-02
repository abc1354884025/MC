using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MFarm.Inventory
{
    public class ShowItemTooltip : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
    {

        private SlotUI slotUI;

        private InventoryUI inventoryUI=>GetComponentInParent<InventoryUI>();

        private void Awake()
        {
            slotUI = GetComponent<SlotUI>();
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (slotUI.itemAmount != 0)
            {
                inventoryUI.itemTooltip.gameObject.SetActive(true);
                inventoryUI.itemTooltip.SetupTooTip(slotUI.itemDetails, slotUI.slotType);

                inventoryUI.itemTooltip.transform.position=eventData.position;

                var rect = inventoryUI.itemTooltip.transform.GetComponent<RectTransform>();

                var rectX = (rect.rect.width+ eventData.position.x > Screen.width) ? 1 : 0;
                var rectY = (eventData.position.y- rect.rect.height <0) ? 0 : 1;
                rect.pivot = new Vector3(rectX, rectY, 0);

            }
            else
            {
                inventoryUI.itemTooltip.gameObject.SetActive(false);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            inventoryUI.itemTooltip.gameObject.SetActive(false);
        }

    }
}
