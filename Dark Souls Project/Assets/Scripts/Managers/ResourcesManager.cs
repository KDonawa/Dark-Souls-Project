using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers { 
    public class ResourcesManager : MonoBehaviour
    {

        public Inventory.Inventory inventory;


        void Awake()
        {
            inventory.Initialize();
        }

        public Inventory.Item GetItem(string id)
        {
            return inventory.GetItem(id);
        }

        public Inventory.Weapon GetWeapon(string id)
        {
            Inventory.Item item = GetItem(id);
            return (Inventory.Weapon)item.obj;
        }

        public Inventory.Armor GetArmor(string id)
        {
            Inventory.Item item = GetItem(id);
            return (Inventory.Armor)item.obj;
        }


    }
}
