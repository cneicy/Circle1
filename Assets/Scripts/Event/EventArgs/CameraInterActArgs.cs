using UnityEngine;

namespace Event.EventArgs
{
    public class CameraInterActArgs : System.EventArgs
    {
        public GameObject Item { get; }

        public CameraInterActArgs(GameObject item)
        {
            Item = item;
        }
    }
}