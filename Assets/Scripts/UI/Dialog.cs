using System.Collections.Generic;
using System.IO;
using Event;
using Event.EventArgs;
using Newtonsoft.Json;
using UnityEngine;

namespace UI
{
    public class Dialog : MonoBehaviour
    {
        [SerializeField] private string dialogsPath;
        [SerializeField] private string dialogJson;
        private Dictionary<int, string> _dialogsMap = new();

        private void Awake()
        {
            dialogsPath = Application.persistentDataPath + "/" + "Dialogs.json";

            if (File.Exists(dialogsPath))
            {
                dialogJson = File.ReadAllText(dialogsPath);
                _dialogsMap = JsonConvert.DeserializeObject<Dictionary<int, string>>(dialogJson);
            }
            else
            {
                _dialogsMap.Add(0, "New Dialog");
                dialogJson = JsonConvert.SerializeObject(_dialogsMap, Formatting.Indented);
                File.WriteAllText(dialogsPath, dialogJson);
            }
        }

        private void Start()
        {
            EventManager.Instance.DialogPop += OpenDialog;
        }

        public void OpenDialog(DialogPopArgs e)
        {
            Debug.Log(_dialogsMap[e.Index]);
        }
    }
}