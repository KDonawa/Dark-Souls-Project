using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scriptable
{
    [CreateAssetMenu(menuName = "Actions/Attack Action")]
    public class AttackAction : ScriptableObject
    {
        public StringVariable attackAnim;
        public bool canBeParried = true;
        public bool changeSpeed = false;
        public float animSpeed = 1;
        public bool canParry = false;
        public bool canBackstab = false;
      
    }
}
