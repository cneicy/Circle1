using System.Collections;
using Event;
using Event.EventArgs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dialog
{
    // 处理对话框的显示与输入效果
    public class BoxDialog : MonoBehaviour
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
        public TMP_Text textMeshPro; // 用于显示文本的TMP组件
        public float printTime; // 用于计时对话框的显示时间
        public float maxPrintTime = 10; // 最大显示时间
        public bool isTime2Break; // 是否达到关闭对话框的时间

        private string _eventToBeExc; // 要执行的事件
        private string _eventArg; // 事件参数

        [SerializeField] private GameObject dialogBox; // 对话框对象
        [SerializeField] private RawImage head; // 角色头像
        [SerializeField] private RawImage face; // 角色表情

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

            // 判断是否达到关闭条件
            Time2Break();

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
            yield return new WaitForSeconds(0.5f);
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

        // 判断是否达到关闭对话框的条件
        private void Time2Break()
        {
            if (!isTime2Break) return;
            _start = false; // 停止打字过程
            printTime = 0; // 重置计时
            isTime2Break = false; // 重置状态
            textMeshPro.text = ""; // 清空文本
            dialogBox.SetActive(false); // 隐藏对话框
        }

        // 开始打印对话内容
        private void StartPrinting(DialogPopArgs e)
        {
            // 检查对话框类型是否为"box"
            if (!DialogManager.Instance.GetDialogByIndex(e.Index).Type.Equals("box")) return;
            if (printTime != 0) return; // 防止重复打开

            // 显示对话框
            dialogBox.SetActive(true);
            _start = true; // 开始打字
            _currentText = ""; // 清空当前文本
            introduceText = DialogManager.Instance.GetDialogByIndex(e.Index).Content; // 获取对话内容
            
            // 获取并存储事件信息
            _eventToBeExc = DialogManager.Instance.GetDialogByIndex(e.Index).DialogEvent;
            _eventArg = DialogManager.Instance.GetDialogByIndex(e.Index).DialogEventArg;

            // 加载角色头像和表情
            LoadCharacterGraphics(e.Index);

            // 初始化状态
            _currentIndex = 0; // 重置索引
            _timer = 0f; // 重置计时器
            textMeshPro.fontSize = 80; // 设置字体大小
        }

        // 加载角色的头像和表情
        private void LoadCharacterGraphics(int dialogIndex)
        {
            var dialog = DialogManager.Instance.GetDialogByIndex(dialogIndex);
            head.texture = Resources.Load<Texture2D>("Character/Head/" + dialog.Character + "_Head");
            face.texture = Resources.Load<Texture2D>("Character/Face/" + dialog.Character + "_" + dialog.Animation + "_Face");
        }
    }
}
