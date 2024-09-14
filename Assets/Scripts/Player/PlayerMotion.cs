using Event;
using Keyboard;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Player
{
    public class PlayerMotion : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private Animator animator;
        [SerializeField] private float maxVelocity;
        [SerializeField] private PostProcessVolume postProcessVolume;
        [SerializeField] private UnityEngine.Camera postProcessCamera;
        private Vector3 _velocity;
        private bool _isRunning;
        private bool _isWalking;

        private void Update()
        {
            Run();
        }

        private void Run()
        {
            _velocity = KeySettingManager.Instance.Direction.x * transform.right +
                        KeySettingManager.Instance.Direction.y * transform.forward;
            rb.AddForce(_velocity.normalized * (Time.deltaTime * 50), ForceMode.Impulse);
            if (_velocity == Vector3.zero)
            {
                var temp = rb.velocity;
                temp = Vector3.ClampMagnitude(temp, 2);
                temp.y = rb.velocity.y;
                rb.velocity = temp;
            }

            if (Input.GetKey(KeySettingManager.Instance.GetKey("Run")) &&
                KeySettingManager.Instance.Direction != Vector2.zero)
            {
                if (!_isRunning)
                {
                    _isRunning = true;
                    _isWalking = false;
                    EventManager.Instance.OnPlayerWalkStop();
                    EventManager.Instance.OnPlayerRunStart();
                }

                EventManager.Instance.OnPlayerRunning();
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity + 2);
            }

            if (!Input.GetKey(KeySettingManager.Instance.GetKey("Run")) &&
                KeySettingManager.Instance.Direction != Vector2.zero)
            {
                if (!_isWalking)
                {
                    _isRunning = false;
                    _isWalking = true;
                    EventManager.Instance.OnPlayerRunStop();
                    EventManager.Instance.OnPlayerWalkStart();
                }

                EventManager.Instance.OnPlayerWalking();
                rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
            }

            if (KeySettingManager.Instance.Direction == Vector2.zero)
            {
                EventManager.Instance.OnPlayerRunStop();
                EventManager.Instance.OnPlayerWalkStop();
            }
        }
    }
}