using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
    [CreateAssetMenu(menuName = "Items/Armor")]
    public class Armor : ScriptableObject
    {
        public ArmorType armorType;
        public Mesh armorMesh;
        public Material[] materials;
        public bool baseBodyEnabled;

    }

    
}

public enum ArmorType
{
    chest, legs, hands, head
}