namespace SimpleGame.GameCore.Map
{
    public abstract class InventoryItem
    {
        public virtual int Count { get; set; }
        public virtual int TextureId { get; set; }
    }
}