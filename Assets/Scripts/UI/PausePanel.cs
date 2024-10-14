using UnityEngine;

namespace UI
{
    // 控制暂停面板的类
    public class PausePanel : MonoBehaviour
    {
        [SerializeField] private GameObject pauseButton; // 暂停按钮的游戏对象

        private void Start()
        {
            Time.timeScale = 1; // 确保游戏开始时时间流动
            gameObject.SetActive(false); // 初始时隐藏暂停面板
        }
        
        private void Update()
        {
            // 检测是否按下 Escape 键以继续游戏
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Continue(); // 调用继续游戏的方法
            }
        }

        // 继续游戏的方法
        public void Continue()
        {
            pauseButton.SetActive(true); // 显示暂停按钮
            Time.timeScale = 1; // 恢复游戏时间
            Cursor.lockState = CursorLockMode.Locked; // 锁定鼠标光标
            gameObject.SetActive(false); // 隐藏暂停面板
        }

        // 退出游戏的方法
        public void Exit()
        {
            Application.Quit(); // 退出应用程序
        }

        // 返回主菜单的方法（可扩展）
        public void MainMenu()
        {
            Time.timeScale = 1; // 确保时间流动恢复
            // todo 这里可以添加加载主菜单的逻辑
        }
    }
}