using System.Collections;
using Event;
using Event.EventArgs;
using TMPro;
using UnityEngine;

namespace Dialog
{
    // 处理屏幕对话框的显示与输入效果
    public class ScreenDialog : MonoBehaviour
    {
        [Header("Typing Settings")]
        [Tooltip("每个字符之间的打字速度")]
        public float typingSpeed = 0.05f; // 打字速度
        
        [Tooltip("对话内容")]
        public string introduceText; // 要显示的文本内容

        private string _currentText = ""; // 当前显示的文本
        private float _timer; // 计时器，用于控制打字速度
        private int _currentIndex; // 当前字符的索引
        private bool _start; // 控制打字过程的标志
        private bool _isBlinking; // 控制光标闪烁的标志

        public TMP_Text textMeshPro; // 用于显示文本的TMP组件
        public float printTime; // 用于计时对话框的显示时间
        public float maxPrintTime = 10; // 最大显示时间
        public bool isTime2Break; // 是否达到关闭对话框的时间
        
        private string _eventToBeExc; // 要执行的事件
        private string _eventArg; // 事件参数

        private void Update()
        {
            // 如果对话未开始，直接返回
            if (!_start) return;

            // 计时对话框的显示时间
            printTime += Time.deltaTime;
            if (printTime > maxPrintTime)
            {
                isTime2Break = true;
                StartCoroutine(Delay());
            }

            // 如果当前文本已全部显示，直接返回
            if (_currentText == introduceText) return;

            // 控制打字效果
            _timer += Time.deltaTime;
            if (_timer < typingSpeed) return; // 控制打字速度
            _timer = 0f; // 重置计时器

            // 显示下一个字符
            _currentText += introduceText[_currentIndex];
            textMeshPro.text = _currentText; // 更新显示文本
            _currentIndex++; // 移动到下一个字符
        }

        // 控制事件的延迟执行
        private IEnumerator Delay()
        {
            yield return new WaitForSeconds(1f); // 等待1秒后执行
            EventManager.Instance.EventSwitch(_eventToBeExc, _eventArg); // 执行事件
        }

        private void OnEnable()
        {
            // 订阅对话弹出事件
            EventManager.Instance.DialogPop += StartPrinting;
        }

        private void OnDisable()
        {
            // 取消订阅对话弹出事件
            EventManager.Instance.DialogPop -= StartPrinting;
        }

        // 光标闪烁协程
        private IEnumerator BlinkCursor()
        {
            while (true)
            {
                // 如果达到关闭条件，停止闪烁并重置文本
                if (isTime2Break)
                {
                    _start = false; // 停止打字过程
                    printTime = 0; // 重置计时
                    isTime2Break = false; // 重置状态
                    textMeshPro.fontSize = 48.7f; // 设置字体大小
                    textMeshPro.text = "+"; // 显示结束标志
                    break; // 退出循环
                }

                // 切换光标状态
                if (!_isBlinking)
                {
                    textMeshPro.text += "|"; // 添加光标
                    _isBlinking = true; // 设置闪烁状态
                }
                else
                {
                    textMeshPro.text = textMeshPro.text[..^1]; // 移除光标
                    _isBlinking = false; // 设置非闪烁状态
                }

                yield return new WaitForSeconds(0.3f); // 等待0.3秒
            }
        }

        // 开始打印对话内容
        private void StartPrinting(DialogPopArgs e)
        {
            // 检查对话框类型是否为"screen"
            if (!DialogManager.Instance.GetDialogByIndex(e.Index).Type.Equals("screen")) return;
            if (printTime != 0) return; // 防止重复打开

            // 开始打字过程
            _start = true;
            _currentText = ""; // 清空当前文本
            introduceText = DialogManager.Instance.GetDialogByIndex(e.Index).Content; // 获取对话内容
            
            // 获取并存储事件信息
            _eventToBeExc = DialogManager.Instance.GetDialogByIndex(e.Index).DialogEvent;
            _eventArg = DialogManager.Instance.GetDialogByIndex(e.Index).DialogEventArg;

            // 初始化状态
            _currentIndex = 0; // 重置索引
            _timer = 0f; // 重置计时器
            textMeshPro.fontSize = 80; // 设置字体大小
            
            // 启动光标闪烁协程
            StartCoroutine(BlinkCursor());
        }
    }
}
