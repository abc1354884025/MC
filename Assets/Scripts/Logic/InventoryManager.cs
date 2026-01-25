using MFarm.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    public ItemDataList_SO itemDataList;

    public InventoryBag_SO playerBag;

    /// <summary>
    /// 根据itemID获取物品详情
    /// </summary>
    /// <param name="itemID">物品ID</param>
    /// <returns></returns>
    public ItemDetails GetItemDetails(int itemID)
    {
        return itemDataList.itemDetailsList.Find(item => item.itemID == itemID);
    }

    /// <summary>
    /// 添加物品
    /// </summary>
    /// <param name="item"></param>
    /// <param name="toDestory">是否要删除 默认删除</param>

    public void AddItem(Item item,bool toDestory =true)
    {

        var index=GetItemIndexInBag(item.itemID);
        if(index==-1&& !CheckBagCapacity()) return;
        AddItemAtIndex(item.itemID, index, 1);

        InventoryItem newItem= new InventoryItem();
        newItem.itemID = item.itemID;
        newItem.itemAmount = 1;

        Debug.LogFormat("获得物品:{0}", GetItemDetails(item.itemID).itemName);
        ;
        if(toDestory){
            Destroy(item.gameObject);
        }
    }

    /// <summary>
    /// 检查背包是否有空位
    /// </summary>
    /// <returns></returns>
    private bool CheckBagCapacity()
    {
        for(int i = 0; i < playerBag.itemList.Count; i++)
        {
            if (playerBag.itemList[i].itemID == 0)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 通过物品ID找到背包已有物品位置
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    private int GetItemIndexInBag(int ID)
    {
        for(int i = 0;i < playerBag.itemList.Count;i++)
        {
            if (playerBag.itemList[i].itemID == ID)
            {
                return i;
            }
        }
        return -1;
    }

    private void AddItemAtIndex(int ID, int index, int amount)
    {
        if (index == -1)
        {
            var item = new InventoryItem { itemID = ID, itemAmount = amount };
            for(int i = 0; i < playerBag.itemList.Count; i++)
            {
                if (playerBag.itemList[i].itemID == 0)
                {
                    playerBag.itemList[i] = item;
                    break;
                }
            }
            return;
        }
        else
        {
            int currentAmount = playerBag.itemList[index].itemAmount + amount;
            var item = new InventoryItem { itemID = ID, itemAmount = currentAmount };
            playerBag.itemList[index] = item;
        }
        
    }
}
