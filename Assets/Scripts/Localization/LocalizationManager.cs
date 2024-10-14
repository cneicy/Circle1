using System.Collections.Generic;
using UnityEngine;

namespace Localization
{
    // 语言枚举
    public enum Language
    {
        en_us,
        zh_cn
    }

    // 本地化管理器类，用于加载和获取本地化文本
    public class LocalizationManager : MonoBehaviour
    {
        // 单例实例
        public static LocalizationManager Instance { get; private set; }
        
        // 默认语言设置
        public Language defaultLanguage = Language.zh_cn;

        // 存储本地化文本的字典
        private readonly Dictionary<string, string> _localizedText = new();

        // Awake 方法，初始化单例并加载默认语言
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject); // 如果实例已存在，销毁当前对象
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // 保持在场景切换中不销毁
                LoadLanguage(defaultLanguage); // 加载默认语言
            }
        }

        // 加载指定语言的文本文件
        public void LoadLanguage(Language language)
        {
            _localizedText.Clear(); // 清空现有文本

            // 从资源中加载语言文件
            var textAsset = Resources.Load<TextAsset>($"Language/{language}");
            if (textAsset == null)
            {
                Debug.LogError($"Unable to find language file for: {language}");
                return;
            }

            // 按行读取文本并填充字典
            var lines = textAsset.text.Split('\n');
            foreach (var line in lines)
            {
                var keyValue = line.Split('=');
                if (keyValue.Length == 2)
                {
                    // 去除多余空格并添加到字典中
                    _localizedText[keyValue[0].Trim()] = keyValue[1].Trim();
                }
            }
        }

        // 根据键获取本地化的值
        public string GetLocalizedValue(string key)
        {
            // 尝试从字典中获取对应的值
            if (_localizedText.TryGetValue(key, out var value))
            {
                return value;
            }

            // 如果找不到对应的值，打印警告并返回原 key
            Debug.LogWarning($"Localization key '{key}' not found.");
            return key;
        }
    }
}
