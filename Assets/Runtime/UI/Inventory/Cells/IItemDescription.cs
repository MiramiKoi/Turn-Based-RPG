namespace Runtime.UI.Inventory.Cells
{
    public interface IItemDescription
    {
        public string Id { get; }
        public string Type { get; }
        public int StackSize { get; }
        public string ViewId { get; }
    }
}