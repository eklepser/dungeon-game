using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Text;
using System.Threading.Tasks;
using Venefica.Logic.Base;

namespace Venefica.Logic.UI;

internal static class LayourManager
{
    public static void PlaceElements(string direction, Slot[] slots)
    {
        int screenWidth = Constants.ScreenWidth;
        int screenHeight = Constants.ScreenHeight;
        int slotSize = slots[0].RectDst.Width;
        int slotsAmount = slots.Length;
        int step = 4;
        int startPosX = 0;
        int startPosY = 0;
        int posX = 0;
        int posY = 0;

        switch (direction)
        {
            case "N":
                startPosX = (screenWidth - (slotSize + step) * (slotsAmount + 1)) / 2 - slotSize / 2 - step / 2;
                posX = startPosX;
                posY = step;
                for (int i = 0; i < slotsAmount; i++)
                {
                    posX += slotSize + step;
                    slots[i].Position = new Vector2(posX, posY);
                }
                break;

            case "E":
                startPosY = (screenHeight - (slotSize + step) * (slotsAmount + 1)) / 2 - slotSize / 2 - step / 2;
                posY = startPosY;
                posX = screenWidth - slotSize - step;
                for (int i = 0; i < slotsAmount; i++)
                {
                    posY += slotSize + step;
                    slots[i].Position = new Vector2(posX, posY);
                }
                break;

            case "S":
                startPosX = (screenWidth - (slotSize + step) * (slotsAmount + 1)) / 2 - slotSize / 2 - step / 2;
                posX = startPosX;
                posY = screenHeight - slotSize - step;
                for (int i = 0; i < slotsAmount; i++)
                {
                    posX += slotSize + step;
                    slots[i].Position = new Vector2(posX, posY);
                }
                break;

            case "W":
                startPosY = (screenHeight - (slotSize + step) * (slotsAmount + 1)) / 2 - slotSize / 2 - step / 2;
                posY = startPosY;
                posX = step;
                for (int i = 0; i < slotsAmount; i++)
                {
                    posY += slotSize + step;
                    slots[i].Position = new Vector2(posX, posY);
                }
                break;
        }
    }
}
