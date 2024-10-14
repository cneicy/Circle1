using Event;
using Event.EventArgs;
using UnityEngine;

namespace Items.Abstract
{
    public abstract class ItemBase : MonoBehaviour
    {
        public int index;  // 对话索引
        public string itemNameKey;  //物体本地化key

        #region 相机交互事件订阅
        private void OnEnable()
        {
            EventManager.Instance.CameraInterAct += ReceiveEvent;
        }

        private void OnDisable()
        {
            EventManager.Instance.CameraInterAct -= ReceiveEvent;
        }
        #endregion
        
        // 收到相机交互事件
        protected virtual void ReceiveEvent(CameraInterActArgs item)
        {
            if (item.Item != gameObject) return;
            ActivateItem();
        }

        //物体逻辑
        protected virtual void ActivateItem()
        {
            //Debug.Log("Item is activated");
        }
    }
}