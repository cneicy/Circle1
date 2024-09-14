using Event;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Camera
{
    public class CameraPostProcessing : MonoBehaviour
    {
        private UnityEngine.Camera _camera;
        private PostProcessVolume _processVolume;

        private void Awake()
        {
            _camera = GetComponent<UnityEngine.Camera>();
            _processVolume = GetComponent<PostProcessVolume>();
        }

        private void Start()
        {
            EventManager.Instance.PlayerRunning += PostProcess;
            EventManager.Instance.PlayerRunStop += ProcessingStop;
        }

        private void PostProcess()
        {
            _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, 90f, Time.deltaTime * 10f);
            _processVolume.profile.GetSetting<ChromaticAberration>().intensity.value = Mathf.Lerp(
                _processVolume.profile.GetSetting<ChromaticAberration>().intensity.value, 1f, Time.deltaTime * 10f);
        }

        private void ProcessingStop()
        {
            _processVolume.profile.GetSetting<ChromaticAberration>().intensity.value = Mathf.Lerp(
                _processVolume.profile.GetSetting<ChromaticAberration>().intensity.value, 0f, Time.deltaTime * 10f);
            _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, 60f, Time.deltaTime * 10f);
        }
    }
}