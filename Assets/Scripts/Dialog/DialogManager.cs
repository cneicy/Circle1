using System.Collections.Generic;
using UnityEngine;
using Localization;

namespace Dialog
{
    // 对话类，包含对话的相关信息
    public class Dialog
    {
        public readonly int Index; // 对话索引
        public string Content; // 对话内容（会被本地化处理）
        public readonly string Type; // 对话类型
        public readonly string Animation; // 动画效果
        public readonly string Character; // 角色名称
        public readonly string DialogEvent; // 对话事件
        public readonly string DialogEventArg; // 对话事件参数

        public Dialog(int index, string content, string type, string animation, string character, string dialogEvent, string dialogEventArg)
        {
            Index = index;
            Content = content;
            Type = type;
            Animation = animation;
            Character = character;
            DialogEvent = dialogEvent;
            DialogEventArg = dialogEventArg;
        }
    }

    // 物品文本类，包含物品的名称和描述
    public class ItemText
    {
        public string Name; // 物品名称
        public string Description; // 物品描述

        public ItemText(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }

    // 对话管理器，负责加载和管理对话和物品文本
    public class DialogManager : MonoBehaviour
    {
        // 存储对话和物品文本的字典
        private readonly Dictionary<int, Dialog> _dialogDictionary = new();
        private readonly Dictionary<string, ItemText> _itemTextDictionary = new();
        private static DialogManager _instance;

        public static DialogManager Instance
        {
            get
            {
                // 确保只存在一个对话管理器实例
                if (_instance != null) return _instance;
                _instance = FindFirstObjectByType<DialogManager>() ?? 
                            new GameObject("DialogManager").AddComponent<DialogManager>();
                return _instance;
            }
        }

        private void Awake()
        {
            // 初始化对话管理器
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject); // 如果已存在，则销毁当前对象
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(gameObject); // 保持在场景切换中不销毁
                LoadData("Dialog/DialogData", DataType.Dialog); // 加载对话数据
                LoadData("Dialog/ItemText", DataType.ItemText); // 加载物品文本数据
            }
        }

        // 数据类型枚举，用于区分加载的数据类型
        private enum DataType
        {
            Dialog,
            ItemText
        }

        // 加载指定路径的数据
        private void LoadData(string resourcePath, DataType dataType)
        {
            var textAsset = Resources.Load<TextAsset>(resourcePath);
            if (textAsset == null)
            {
                Debug.LogError($"Unable to find CSV file at path: {resourcePath}");
                return;
            }

            // 按行读取文本
            var lines = textAsset.text.Split('\n');
            for (var i = 1; i < lines.Length; i++)
            {
                var values = lines[i].Split(',');
                switch (dataType)
                {
                    case DataType.Dialog:
                        // 加载对话数据
                        if (values.Length < 7) continue; // 检查字段数量
                        var dialog = new Dialog(
                            int.Parse(values[0].Trim()),
                            values[1].Trim(),
                            values[2].Trim(),
                            values[3].Trim(),
                            values[4].Trim(),
                            values[5].Trim(),
                            values[6].Trim()
                        );
                        _dialogDictionary[dialog.Index] = dialog; // 添加到字典
                        dialog.Content = LocalizationManager.Instance.GetLocalizedValue(dialog.Content); // 本地化对话内容
                        break;

                    case DataType.ItemText:
                        // 加载物品文本数据
                        if (values.Length < 2) continue; // 检查字段数量
                        var itemNameKey = values[0].Trim();
                        var itemDescriptionKey = values[1].Trim();
                        var itemText = new ItemText(itemNameKey, itemDescriptionKey);
                        _itemTextDictionary[itemNameKey] = itemText; // 添加到字典
                        itemText.Name = LocalizationManager.Instance.GetLocalizedValue(itemText.Name); // 本地化物品名称
                        itemText.Description = LocalizationManager.Instance.GetLocalizedValue(itemText.Description); // 本地化物品描述
                        break;
                }
            }
        }

        // 根据索引获取对话
        public Dialog GetDialogByIndex(int index)
        {
            if (_dialogDictionary.TryGetValue(index, out var dialog))
            {
                return dialog; // 返回找到的对话
            }

            Debug.LogWarning($"Dialog with index {index} not found.");
            return null; // 如果未找到则返回 null
        }

        // 根据名称获取物品文本
        public ItemText GetItemText(string name)
        {
            if (_itemTextDictionary.TryGetValue(name, out var itemText))
            {
                return itemText; // 返回找到的物品文本
            }

            Debug.LogWarning($"ItemText with name {name} not found.");
            return null; // 如果未找到则返回 null
        }
    }
}
