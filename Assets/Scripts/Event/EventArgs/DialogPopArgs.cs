namespace Event.EventArgs
{
    public class DialogPopArgs : System.EventArgs
    {
        public int Index { get; private set; }

        public DialogPopArgs(int index)
        {
            Index = index;
        }
    }
}