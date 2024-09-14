using Event;
using Keyboard;
using UnityEngine;

namespace Camera
{
    public class CameraInterAct : MonoBehaviour
    {
        [SerializeField] private int maxInterActDistance = 10;
        [SerializeField] private GameObject target;


        private void Update()
        {
            Physics.Raycast(transform.position, transform.forward, out var raycastHit, maxInterActDistance);
            if (raycastHit.collider && Input.GetKeyDown(KeySettingManager.Instance.GetKey("InterAct")))
                EventManager.Instance.OnCameraInterAct(raycastHit.collider.gameObject);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * maxInterActDistance);
        }
    }
}