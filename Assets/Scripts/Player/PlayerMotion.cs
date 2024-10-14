using Event;
using Keyboard;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Player
{
    // 玩家运动管理类
    public class PlayerMotion : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb; // 角色的 Rigidbody 组件
        [SerializeField] private Animator animator; // 角色的 Animator 组件
        [SerializeField] private float maxVelocity; // 最大速度
        [SerializeField] private PostProcessVolume postProcessVolume; // 后处理卷
        [SerializeField] private UnityEngine.Camera postProcessCamera; // 后处理相机
        private Vector3 _velocity; // 当前速度
        private bool _isRunning; // 是否正在奔跑
        private bool _isWalking; // 是否正在行走

        private void Update()
        {
            Run(); // 在每帧更新角色的运动
        }

        // 处理角色的运动逻辑
        private void Run()
        {
            // 根据输入方向计算速度
            _velocity = KeySettingManager.Instance.Direction.x * transform.right +
                        KeySettingManager.Instance.Direction.y * transform.forward;

            // 添加力以移动角色
            rb.AddForce(_velocity.normalized * (Time.deltaTime * 50), ForceMode.Impulse);

            // 如果没有输入方向，则限制角色的速度
            if (_velocity == Vector3.zero)
            {
                var temp = rb.linearVelocity; // 获取当前线性速度
                temp = Vector3.ClampMagnitude(temp, 2); // 限制最大速度为 2
                temp.y = rb.linearVelocity.y; // 保留 Y 轴速度
                rb.linearVelocity = temp; // 更新角色的速度
            }

            // 处理奔跑状态
            if (Input.GetKey(KeySettingManager.Instance.GetKey("Run")) &&
                KeySettingManager.Instance.Direction != Vector2.zero)
            {
                HandleRunningState();
            }
            // 处理行走状态
            else if (KeySettingManager.Instance.Direction != Vector2.zero)
            {
                HandleWalkingState();
            }
            // 如果没有输入方向，停止所有运动状态
            else if (KeySettingManager.Instance.Direction == Vector2.zero)
            {
                StopMovement();
            }
        }

        // 处理奔跑状态逻辑
        private void HandleRunningState()
        {
            if (!_isRunning)
            {
                _isRunning = true;
                _isWalking = false;
                EventManager.Instance.OnPlayerWalkStop();
                EventManager.Instance.OnPlayerRunStart();
            }

            EventManager.Instance.OnPlayerRunning();
            rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxVelocity + 2); // 限制速度
        }

        // 处理行走状态逻辑
        private void HandleWalkingState()
        {
            if (!_isWalking)
            {
                _isRunning = false;
                _isWalking = true;
                EventManager.Instance.OnPlayerRunStop();
                EventManager.Instance.OnPlayerWalkStart();
            }

            EventManager.Instance.OnPlayerWalking();
            rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxVelocity); // 限制速度
        }

        // 停止运动状态
        private void StopMovement()
        {
            EventManager.Instance.OnPlayerRunStop();
            EventManager.Instance.OnPlayerWalkStop();
        }
    }
}
