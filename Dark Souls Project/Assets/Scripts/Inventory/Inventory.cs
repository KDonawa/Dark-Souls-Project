using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(menuName = "Single Instances/Inventory")]
    public class Inventory : ScriptableObject
    {
        public Item[] items;
        Dictionary<string, int> dict = new Dictionary<string, int>();

        public void Initialize()
        {
            for(int k=0;k<items.Length;k++)
            {
                if(dict.ContainsKey(items[k].itemID))
                {

                }
                else
                {
                    dict.Add(items[k].itemID, k);
                }
            }
        }

        public Item GetItem(string id)
        {
            int index = -1;
            if (dict.TryGetValue(id, out index))
                return items[index];

            Debug.Log("No item with " + id + " found!");
            return null;
        }

    }
}