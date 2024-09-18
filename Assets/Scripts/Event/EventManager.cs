using Event.EventArgs;
using Event.EventHandler;
using UnityEngine;

namespace Event
{
    public class EventManager : MonoBehaviour
    {
        private static EventManager _instance;

        public static EventManager Instance
        {
            get
            {
                if (_instance)
                    return _instance;
                _instance = FindObjectOfType<EventManager>() ??
                            new GameObject("EventManager").AddComponent<EventManager>();
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        public event CameraInterActHandler CameraInterAct;
        public event DialogPopHandler DialogPop;

        public event PlayerRunStartHandler PlayerRunStart;
        public event PlayerRunningHandler PlayerRunning;

        public event PlayerRunStopHandler PlayerRunStop;
        public event PlayerWalkStartHandler PlayerWalkStart;
        public event PlayerWalkStopHandler PlayerWalkStop;
        public event PlayerWalkingHandler PlayerWalking;

        public void DialogEventSwitch(string eventName, string args)
        {
            switch (eventName)
            {
                case "OnDialogPop":
                    OnDialogPop(int.Parse(args));
                    break;
                default:
                    break;
            }
        }

        public void OnCameraInterAct(GameObject item)
        {
            CameraInterAct?.Invoke(new CameraInterActArgs(item));
        }

        public void OnDialogPop(int index)
        {
            DialogPop?.Invoke(new DialogPopArgs(index));
        }

        public void OnPlayerWalkStart()
        {
            PlayerWalkStart?.Invoke();
        }

        public void OnPlayerWalkStop()
        {
            PlayerWalkStop?.Invoke();
        }

        public void OnPlayerWalking()
        {
            PlayerWalking?.Invoke();
        }

        public void OnPlayerRunStart()
        {
            PlayerRunStart?.Invoke();
        }

        public void OnPlayerRunning()
        {
            PlayerRunning?.Invoke();
        }

        public void OnPlayerRunStop()
        {
            PlayerRunStop?.Invoke();
        }
    }
}