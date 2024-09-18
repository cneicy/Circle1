using UnityEngine;

namespace UI
{
    public class PausePanel : MonoBehaviour
    {
        [SerializeField] private GameObject pauseButton;

        private void Start()
        {
            Time.timeScale = 1;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Continue();
            }
        }

        public void Continue()
        {
            pauseButton.SetActive(true);
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            gameObject.SetActive(false);
        }

        public void Exit()
        {
            Application.Quit();
        }

        public void MainMenu()
        {
            Time.timeScale = 1;
        }
    }
}