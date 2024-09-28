using UnityEngine;

namespace UI
{
    public class PauseButton : MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenu;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Pause();
                Cursor.lockState = CursorLockMode.None;
            }
        }

        public void Pause()
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
            gameObject.SetActive(false);
        }
    }
}