using System.Collections;
using Event;
using Event.EventArgs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dialog
{
    public class BoxDialog : MonoBehaviour
    {
        public float typingSpeed = 0.05f;
        public string introduceText;
        private string _currentText = "";
        private float _timer;
        private int _currentIndex;
        private bool _start;
        public TMP_Text textMeshPro;
        public float printTime;
        public float maxPrintTime = 10;
        public bool isTime2Break;
        private string _eventToBeExc;
        private string _eventArg;
        [SerializeField] private GameObject dialogBox;
        [SerializeField] private RawImage head;
        [SerializeField] private RawImage face;

        private void Update()
        {
            if (!_start) return;
            printTime += Time.deltaTime;
            if (printTime > maxPrintTime)
            {
                isTime2Break = true;
                StartCoroutine(Delay());
            }

            Time2Break();
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
            yield return new WaitForSeconds(0.5f);
            EventManager.Instance.DialogEventSwitch(_eventToBeExc, _eventArg);
        }

        private void Start()
        {
            EventManager.Instance.DialogPop += StartPrinting;
        }

        private void Time2Break()
        {
            if (!isTime2Break) return;
            _start = false;
            printTime = 0;
            isTime2Break = false;
            textMeshPro.text = "";
            dialogBox.SetActive(false);
        }

        private void StartPrinting(DialogPopArgs e)
        {
            if (!DialogManager.Instance.GetDialogByIndex(e.Index).Type.Equals("box")) return;
            if (printTime != 0) return;
            dialogBox.SetActive(true);
            _start = true;
            _currentText = "";
            introduceText = DialogManager.Instance.GetDialogByIndex(e.Index).Content;
            _eventToBeExc = DialogManager.Instance.GetDialogByIndex(e.Index).DialogEvent;
            _eventArg = DialogManager.Instance.GetDialogByIndex(e.Index).DialogEventArg;

            head.texture = Resources.Load<Texture2D>("Character/Head" + "/" +
                                                     DialogManager.Instance.GetDialogByIndex(e.Index).Character +
                                                     "_Head");
            face.texture = Resources.Load<Texture2D>("Character/Face" + "/" +
                                                     DialogManager.Instance.GetDialogByIndex(e.Index).Character + "_" +
                                                     DialogManager.Instance.GetDialogByIndex(e.Index).Animation +
                                                     "_Face");
            _currentIndex = 0;
            _timer = 0f;
            textMeshPro.fontSize = 80;
        }
    }
}