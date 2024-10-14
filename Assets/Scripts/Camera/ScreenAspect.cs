using UnityEngine;
using UnityEngine.Serialization;

namespace Camera
{
    // 处理屏幕的纵横比，以确保游戏在不同分辨率下的正确显示
    public class ScreenAspect : MonoBehaviour
    {
        [FormerlySerializedAs("TargetAspect")]
        [Header("Target Aspect Ratio")]
        [Tooltip("目标纵横比，默认设置为16:9")]
        public float targetAspect = 16f / 9f; // 默认目标比例

        private UnityEngine.Camera _mainCamera; // 主相机引用

        private void Awake()
        {
            // 获取主相机
            _mainCamera = UnityEngine.Camera.main;

            // 计算当前窗口的纵横比
            var windowAspect = Screen.width / (float)Screen.height;

            // 计算缩放高度
            var scaleHeight = windowAspect / targetAspect;

            // 根据缩放比例调整相机的视口
            if (scaleHeight < 1f)
            {
                // 纵向拉伸的情况
                SetCameraRect(1f, scaleHeight, 0, (1f - scaleHeight) / 2f);
            }
            else
            {
                // 横向拉伸的情况
                var scaleWidth = 1f / scaleHeight;
                SetCameraRect(scaleWidth, 1f, (1f - scaleWidth) / 2f, 0);
            }
        }

        // 设置相机视口的矩形
        private void SetCameraRect(float width, float height, float x, float y)
        {
            var rect = _mainCamera.rect; // 获取相机的当前视口矩形

            rect.width = width; // 设置视口宽度
            rect.height = height; // 设置视口高度
            rect.x = x; // 设置视口X位置
            rect.y = y; // 设置视口Y位置

            _mainCamera.rect = rect; // 应用新的视口矩形
        }
    }
}