using Event;
using UnityEngine;

namespace Camera
{
    public class CameraShake : MonoBehaviour
    {
        public Transform cameraTransform;
        public float amplitude = 0.05f;
        public float frequency = 10.0f;

        private Vector3 _originalPos;
        private bool _isRunning;
        private bool _isWalking;

        private void Start()
        {
            _originalPos = cameraTransform.localPosition;
        }

        private void OnEnable()
        {
            EventManager.Instance.PlayerRunning += OnPlayerRunning;
            EventManager.Instance.PlayerRunStop += StopRunning;
            EventManager.Instance.PlayerWalking += OnPlayerWalking;
            EventManager.Instance.PlayerWalkStop += StopWalking;
        }

        private void OnDisable()
        {
            EventManager.Instance.PlayerRunning -= OnPlayerRunning;
            EventManager.Instance.PlayerRunStop -= StopRunning;
            EventManager.Instance.PlayerWalking -= OnPlayerWalking;
            EventManager.Instance.PlayerWalkStop -= StopWalking;
        }

        private void OnPlayerRunning()
        {
            _isRunning = true;
        }

        private void OnPlayerWalking()
        {
            _isWalking = true;
        }

        private void StopRunning()
        {
            _isRunning = false;
        }

        private void StopWalking()
        {
            _isWalking = false;
        }

        private void Update()
        {
            if (_isRunning)
            {
                var xShake = Mathf.Sin(Time.time * frequency) * amplitude;
                var yShake = Mathf.Cos(Time.time * frequency * 2) * amplitude * 0.5f;

                cameraTransform.localPosition = _originalPos + new Vector3(xShake, yShake, 0);
            }
            else
            {
                if (_isWalking)
                {
                    var xShake = Mathf.Sin(Time.time * frequency / 2) * amplitude;
                    var yShake = Mathf.Cos(Time.time * frequency / 2 * 2) * amplitude * 0.5f;

                    cameraTransform.localPosition = _originalPos + new Vector3(xShake, yShake, 0);
                }
                else
                    cameraTransform.localPosition = _originalPos;
            }
        }
    }
}