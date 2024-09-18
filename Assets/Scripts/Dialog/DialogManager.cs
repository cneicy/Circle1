using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Dialog
{
    public class Dialog
    {
        public readonly int Index;
        public readonly string Content;
        public readonly string Type;
        public readonly string Animation;
        public readonly string Character;
        public readonly string DialogEvent;
        public readonly string DialogEventArg;

        public Dialog(int index, string content, string type, string animation, string character, string dialogEvent,
            string dialogEventArg)
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

    public class DialogManager : MonoBehaviour
    {
        private readonly List<Dialog> _dialog = new();
        private static DialogManager _instance;

        public static DialogManager Instance
        {
            get
            {
                if (_instance)
                    return _instance;
                _instance = FindObjectOfType<DialogManager>() ??
                            new GameObject("DialogData").AddComponent<DialogManager>();
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                LoadCsv("Dialog/DialogData");
                _instance = this;
            }
        }

        private void LoadCsv(string resourcePath)
        {
            var textAsset = Resources.Load<TextAsset>(resourcePath);
            if (textAsset == null)
            {
                Debug.LogError($"Unable to find CSV file at path: {resourcePath}");
                return;
            }

            var lines = textAsset.text.Split('\n');
            for (var i = 1; i < lines.Length; i++)
            {
                var values = lines[i].Split(',');
                if (values.Length < 7) continue;
                var dialog = new Dialog(
                    int.Parse(values[0]),
                    values[1],
                    values[2],
                    values[3],
                    values[4],
                    values[5],
                    values[6]
                );

                _dialog.Add(dialog);
            }
        }

        public Dialog GetDialogByIndex(int index)
        {
            foreach (var dialog in _dialog.Where(dialogue => dialogue.Index == index))
            {
                return dialog;
            }

            Debug.LogWarning($"Dialog with index {index} not found.");
            return null;
        }
    }
}