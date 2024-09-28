using Event;
using Items.Abstract;
using Items.Interface;

namespace Items
{
    public class TestItem : ItemBase, IItem
    {
        protected override void ActivateItem()
        {
            base.ActivateItem();
            EventManager.Instance.OnDialogPop(index);
            EventManager.Instance.OnItemDialog(gameObject.name);
            Destroy(gameObject);
        }
    }
}