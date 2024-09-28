namespace Event.EventArgs
{
    public class ItemDialogArgs : System.EventArgs
    {
        public string ItemName { get;private set; }
        
        public ItemDialogArgs(string nItemName)
        {
            ItemName = nItemName;
        }
    }
}