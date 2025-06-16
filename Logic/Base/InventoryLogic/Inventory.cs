using System.Collections.Generic;
using Venefica.Logic.Base.Items;

namespace Venefica.Logic.Base.InventoryLogic;

internal class Inventory
{
    public Item Dragged { get; set; }
    public Item[] Hands = new Item[2];
    public Item[] Backpack = new Item[6];

    public int HandsAmount = 2;
    public int BackpackSize = 6;
}
