using Event;
using Keyboard;
using UnityEngine;

namespace Camera
{
    // 处理摄像机与可交互物体的交互
    public class CameraInterAct : MonoBehaviour
    {
        [Header("Interaction Settings")]
        [SerializeField] private int maxInterActDistance = 10; // 最大交互距离

        private void Update()
        {
            // 执行射线检测
            if (!Physics.Raycast(transform.position, transform.forward, out var raycastHit,
                    maxInterActDistance)) return;
            // 检测到碰撞体并按下交互键
            if (Input.GetKeyDown(KeySettingManager.Instance.GetKey("InterAct")))
            {
                EventManager.Instance.OnCameraInterAct(raycastHit.collider.gameObject);
            }
        }

        private void OnDrawGizmos()
        {
            // 在场景视图中绘制射线，便于调试
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * maxInterActDistance);
        }
    }
}