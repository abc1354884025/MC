using MFarm.Inventory;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    public ItemDataList_SO itemDataList;

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
        Debug.LogFormat("获得物品:{0}", GetItemDetails(item.itemID).itemName);
        ;
        if(toDestory){
            Destroy(item.gameObject);
        }
    }
}
