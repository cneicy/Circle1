using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

namespace Keyboard
{
    public class KeySettingManager : MonoBehaviour
    {
        public List<KeyMapping> keyMappings = new();
        public string filePath;

        private static KeySettingManager _instance;

        public static KeySettingManager Instance
        {
            get
            {
                if (_instance)
                    return _instance;
                _instance = FindObjectOfType<KeySettingManager>() ??
                            new GameObject("KeySettingManager").AddComponent<KeySettingManager>();
                return _instance;
            }
        }

        public Vector2 Direction { get; private set; }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }

            DontDestroyOnLoad(gameObject);
            filePath = Application.persistentDataPath + "/" + "KeySetting.json";
            LoadKeySettings();
        }

        //加载键位设置
        private void LoadKeySettings()
        {
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                keyMappings = JsonConvert.DeserializeObject<List<KeyMapping>>(json);
            }
            else
            {
                //如果文件不存在，则创建默认键位设置
                keyMappings.Add(new KeyMapping("InterAct", KeyCode.E));
                keyMappings.Add(new KeyMapping("Left", KeyCode.A));
                keyMappings.Add(new KeyMapping("Right", KeyCode.D));
                keyMappings.Add(new KeyMapping("Up", KeyCode.W));
                keyMappings.Add(new KeyMapping("Down", KeyCode.S));
                keyMappings.Add(new KeyMapping("Run", KeyCode.LeftShift));
                SaveKeySettings();
            }
        }

        //保存键位设置
        private void SaveKeySettings()
        {
            var json = JsonConvert.SerializeObject(keyMappings, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        //获取键位
        public KeyCode GetKey(string actionName)
        {
            return (from mapping in keyMappings where mapping.actionName == actionName select mapping.keyCode)
                .FirstOrDefault();
        }

        //设置键位
        public void SetKey(string actionName, KeyCode newKeyCode)
        {
            foreach (var mapping in keyMappings.Where(mapping => mapping.actionName == actionName))
            {
                mapping.keyCode = newKeyCode;
                break;
            }

            SaveKeySettings();
        }

        //更新Direction
        private void Update()
        {
            if (Input.GetKey(GetKey("Left")))
            {
                Direction = new Vector2(-1, Direction.y);
            }

            if (Input.GetKey(GetKey("Right")))
            {
                Direction = new Vector2(1, Direction.y);
            }

            if (Input.GetKey(GetKey("Up")))
            {
                Direction = new Vector2(Direction.x, 1);
            }

            if (Input.GetKey(GetKey("Down")))
            {
                Direction = new Vector2(Direction.x, -1);
            }

            if (Input.GetKey(GetKey("Left")) && Input.GetKey(GetKey("Right")))
            {
                Direction = new Vector2(0, Direction.y);
            }

            if (!Input.GetKey(GetKey("Left")) && !Input.GetKey(GetKey("Right")))
            {
                Direction = new Vector2(0, Direction.y);
            }

            if (Input.GetKey(GetKey("Up")) && Input.GetKey(GetKey("Down")))
            {
                Direction = new Vector2(Direction.x, 0);
            }

            if (!Input.GetKey(GetKey("Up")) && !Input.GetKey(GetKey("Down")))
            {
                Direction = new Vector2(Direction.x, 0);
            }
        }
    }
}