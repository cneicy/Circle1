using System.Collections;
using Event;
using Event.EventArgs;
using TMPro;
using UnityEngine;

namespace Dialog
{
    public class ScreenDialog : MonoBehaviour
    {
        public float typingSpeed = 0.05f;
        public string introduceText;
        private string _currentText = "";
        private float _timer;
        private int _currentIndex;
        private bool _start;
        private bool _isBlinking;
        public TMP_Text textMeshPro;
        public float printTime;
        public float maxPrintTime = 10;
        public bool isTime2Break;
        private string _eventToBeExc;
        private string _eventArg;

        private void Update()
        {
            if (!_start) return;
            printTime += Time.deltaTime;
            if (printTime > maxPrintTime)
            {
                isTime2Break = true;
                StartCoroutine(Delay());
            }

            if (_currentText == introduceText)
            {
                return;
            }

            _timer += Time.deltaTime;
            if (!(_timer >= typingSpeed)) return;
            _timer = 0f;
            _currentText += introduceText[_currentIndex];
            textMeshPro.text = _currentText;
            _currentIndex++;
        }

        // 沟槽的生命周期
        private IEnumerator Delay()
        {
            yield return new WaitForSeconds(1f);
            EventManager.Instance.DialogEventSwitch(_eventToBeExc, _eventArg);
        }

        private void Start()
        {
            EventManager.Instance.DialogPop += StartPrinting;
        }

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

                yield return new WaitForSeconds(0.3f);
            }
        }

        private void StartPrinting(DialogPopArgs e)
        {
            if (!DialogManager.Instance.GetDialogByIndex(e.Index).Type.Equals("screen")) return;
            if (printTime != 0) return;
            _start = true;
            _currentText = "";
            introduceText = DialogManager.Instance.GetDialogByIndex(e.Index).Content;
            _eventToBeExc = DialogManager.Instance.GetDialogByIndex(e.Index).DialogEvent;
            _eventArg = DialogManager.Instance.GetDialogByIndex(e.Index).DialogEventArg;
            _currentIndex = 0;
            _timer = 0f;
            textMeshPro.fontSize = 80;
            StartCoroutine(BlinkCursor());
        }
    }
}