using UnityEngine;

namespace Camera
{
    // 控制摄像机的类
    public class CameraController : MonoBehaviour
    {
        [Header("Camera Settings")]
        public float mouseSensitivity = 100.0f;  // 鼠标灵敏度
        public Transform playerBody;              // 玩家身体的 Transform 组件

        private float _xRotation;          // 摄像机的上下旋转角度

        private void Start()
        {
            // 锁定鼠标光标到屏幕中心
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            // 获取鼠标的水平和垂直移动值
            var mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            var mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            // 更新垂直旋转值并限制在 -90 到 90 度之间
            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

            // 应用旋转到摄像机
            transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

            // 旋转玩家身体
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}