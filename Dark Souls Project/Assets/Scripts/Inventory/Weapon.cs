using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scriptable;

namespace Inventory
{
    [CreateAssetMenu(menuName = "Items/Weapon")]
    public class Weapon : ScriptableObject
    {
        public StringVariable oh_idle;
        public StringVariable th_idle;
        public GameObject modelProfab;

        public ActionHolder[] actions;

        public ActionHolder GetActionHolder(InputType input)
        {
            for (int k =0; k<actions.Length;k++)
            {
                if (actions[k].input == input)
                    return actions[k];
            }
            return null;
        }

        public Action GetAction(InputType input)
        {
            ActionHolder actionHolder = GetActionHolder(input);
            if(actionHolder != null)
                return actionHolder.action;
            return null;
        }
    }

    [System.Serializable]
    public class ActionHolder
    {
        public InputType input;
        public Action action;
    }


}

public enum InputType
{
    LMB/*rb*/, lb, RMB/*rt*/, lt
}
