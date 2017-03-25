using System;
namespace HiT.CommandPromptBox
{
    public struct ConsoleCharCell
    {
        char value;
        ConsoleColor backColor;
        ConsoleColor foreColor;
        public ConsoleCharCell(char value, ConsoleColor backColor, ConsoleColor foreColor)
        {
            this.value = value;
            this.backColor = backColor;
            this.foreColor = foreColor;
        }
        public char Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
            }
        }
        public ConsoleColor BackColor
        {
            get
            {
                return backColor;
            }
            set
            {
                backColor = value;
            }
        }
        public ConsoleColor ForeColor
        {
            get
            {
                return foreColor;
            }
            set
            {
                foreColor = value;
            }
        }
    }
}
