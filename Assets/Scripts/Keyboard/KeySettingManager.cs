using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

namespace Keyboard
{
    // 管理键位设置，包括加载、保存和获取按键
    public class KeySettingManager : MonoBehaviour
    {
        public List<KeyMapping> keyMappings = new(); // 存储键位映射
        public string filePath; // 键位设置文件路径

        private static KeySettingManager _instance; // 单例实例

        // 单例属性
        public static KeySettingManager Instance
        {
            get
            {
                if (_instance) return _instance; // 如果实例已存在，返回实例
                // 否则，查找场景中的实例或创建新实例
                _instance = FindFirstObjectByType<KeySettingManager>() ?? 
                            new GameObject("KeySettingManager").AddComponent<KeySettingManager>();
                return _instance;
            }
        }

        public Vector2 Direction { get; private set; } // 存储方向

        private void Awake()
        {
            // 确保只有一个实例存在
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject); // 销毁重复的实例
            }
            else
            {
                _instance = this; // 设置单例实例
            }

            DontDestroyOnLoad(gameObject); // 确保在场景切换时不被销毁
            filePath = Application.persistentDataPath + "/" + "KeySetting.json"; // 设置文件路径
            LoadKeySettings(); // 加载键位设置
        }

        // 加载键位设置
        private void LoadKeySettings()
        {
            // 如果文件存在，读取并反序列化
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                keyMappings = JsonConvert.DeserializeObject<List<KeyMapping>>(json);
            }
            else
            {
                // 如果文件不存在，创建默认键位设置
                CreateDefaultKeyMappings();
                SaveKeySettings(); // 保存默认设置
            }
        }

        // 创建默认键位设置
        private void CreateDefaultKeyMappings()
        {
            keyMappings.Add(new KeyMapping("InterAct", KeyCode.E));
            keyMappings.Add(new KeyMapping("Left", KeyCode.A));
            keyMappings.Add(new KeyMapping("Right", KeyCode.D));
            keyMappings.Add(new KeyMapping("Up", KeyCode.W));
            keyMappings.Add(new KeyMapping("Down", KeyCode.S));
            keyMappings.Add(new KeyMapping("Run", KeyCode.LeftShift));
        }

        // 保存键位设置
        private void SaveKeySettings()
        {
            var json = JsonConvert.SerializeObject(keyMappings, Formatting.Indented);
            File.WriteAllText(filePath, json); // 写入JSON文件
        }

        // 获取指定动作的按键
        public KeyCode GetKey(string actionName)
        {
            return keyMappings.FirstOrDefault(mapping => mapping.actionName == actionName).keyCode; // 返回对应的按键
        }

        // 设置指定动作的按键
        public void SetKey(string actionName, KeyCode newKeyCode)
        {
            var mapping = keyMappings.FirstOrDefault(m => m.actionName == actionName);
            if (mapping != null)
            {
                mapping.keyCode = newKeyCode; // 更新键位
                SaveKeySettings(); // 保存更改
            }
        }

        // 更新方向
        private void Update()
        {
            // 重置方向为零
            Direction = Vector2.zero;

            // 根据按键更新方向
            if (Input.GetKey(GetKey("Left"))) Direction += Vector2.left;
            if (Input.GetKey(GetKey("Right"))) Direction += Vector2.right;
            if (Input.GetKey(GetKey("Up"))) Direction += Vector2.up;
            if (Input.GetKey(GetKey("Down"))) Direction += Vector2.down;
        }
    }
}
