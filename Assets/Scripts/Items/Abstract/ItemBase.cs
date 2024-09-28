using Event;
using Event.EventArgs;
using UnityEngine;

namespace Items.Abstract
{
    public abstract class ItemBase : MonoBehaviour
    {
        public int index;

        private void OnEnable()
        {
            EventManager.Instance.CameraInterAct += ReceiveEvent;
        }

        private void OnDisable()
        {
            EventManager.Instance.CameraInterAct -= ReceiveEvent;
        }

        protected virtual void ReceiveEvent(CameraInterActArgs item)
        {
            if (item.Item != gameObject) return;
            ActivateItem();
        }

        protected virtual void ActivateItem()
        {
            //Debug.Log("Item is activated");
        }
    }
}