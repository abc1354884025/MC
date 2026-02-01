using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MFarm.Inventory
{
    public class ItemManager : MonoBehaviour
    {
        public Item itemPrefab;

        private Transform itemParent;

        private void OnEnable()
        {
            EventHandler.InstantiateItemInScene+= OnInstantiateItenInScene;
        }

        private void OnDisable()
        {   
            EventHandler.InstantiateItemInScene-= OnInstantiateItenInScene;
        }

        private void Start()
        {
            itemParent = GameObject.FindWithTag("ItemParent").transform;
        }

        private void OnInstantiateItenInScene(int ID, Vector3 position)
        {
            var item = Instantiate(itemPrefab, position, Quaternion.identity, itemParent);
            item.itemID = ID;
        }
    }
}
