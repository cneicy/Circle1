using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Event;
using Event.EventArgs;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

namespace UI
{
    public class Dialog : MonoBehaviour
    {
        [SerializeField] private string dialogsPath;
        [SerializeField] private string dialogJson;
        private Dictionary<int, string> _dialogsMap = new();

        public float typingSpeed = 0.05f;
        public string IntroduceText;
        private string _currentText = "";
        private float _timer;
        private int _currentIndex;
        private bool _start;
        private bool _isBlinking;
        public TMP_Text textMeshPro;
        public float printTime;
        public float maxPrintTime=10;
        public bool isTime2Break;
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
                _dialogsMap.Add(0, "我会视奸你，永远...永远......");
                dialogJson = JsonConvert.SerializeObject(_dialogsMap, Formatting.Indented);
                File.WriteAllText(dialogsPath, dialogJson);
            }
        }

        private void Update()
        {
            if (!_start) return;
            printTime += Time.deltaTime;
            if (printTime > maxPrintTime)
            {
                isTime2Break = true;
            }
            if (_currentText == IntroduceText) return;
            _timer += Time.deltaTime;
            if (!(_timer >= typingSpeed)) return;
            _timer = 0f;
            _currentText += IntroduceText[_currentIndex];
            textMeshPro.text = _currentText;
            _currentIndex++;
        }
        
        private void Start()
        {
            EventManager.Instance.DialogPop += StartPrinting;
        }

        /*public void OpenDialog(DialogPopArgs e)
        {
            Debug.Log(_dialogsMap[e.Index]);
        }*/
        
        //闪烁光标
        private IEnumerator BlinkCursor()
        {
            while (true)
            {
                if (isTime2Break)
                {
                    _start = false;
                    printTime = 0;
                    isTime2Break = false;
                    textMeshPro.fontSize = 48.7f;
                    textMeshPro.text = "+";
                    break;
                }
                if (!_isBlinking)
                {
                    textMeshPro.text += "|";
                    _isBlinking = true;
                }
                else
                {
                    textMeshPro.text = textMeshPro.text[..^1];
                    _isBlinking = false;
                }
                yield return new WaitForSeconds(0.3f); // 闪烁速度
            }
        }

        // 调用此方法开始打印文字
        private void StartPrinting(DialogPopArgs e)
        {
            if(printTime != 0) return;
            _start = true;
            _currentText = "";
            
            
            IntroduceText = _dialogsMap[e.Index];
            _currentIndex = 0;
            _timer = 0f;
            textMeshPro.fontSize = 80;
            StartCoroutine(BlinkCursor());
        }
    }
}