using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventHandler
{
    /// <summary>
    /// 更新背包UI
    /// </summary>
    public static event Action<InventoryLocation, List<InventoryItem>> UpdateInventoryUI;
    public static void CallUpdateInventoryUI(InventoryLocation location,List<InventoryItem> list)
    {
        UpdateInventoryUI?.Invoke(location,list);
    }
    

    /// <summary>
    /// 在场景中生成物品
    /// </summary>
    public static event Action<int, Vector3> InstantiateItemInScene;
    public static void CallInstantiateItemInScene(int itemID, Vector3 position)
    {
        InstantiateItemInScene?.Invoke(itemID, position);
    }

    /// <summary>
    /// 
    /// </summary>
    public static event Action<ItemDetails,bool> ItemSelectEvent;
    public static void CallItemSelectEvent(ItemDetails itemDetails, bool isSelected)
    {
        ItemSelectEvent?.Invoke(itemDetails, isSelected);
    }


    /// <summary>
    /// 游戏时间变化
    /// </summary>
    public static event Action<int,int> GameMinuteEvent;
    public static void CallGameMinuteEvent(int minute, int hour)
    {
        GameMinuteEvent?.Invoke(minute, hour);
    }


    /// <summary>
    /// 游戏日期变化
    /// </summary>
    public static event Action<int,int,int,int,Season> GameDateEvent;
    public static void CallGameDateEvent(int hour,int day, int month, int year,Season season)
    {
        GameDateEvent?.Invoke(hour, day, month, year, season);
    }
}
