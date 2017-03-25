using System;
namespace HiT.CommandPromptBox
{
    internal class ChangedCellsInfo
    {
        int end;
        int start;
        public ChangedCellsInfo(int start, int end)
        {
            this.start = start;
            this.end = end;
        }
        public int Start
        {
            get
            {
                return start;
            }
        }
        public int End
        {
            get
            {
                return end;
            }
        }
    }
}
