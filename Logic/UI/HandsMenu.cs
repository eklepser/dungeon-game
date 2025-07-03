using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Venefica.Logic.Base;
using Venefica.Logic.Base.Items;
using Venefica.Logic.Graphics;

namespace Venefica.Logic.UI;

internal class HandsMenu : UiObject
{
    //private Sprite _backgroundSprite;
    private Item[] _playerHands;
    private List<float> _lastAnimTimeList = new();
    public int HandsAmount { get; set; }
    public Slot[] HandSlots { get; set; }

    public HandsMenu(ContentManager content, Player player)
    {
        IsEnabled = true;
        Alpha = 0.8f;

        _playerHands = player.Inventory.Hands;
        HandsAmount = _playerHands.Length;
        HandSlots = new Slot[HandsAmount];

        //_backgroundSprite = new("backpack", content);

        Sprite slotSprite = new("hand_slot", content, 32);
        for (int i = 0; i < HandsAmount; i++)
        {
            Slot slot = new(content, Vector2.Zero, slotSprite, _playerHands[i]);
            HandSlots[i] = slot;
            _lastAnimTimeList.Add(0);
        }
        LayoutManager.PlaceElements("S", HandSlots);
    }

    public override void Update(Vector2 cursorPosition, Player player, float deltaTime)
    {
        PlayHandsAnimation(deltaTime);
        for (int i = 0; i < _playerHands.Length; i++)
        {
            Slot currentSlot = HandSlots[i];
            currentSlot.Update(_playerHands[i]);

            if (currentSlot.RectDst.Contains(cursorPosition))
            {
                currentSlot.ShowTooltip();
                if (UiManager.IsMouseClicked(mouse => mouse.LeftButton))
                {
                    HandleSlotsInteraction(currentSlot, i, UiManager.DraggedSlot);
                }
            }
            else currentSlot.HideTooltip();
        }
    }

    private void HandleSlotsInteraction(Slot currentSlot, int currentSlotIndex, Slot draggedSlot)
    {
        if ((currentSlot.Item != null) && (draggedSlot.Item == null))
        {
            UiManager.SaveSlot(_playerHands, currentSlotIndex);
            draggedSlot.Update(currentSlot.Item);
            _playerHands[currentSlotIndex] = null;
        }

        else if ((currentSlot.Item == null) && (draggedSlot.Item != null))
        {
            _playerHands[currentSlotIndex] = draggedSlot.Item;
            draggedSlot.Update(null);
        }

        else if ((currentSlot.Item != null) && (draggedSlot.Item != null))
        {
            UiManager.SavedArray[UiManager.SavedArrayIndex] = _playerHands[currentSlotIndex];
            _playerHands[currentSlotIndex] = draggedSlot.Item;
            draggedSlot.Update(null);
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        //spriteBatch.Draw(_backgroundSprite.Texture, new Rectangle(0, 0, 100, 440), _backgroundSprite.RectSrc, new Color(Color.White, Alpha));
        for (int i = 0; i < _playerHands.Length; i++)
        {
            HandSlots[i].Draw(spriteBatch);
        }
    }

    private void PlayHandsAnimation(float deltaTime)
    {
        for (int i = 0; i < HandsAmount; i++)
        {
            if (_playerHands[i] is Staff staff)
            {
                // start animation
                if (_lastAnimTimeList[i] - staff.LastShootTime < 0)
                {
                    HandSlots[i].Alpha = 0.4f;
                    _lastAnimTimeList[i] = staff.LastShootTime;
                }
                else if ((_lastAnimTimeList[i] == staff.LastShootTime) && (HandSlots[i].Alpha < 0.8f))
                {
                    HandSlots[i].Alpha += staff.AttackSpeed * deltaTime * 0.4f;
                }
            }
        }
    }
}
