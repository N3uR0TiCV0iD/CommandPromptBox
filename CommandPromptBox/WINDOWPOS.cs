using System;
using System.Drawing;
using System.Runtime.InteropServices;
namespace HiT.CommandPromptBox
{
    internal enum WMSZ
    {
        BOTTOM = 6,
        BOTTOMLEFT = 7,
        BOTTOMRIGHT = 8,
        LEFT = 1,
        RIGHT = 2,
        TOP = 3,
        TOPLEFT = 4,
        TOPRIGHT = 5
    }
    internal struct WINDOWPOS
    {
        #pragma warning disable 0649
        public IntPtr hwnd;
        public IntPtr hwndInsertAfter;
        public int x;
        public int y;
        public int cx;
        public int cy;
        public uint flags;
        #pragma warning restore 0649

        public const int WM_MOVING = 0x216;
        public const int WM_SIZING = 0x214;
        public const int WM_EXITSIZEMOVE = 0x232;
        public const int WM_GETMINMAXINFO = 0x24;
        public const int WM_ENTERSIZEMOVE = 0x231;
        public const int WM_WINDOWPOSCHANGING = 0x46;
        public static void FixWINDOWPOS(IntPtr lParam, WMSZ sizeOperation, Point startLocation, bool horizontalScroll, bool verticalScroll)
        {
            WINDOWPOS windowPos = (WINDOWPOS)Marshal.PtrToStructure(lParam, typeof(WINDOWPOS));
            bool updateWindowPos = false;
            /*
            if (horizontalScroll)
            {
                windowPos.cx -= 17;
            }
            if (verticalScroll)
            {
                windowPos.cy -= 17;
            }
            */
            if (windowPos.cx % 8 != 0)
            {
                windowPos.cx = (windowPos.cx / 8) * 8;
                updateWindowPos = true;
            }
            if (windowPos.cy % 12 != 0)
            {
                windowPos.cy = (windowPos.cy / 12) * 12;
                updateWindowPos = true;
            }
            switch (sizeOperation)
            {
                case WMSZ.LEFT:
                case WMSZ.BOTTOMLEFT:
                    int xDifference;
                    xDifference = windowPos.x - startLocation.X;
                    if (xDifference % 8 != 0)
                    {
                        windowPos.x = startLocation.X + ((xDifference / 8) * 8);
                        updateWindowPos = true;
                        if (xDifference > 0)
                        {
                            windowPos.cx += 8;
                        }
                    }
                break;
                case WMSZ.TOP:
                case WMSZ.TOPRIGHT:
                    updateWindowPos = TOPSizeOperation(ref windowPos, startLocation);
                break;
                case WMSZ.TOPLEFT:
                    updateWindowPos = TOPSizeOperation(ref windowPos, startLocation);
                    goto case WMSZ.LEFT;
                //break;
            }
            if (updateWindowPos)
            {
                Marshal.StructureToPtr(windowPos, lParam, true);
            }
        }
        private static bool TOPSizeOperation(ref WINDOWPOS windowPos, Point startLocation)
        {
            int yDifference = windowPos.y - startLocation.Y;
            if (yDifference % 12 != 0)
            {
                windowPos.y = startLocation.Y + ((yDifference / 12) * 12);
                if (yDifference > 0)
                {
                    windowPos.cy += 12;
                }
                return true;
            }
            return false;
        }
    }
}
