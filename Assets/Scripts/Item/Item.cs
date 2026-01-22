using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MFarm.Inventory
{
    public class Item : MonoBehaviour
    {
        public int itemID;
        public ItemDetails itemDetails{ private set; get; }

        private SpriteRenderer sr;
        
        private BoxCollider2D collider;

        private void Awake()
        {
            sr = GetComponentInChildren<SpriteRenderer>();
            collider = GetComponent<BoxCollider2D>();
            if (itemID != 0) Init();
        }

        public void Init()
        {
            itemDetails = InventoryManager.Instance.GetItemDetails(itemID);
            if(itemDetails != null)
            {
                sr.sprite = itemDetails.itemOnWorldSprite!=null?itemDetails.itemOnWorldSprite:itemDetails.itemIcon;

                //ÐÞ¸ÄÅö×²Ìå³ß´ç
                Vector2 newSize= new Vector2(sr.sprite.bounds.size.x,sr.sprite.bounds.size.y);
                collider.size = newSize;
                collider.offset=new Vector2(sr.sprite.bounds.center.x, sr.sprite.bounds.center.y);
                
            }
        }

    }

}
