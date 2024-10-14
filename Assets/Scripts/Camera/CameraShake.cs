using Event;
using UnityEngine;

namespace Camera
{
    // 处理相机的震动效果
    public class CameraShake : MonoBehaviour
    {
        [Header("Camera Settings")]
        [Tooltip("相机抖动的Transform。")]
        public Transform cameraTransform; // 相机的变换组件

        [Header("抖动参数")]
        [Tooltip("抖动效果的振幅。")]
        public float amplitude = 0.05f; // 震动幅度
        [Tooltip("抖动效果的频率。")]
        public float frequency = 10.0f; // 震动频率

        private Vector3 _originalPos; // 相机的初始位置
        private bool _isRunning; // 玩家是否在奔跑
        private bool _isWalking; // 玩家是否在行走

        private void Start()
        {
            // 存储相机的初始位置
            _originalPos = cameraTransform.localPosition;
        }

        private void OnEnable()
        {
            // 订阅事件
            EventManager.Instance.PlayerRunning += OnPlayerRunning; // 玩家开始奔跑事件
            EventManager.Instance.PlayerRunStop += StopRunning; // 玩家停止奔跑事件
            EventManager.Instance.PlayerWalking += OnPlayerWalking; // 玩家开始行走事件
            EventManager.Instance.PlayerWalkStop += StopWalking; // 玩家停止行走事件
        }

        private void OnDisable()
        {
            // 取消订阅事件
            EventManager.Instance.PlayerRunning -= OnPlayerRunning;
            EventManager.Instance.PlayerRunStop -= StopRunning;
            EventManager.Instance.PlayerWalking -= OnPlayerWalking;
            EventManager.Instance.PlayerWalkStop -= StopWalking;
        }

        // 玩家开始奔跑时调用
        private void OnPlayerRunning()
        {
            _isRunning = true; // 设置奔跑状态
        }

        // 玩家开始行走时调用
        private void OnPlayerWalking()
        {
            _isWalking = true; // 设置行走状态
        }

        // 停止奔跑时调用
        private void StopRunning()
        {
            _isRunning = false; // 重置奔跑状态
        }

        // 停止行走时调用
        private void StopWalking()
        {
            _isWalking = false; // 重置行走状态
        }

        private void Update()
        {
            // 处理相机震动效果
            if (_isRunning)
            {
                // 计算奔跑状态下的震动
                ApplyShake(frequency, amplitude);
            }
            else if (_isWalking)
            {
                // 计算行走状态下的震动
                ApplyShake(frequency / 2, amplitude);
            }
            else
            {
                // 恢复相机到初始位置
                cameraTransform.localPosition = _originalPos;
            }
        }

        // 应用相机震动效果
        private void ApplyShake(float shakeFrequency, float shakeAmplitude)
        {
            var xShake = Mathf.Sin(Time.time * shakeFrequency) * shakeAmplitude; // X轴震动
            var yShake = Mathf.Cos(Time.time * shakeFrequency * 2) * shakeAmplitude * 0.5f; // Y轴震动

            // 更新相机位置
            cameraTransform.localPosition = _originalPos + new Vector3(xShake, yShake, 0);
        }
    }
}
