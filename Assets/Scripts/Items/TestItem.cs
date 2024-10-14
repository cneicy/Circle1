using Event;
using Items.Abstract;
using Items.Interface;

namespace Items
{
    // 测试物品类，继承自 ItemBase 并实现 IItem 接口
    public class TestItem : ItemBase, IItem
    {
        // 重写物品激活方法
        protected override void ActivateItem()
        {
            // 调用基类的激活方法
            base.ActivateItem();

            // 触发物品对话事件，使用物品名称作为参数
            EventManager.Instance.OnItemDialog(itemNameKey);
            EventManager.Instance.OnDialogPop(index);
            // 销毁当前物体
            Destroy(gameObject);
        }
    }
}