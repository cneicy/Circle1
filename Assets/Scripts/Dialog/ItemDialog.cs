using Event;
using Event.EventArgs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dialog
{
    // 物体展示框类，用于显示物品的详细信息（图标、名称、描述）
    public class ItemDialog : MonoBehaviour
    {
        public RawImage itemIcon;          // 物体图标
        public TMP_Text itemName;          // 物体名称文本
        public TMP_Text itemDescription;   // 物体描述文本
        public GameObject panel;           // 物体展示框的UI面板

        // 事件订阅
        private void OnEnable()
        {
            EventManager.Instance.ItemDialog += DialogPop;
        }

        // 取消订阅事件
        private void OnDisable()
        {
            EventManager.Instance.ItemDialog -= DialogPop;
        }
        
        // 物体展示框弹出，接收物体对话参数
        private void DialogPop(ItemDialogArgs itemDialogArgs)
        {
            // 从DialogManager尝试获取物体文本信息
            var itemText = DialogManager.Instance.GetItemText(itemDialogArgs.ItemName);
            
            // 如果没有找到对应的物体文本，直接返回
            if (itemText == null) return;

            // 显示物体展示框
            panel.SetActive(true);

            // 加载物体图标，并更新UI文本
            LoadItemIcon(itemText.Name);
            UpdateItemText(itemText);
            
            // 暂停游戏时间
            Time.timeScale = 0;
        }

        // 加载物体图标
        private void LoadItemIcon(string itemName)
        {
            string iconPath = $"Item/{itemName}"; // 更灵活的图标路径
            itemIcon.texture = Resources.Load<Texture2D>(iconPath);
        }

        // 更新物体的名称和描述文本
        private void UpdateItemText(ItemText itemText)
        {
            itemName.text = itemText.Name;
            itemDescription.text = itemText.Description;
        }
        
        private void Update()
        {
            // 检测是否关闭物体展示框
            if ((!panel.activeSelf || !Input.GetKeyDown(KeyCode.Escape)) && Time.timeScale == 0) return;

            // 恢复游戏时间并隐藏展示框
            Time.timeScale = 1;
            panel.SetActive(false);
        }
    }
}