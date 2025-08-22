using Venefica.Logic.Base.Items;

namespace Venefica.Logic.Base.InventoryLogic;

internal class PlayerInventory
{
    public Item Dragged { get; set; }
    public Item Saved { get; set; }

    public Item[] Hands;
    public Item[] Backpack;

    public int HandsAmount;
    public int BackpackSize;

    public PlayerInventory(int handsAmount, int backpackSize)
    {
        HandsAmount = handsAmount;
        BackpackSize = backpackSize;
        Hands = new Item[handsAmount];
        Backpack = new Item[backpackSize];
    }
}
