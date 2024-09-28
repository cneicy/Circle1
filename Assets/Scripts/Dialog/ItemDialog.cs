using Event;
using Event.EventArgs;
using Keyboard;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dialog
{
    public class ItemDialog : MonoBehaviour
    {
        public RawImage itemIcon;
        public TMP_Text itemName;
        public TMP_Text itemDescription;
        public GameObject panel;
        private string _itemName;
        private void OnEnable()
        {
            EventManager.Instance.ItemDialog+=DialogPop;
        }

        private void OnDisable()
        {
            EventManager.Instance.ItemDialog-=DialogPop;
        }

        private void DialogPop(ItemDialogArgs itemDialogArgs)
        {
            var itemText = DialogManager.Instance.GetItemText(itemDialogArgs.ItemName);
            if (itemText is null) return;
            panel.SetActive(true);
            itemIcon.texture = Resources.Load<Texture2D>("Item" + "/" + itemText.Name);
            itemName.text = itemText.Name;
            itemDescription.text = itemText.Description;
            Time.timeScale = 0;
        }
        
        private void Update()
        {
            if ((!panel.activeSelf || !Input.GetKeyDown(KeyCode.Escape)) &&
                !Input.GetKeyDown(KeySettingManager.Instance.GetKey("InterAct"))) return;
            Time.timeScale = 1;
            panel.SetActive(false);
        }
    }
}