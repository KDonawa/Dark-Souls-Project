using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(menuName = "Items/Item")]
    public class Item : ScriptableObject
    {

        public string itemID;
        public ItemType type;
        public ItemUIStats info;
        public Object obj;

        [System.Serializable]
        public class ItemUIStats
        {
            public string itemName;
            public string itemDescription;
            public string skillDescription;
            public Sprite icon;
        }

    }
    
    public enum ItemType
    {
        weapon, armor, consumable
    }
}

