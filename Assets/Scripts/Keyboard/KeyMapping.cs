using UnityEngine;

namespace Keyboard
{
    [System.Serializable]
    public class KeyMapping
    {
        public string actionName;
        public KeyCode keyCode;

        public KeyMapping(string actionName, KeyCode keyCode)
        {
            this.actionName = actionName;
            this.keyCode = keyCode;
        }
    }
}