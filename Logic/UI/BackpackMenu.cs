using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Venefica.Logic.Base.Items;
using Venefica.Logic.Graphics;
using Venefica.Logic.Base;

namespace Venefica.Logic.UI;

internal class BackpackMenu : UiObject
{
    private Item[] _playerBackpack;
    public int BackpackSize { get; set; }
    public Slot[] BackpackSlots { get; set; }

    public BackpackMenu(ContentManager content, Player player)
    {
        IsEnabled = true;

        _playerBackpack = player.Inventory.Backpack;
        BackpackSize = _playerBackpack.Length;
        BackpackSlots = new Slot[BackpackSize];

        Sprite slotSprite = new("backpack_slot", content, 32);
        for (int i = 0; i < BackpackSize; i++)
        {
            Slot slot = new(Vector2.Zero, slotSprite, _playerBackpack[i]);
            BackpackSlots[i] = slot;
        }  
        LayourManager.PlaceElements("W", BackpackSlots);
    }

    public override void Update(Vector2 cursorPosition, Player player, float deltaTime)
    {
        for (int i = 0; i < _playerBackpack.Length; i++)
        {
            Slot currentSlot = BackpackSlots[i];
            currentSlot.Item = _playerBackpack[i];

            if (currentSlot.RectDst.Contains(cursorPosition))
            {
                currentSlot.Alpha = 1;
                if (UiManager.IsMouseClicked(mouse => mouse.LeftButton))
                {
                    HandleSlotsInteraction(currentSlot, i, UiManager.DraggedSlot);
                }             
            }
            else currentSlot.Alpha = 0.2f;
        }
    }

    private void HandleSlotsInteraction(Slot currentSlot, int currentSlotIndex, Slot draggedSlot)
    {
        if ((currentSlot.Item != null) && (draggedSlot.Item == null))
        {
            UiManager.SaveSlot(_playerBackpack, currentSlotIndex);
            draggedSlot.Item = currentSlot.Item;
            _playerBackpack[currentSlotIndex] = null;
        }

        else if ((currentSlot.Item == null) && (draggedSlot.Item != null))
        {
            _playerBackpack[currentSlotIndex] = draggedSlot.Item;
            draggedSlot.Item = null;
        }

        else if ((currentSlot.Item != null) && (draggedSlot.Item != null))
        {
            UiManager.SavedArray[UiManager.SavedArrayIndex] = _playerBackpack[currentSlotIndex];
            _playerBackpack[currentSlotIndex] = draggedSlot.Item;
            draggedSlot.Item = null;
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < _playerBackpack.Length; i++)
        {
            BackpackSlots[i].Draw(spriteBatch);
        }
    }
}
