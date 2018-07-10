using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scriptable
{
    [System.Serializable]
    public class Action
    {
        public ActionType actionType;
        public Object actionObject;
        
    }

    public enum ActionType
    {
        attack, block, spell, parry
    }
}
