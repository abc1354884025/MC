using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemTooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private GameObject bottemPart;

    public void SetupTooTip(ItemDetails itemDetails,SlotType slotType)
    {
        nameText.text = itemDetails.itemName;
        typeText.text = GetItemType(itemDetails.itemType);
        descriptionText.text = itemDetails.itemDescription;
        if(itemDetails.itemType == ItemType.Seed || itemDetails.itemType == ItemType.Commodity || itemDetails.itemType == ItemType.Furniture)
        {
            bottemPart.SetActive(true);
            var price = itemDetails.price;
            if(slotType == SlotType.Bag)price=(int)(price*itemDetails.sellPercentage);

            valueText.text = price.ToString();
        }
        else
        {
            bottemPart.SetActive(false);
        }
    }

    private string GetItemType(ItemType itemType)
    {
        return itemType switch
        {
            ItemType.Seed => "种子",
            ItemType.Commodity => "商品",
            ItemType.Furniture => "家具",
            ItemType.BreakTool => "工具",
            ItemType.HoeTool => "锄头",
            ItemType.ChopTool => "割草工具",
            ItemType.ReapTool => "收割工具",
            ItemType.WaterTool => "浇水工具",
            ItemType.CollectTool => "采集工具",
            ItemType.Reapablescenery => "可收割植物",
            _ => "未知"
        };
    }
}