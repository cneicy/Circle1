using Event.EventArgs;
using Event.EventHandler;
using UnityEngine;

namespace Event
{
    // 事件管理器类，负责事件的注册和触发
    public class EventManager : MonoBehaviour
    {
        private static EventManager _instance;

        // 单例模式实例获取
        public static EventManager Instance
        {
            get
            {
                if (_instance)
                    return _instance;

                // 创建新的 GameObject 和 EventManager 组件
                _instance = FindFirstObjectByType<EventManager>() ??
                            new GameObject("EventManager").AddComponent<EventManager>();
                return _instance;
            }
        }

        private void Awake()
        {
            // 确保只存在一个 EventManager 实例
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        // 定义事件
        public event CameraInterActHandler CameraInterAct;
        public event DialogPopHandler DialogPop;
        public event PlayerRunStartHandler PlayerRunStart;
        public event PlayerRunningHandler PlayerRunning;
        public event PlayerRunStopHandler PlayerRunStop;
        public event PlayerWalkStartHandler PlayerWalkStart;
        public event PlayerWalkStopHandler PlayerWalkStop;
        public event PlayerWalkingHandler PlayerWalking;
        public event ItemDialogHandler ItemDialog;
        public event TextChangeHandler TextChange;

        // 事件开关，根据事件名称调用相应的方法
        public void EventSwitch(string eventName, string args)
        {
            switch (eventName)
            {
                case "OnDialogPop":
                    OnDialogPop(int.Parse(args));
                    break;
                // 可以在此处添加更多事件
                default:
                    break;
            }
        }

        // 触发相机交互事件
        public void OnCameraInterAct(GameObject item)
        {
            CameraInterAct?.Invoke(new CameraInterActArgs(item));
        }

        // 触发对话框弹出事件
        public void OnDialogPop(int index)
        {
            DialogPop?.Invoke(new DialogPopArgs(index));
        }

        // 触发玩家开始走路事件
        public void OnPlayerWalkStart()
        {
            PlayerWalkStart?.Invoke();
        }

        // 触发玩家停止走路事件
        public void OnPlayerWalkStop()
        {
            PlayerWalkStop?.Invoke();
        }

        // 触发玩家行走事件
        public void OnPlayerWalking()
        {
            PlayerWalking?.Invoke();
        }

        // 触发玩家开始奔跑事件
        public void OnPlayerRunStart()
        {
            PlayerRunStart?.Invoke();
        }

        // 触发玩家正在奔跑事件
        public void OnPlayerRunning()
        {
            PlayerRunning?.Invoke();
        }

        // 触发玩家停止奔跑事件
        public void OnPlayerRunStop()
        {
            PlayerRunStop?.Invoke();
        }

        // 触发文本变化事件
        public void OnTextChange()
        {
            TextChange?.Invoke();
        }

        // 触发物品对话事件
        public void OnItemDialog(string name)
        {
            ItemDialog?.Invoke(new ItemDialogArgs(name));
        }
    }
}
