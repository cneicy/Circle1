using UnityEngine;

namespace Camera
{
    public class ScreenAspect : MonoBehaviour
    {
        //目标比例，默认16:9
        public float TargetAspect = 16f / 9f;
        private UnityEngine.Camera _mainCamera;

        private void Awake()
        {
            _mainCamera = UnityEngine.Camera.main;
            var windowAspect = Screen.width / (float)Screen.height;

            var scaleHeight = windowAspect / TargetAspect;

            if (scaleHeight < 1f)
            {
                var rect = _mainCamera.rect;

                rect.width = 1f;
                rect.height = scaleHeight;
                rect.x = 0;
                rect.y = (1f - scaleHeight) / 2f;

                _mainCamera.rect = rect;
            }
            else
            {
                var scaleWidth = 1f / scaleHeight;

                var rect = _mainCamera.rect;

                rect.width = scaleWidth;
                rect.height = 1f;
                rect.x = (1f - scaleWidth) / 2f;
                rect.y = 0;

                _mainCamera.rect = rect;
            }
        }
    }
}