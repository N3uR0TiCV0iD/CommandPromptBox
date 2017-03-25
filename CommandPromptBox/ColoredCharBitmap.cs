using System;
using System.Drawing;
namespace HiT.CommandPromptBox
{
    internal class ColoredCharBitmap : IDisposable
    {
        Bitmap charBitmap;
        ConsoleColor color;
        internal ColoredCharBitmap(Bitmap charBitmap, ConsoleColor color)
        {
            this.charBitmap = charBitmap;
            this.Color = color;
        }
        internal Bitmap CharBitmap
        {
            get
            {
                return charBitmap;
            }
        }
        internal ConsoleColor Color
        {
            get
            {
                return color;
            }
            set
            {
                if (value != color)
                {
                    Color pixelColor = CommandPromptBox.GetColorFromConsoleColor(value);
                    for (int currYPos = 0; currYPos < 12; currYPos++)
                    {
                        for (int currXPos = 0; currXPos < 8; currXPos++)
                        {
                            if (charBitmap.GetPixel(currXPos, currYPos).A != 0)
                            {
                                charBitmap.SetPixel(currXPos, currYPos, pixelColor);
                            }
                        }
                    }
                    color = value;
                }
            }
        }
        public void Dispose()
        {
            this.charBitmap.Dispose();
        }
    }
}
