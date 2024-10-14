using UnityEngine;

namespace UI
{
    // 控制暂停按钮的类
    public class PauseButton : MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenu; // 暂停菜单的游戏对象

        private void Update()
        {
            // 检测是否按下 Escape 键并且游戏未暂停
            if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale != 0)
            {
                Pause(); // 调用暂停方法
                UnlockCursor(); // 解锁鼠标光标
            }
        }

        // 暂停游戏的方法
        public void Pause()
        {
            pauseMenu.SetActive(true); // 显示暂停菜单
            Time.timeScale = 0; // 将时间缩放设置为 0，暂停游戏
            gameObject.SetActive(false); // 隐藏暂停按钮
        }

        // 解锁鼠标光标的方法
        private void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None; // 设置光标状态为解锁
            Cursor.visible = true; // 确保光标可见
        }
    }
}