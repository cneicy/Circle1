using Event;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Camera
{
    // 处理相机的后期处理效果
    public class CameraPostProcessing : MonoBehaviour
    {
        private UnityEngine.Camera _camera; // 参考相机组件
        private PostProcessVolume _processVolume; // 后期处理体积

        private void Awake()
        {
            // 获取相机和后期处理组件
            _camera = GetComponent<UnityEngine.Camera>();
            _processVolume = GetComponent<PostProcessVolume>();
        }

        private void OnEnable()
        {
            // 订阅事件
            EventManager.Instance.PlayerRunning += PostProcess; // 玩家开始跑步时激活后期处理效果
            EventManager.Instance.PlayerRunStop += ProcessingStop; // 玩家停止跑步时禁用后期处理效果
        }

        private void OnDisable()
        {
            // 取消订阅事件
            EventManager.Instance.PlayerRunning -= PostProcess;
            EventManager.Instance.PlayerRunStop -= ProcessingStop;
        }

        // 启动后期处理效果
        private void PostProcess()
        {
            // 插值设置相机视野
            _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, 90f, Time.deltaTime * 10f);

            // 插值设置色差强度
            var chromaticAberration = _processVolume.profile.GetSetting<ChromaticAberration>();
            chromaticAberration.intensity.value = Mathf.Lerp(chromaticAberration.intensity.value, 1f, Time.deltaTime * 10f);
        }

        // 停止后期处理效果
        private void ProcessingStop()
        {
            // 插值设置色差强度归零
            var chromaticAberration = _processVolume.profile.GetSetting<ChromaticAberration>();
            chromaticAberration.intensity.value = Mathf.Lerp(chromaticAberration.intensity.value, 0f, Time.deltaTime * 10f);

            // 插值设置相机视野回到默认值
            _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, 60f, Time.deltaTime * 10f);
        }
    }
}