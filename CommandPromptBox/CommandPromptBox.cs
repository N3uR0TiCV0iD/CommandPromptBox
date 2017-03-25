using System;
using System.IO;
using System.Text;
using System.Drawing;
using Microsoft.Win32;
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using HiT.CommandPromptBox.Properties;
namespace HiT.CommandPromptBox
{
    public partial class CommandPromptBox : Control
    {
        [DllImport("user32.dll")] private static extern bool CreateCaret(IntPtr hwnd, IntPtr hBmp, int w, int h);
        [DllImport("user32.dll")] private static extern bool SetCaretPos(int x, int y);
        [DllImport("user32.dll")] private static extern bool ShowCaret(IntPtr hwnd);
        [DllImport("user32.dll")] private static extern bool DestroyCaret();
        static ConsoleColor defaultForeColor;
        static ConsoleColor defaultBackColor;
        static Color defaultDrawBackColor;
        static int defaultCursorHeight;
        static int defaultHistorySize;
        static Brush[] consoleBrushes;
        static Color[] consoleColors;
        static bool defaultAllowDups;
        static int defaultColumns;
        static int defaultRows;
        int rows;
        bool hasKey;
        int columns;
        bool readKey;
        int cursorXPos;
        int cursorYPos;
        int inputIndex;
        Keys pressedKey;
        int historySize;
        int inputXStart;
        int inputYStart;
        int cursorHeight;
        TextReader input;
        TextWriter error;
        TextWriter output;
        Keys keyModifiers;
        bool checkNewSize;
        bool updateBuffer;
        bool interceptKey;
        MemoryStream stdin; //Emulated STDIN
        bool cursorVisible;
        WMSZ sizeOperation;
        Bitmap bitmapBuffer;
        Color drawBackColor;
        Point startLocation;
        List<string> history;
        bool controlCIsInput;
        bool allowHistoryDups;
        bool checkPosChanging;
        VScrollBar vScrollBar;
        HScrollBar hScrollBar;
        int historyBrowseIndex;
        ConsoleColor foreColor;
        ConsoleColor backColor;
        ConsoleCharCell[] cells;
        StringBuilder inputBuffer;
        ConsoleKeyInfo readKeyInfo;
        List<ChangedCellsInfo> changedCells;
        Dictionary<char, ColoredCharBitmap> charBitmaps;
        static CommandPromptBox()
        {
            Color currColor;
            int currColorRGB;
            consoleColors = new Color[16];
            consoleBrushes = new Brush[16];
            using (RegistryKey consoleRegistryKey = Registry.CurrentUser.OpenSubKey("Console"))
            {
                int splitValue = (int)consoleRegistryKey.GetValue("ScreenColors");
                for (int currColorIndex = 0; currColorIndex < 10; currColorIndex++)
                {
                    currColorRGB = (int)consoleRegistryKey.GetValue("ColorTable0" + currColorIndex);
                    currColor = Color.FromArgb(255, currColorRGB & 0xFF, (currColorRGB & 0xFF00) >> 8, (currColorRGB & 0xFF0000) >> 16);
                    consoleBrushes[currColorIndex] = new SolidBrush(currColor);
                    consoleColors[currColorIndex] = currColor;
                }
                for (int currColorIndex = 10; currColorIndex < 16; currColorIndex++)
                {
                    currColorRGB = (int)consoleRegistryKey.GetValue("ColorTable" + currColorIndex);
                    currColor = Color.FromArgb(255, currColorRGB & 0xFF, (currColorRGB & 0xFF00) >> 8, (currColorRGB & 0xFF0000) >> 16);
                    consoleBrushes[currColorIndex] = new SolidBrush(currColor);
                    consoleColors[currColorIndex] = currColor;
                }
                defaultForeColor = (ConsoleColor)(splitValue & 0x0F);
                defaultBackColor = (ConsoleColor)((splitValue & 0xF0) >> 4);
                defaultDrawBackColor = GetColorFromConsoleColor(defaultBackColor);
                defaultAllowDups = (int)consoleRegistryKey.GetValue("HistoryNoDup") == 0;
                defaultHistorySize = (int)consoleRegistryKey.GetValue("HistoryBufferSize");
                defaultCursorHeight = (int)( ((int)consoleRegistryKey.GetValue("CursorSize") / 100F) * 12 );
                splitValue = (int)consoleRegistryKey.GetValue("ScreenBufferSize");
                defaultColumns = splitValue & 0xFF;
                defaultRows = splitValue >> 16;
            }
        }
        public static int DefaultRows
        {
            get
            {
                return defaultRows;
            }
        }
        public static int DefaultColumns
        {
            get
            {
                return defaultColumns;
            }
        }
        public new static ConsoleColor DefaultBackColor
        {
            get
            {
                return defaultBackColor;
            }
        }
        public new static ConsoleColor DefaultForeColor
        {
            get
            {
                return defaultForeColor;
            }
        }
        public static int DefaultHistorySize
        {
            get
            {
                return defaultHistorySize;
            }
        }
        public static bool DefaultAllowDups
        {
            get
            {
                return defaultAllowDups;
            }
        }
        public static Color GetColorFromConsoleColor(ConsoleColor consoleColor)
        {
            return consoleColors[(int)consoleColor];
        }
        public static Brush GetBrushFromConsoleColor(ConsoleColor consoleColor)
        {
            return consoleBrushes[(int)consoleColor];
        }
        public CommandPromptBox()
        {
            Rectangle charRectangle;
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            this.charBitmaps = new Dictionary<char, ColoredCharBitmap>();
            this.changedCells = new List<ChangedCellsInfo>();
            using (Bitmap asciiMap = Resources.AsciiMap)
            {
                for (char currCharValue = (char)0x00; currCharValue <= 0xFF; currCharValue++)
                {
                    charRectangle = new Rectangle((currCharValue % 16) * 8, (currCharValue / 16) * 12, 8, 12);
                    this.charBitmaps.Add(currCharValue, new ColoredCharBitmap(asciiMap.Clone(charRectangle, asciiMap.PixelFormat), defaultForeColor));
                }
            }
            this.stdin = new MemoryStream();
            this.SetIn(new StreamReader(stdin));
            this.SetOut( new StreamWriter(Console.OpenStandardOutput()) );
            this.SetError( new StreamWriter(Console.OpenStandardError()) );
            this.inputBuffer = new StringBuilder();
            this.vScrollBar = new VScrollBar();
            this.hScrollBar = new HScrollBar();
            this.Controls.Add(this.vScrollBar);
            this.Controls.Add(this.hScrollBar);
            this.history = new List<string>();
            this.vScrollBar.Dock = DockStyle.Right;
            this.hScrollBar.Dock = DockStyle.Bottom;
            this.cursorHeight = defaultCursorHeight;
            this.vScrollBar.ValueChanged += vScrollBar_ValueChanged;
            this.hScrollBar.ValueChanged += hScrollBar_ValueChanged;
            this.allowHistoryDups = defaultAllowDups;
            this.historySize = defaultHistorySize;
            this.vScrollBar.LargeChange = 1;
            this.hScrollBar.LargeChange = 1;
            this.columns = defaultColumns;
            this.historyBrowseIndex = -1;
            this.updateBuffer = true;
            this.checkNewSize = true;
            this.rows = defaultRows;
            this.UpdateBufferSize();
            this.ResetColor();
        }
        private void hScrollBar_ValueChanged(object sender, EventArgs e)
        {
            this.Refresh();
            UpdateCaretPos();
        }
        private void vScrollBar_ValueChanged(object sender, EventArgs e)
        {
            this.Refresh();
            UpdateCaretPos();
        }
        public new ConsoleColor ForeColor
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
        public new ConsoleColor BackColor
        {
            get
            {
                return backColor;
            }
            set
            {
                drawBackColor = GetColorFromConsoleColor(value);
                backColor = value;
            }
        }
        public int CursorXPos
        {
            get
            {
                return cursorXPos;
            }
            set
            {
                if (value >= 0 && value < columns)
                {
                    cursorXPos = value;
                    UpdateCaretPos();
                }
                else
                {
                    throw new Exception();
                }
            }
        }
        public int CursorYPos
        {
            get
            {
                return cursorYPos;
            }
            set
            {
                if (value >= 0 && value < rows)
                {
                    cursorYPos = value;
                    UpdateCaretPos();
                }
                else
                {
                    throw new Exception();
                }
            }
        }
        public bool CursorVisible
        {
            get
            {
                return cursorVisible;
            }
            set
            {
                cursorVisible = value;
            }
        }
        public int Rows
        {
            get
            {
                return rows;
            }
            set
            {
                if (value >= 1)
                {
                    rows = value;
                    if (updateBuffer)
                    {
                        UpdateBufferSize();
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
        }
        public int Columns
        {
            get
            {
                return columns;
            }
            set
            {
                if (value >= 1)
                {
                    columns = value;
                    if (updateBuffer)
                    {
                        UpdateBufferSize();
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
        }
        public bool AllowHistoryDups
        {
            get
            {
                return allowHistoryDups;
            }
            set
            {
                if (!value)
                {
                    int currCompareIndex;
                    string currHistoryString;
                    for (int currHistoryIndex = 0; currHistoryIndex < history.Count; currHistoryIndex++)
                    {
                        currHistoryString = history[currHistoryIndex];
                        currCompareIndex = currHistoryIndex + 1;
                        while (currCompareIndex < history.Count)
                        {
                            if (history[currCompareIndex] == currHistoryString)
                            {
                                history.RemoveAt(currCompareIndex);
                            }
                            else
                            {
                                currCompareIndex++;
                            }
                        }
                    }
                }
                allowHistoryDups = value;
            }
        }
        public int HistorySize
        {
            get
            {
                return historySize;
            }
            set
            {
                if (historySize > -1)
                {
                    historySize = value;
                }
                else
                {
                    throw new Exception();
                }
            }
        }
        public bool TreatControlCAsInput
        {
            get
            {
                return controlCIsInput;
            }
            set
            {
                controlCIsInput = value;
            }
        }
        public bool KeyAvailable
        {
            get
            {
                return hasKey;
            }
        }
        public int CursorHeight
        {
            get
            {
                return cursorHeight;
            }
            set
            {
                if (value >= 1 && value <= 12)
                {
                    cursorHeight = value;
                    DestroyCaret();
                    CreateCaret(this.Handle, IntPtr.Zero, 8, value);
                    UpdateCaretPos();
                }
                else
                {
                    throw new Exception();
                }
            }
        }
        public TextWriter Error
        {
            get
            {
                return error;
            }
        }
        public TextReader In
        {
            get
            {
                return input;
            }
        }
        public TextWriter Out
        {
            get
            {
                return output;
            }
        }
        internal bool HScrollBarVisible
        {
            get
            {
                return hScrollBar.Visible;
            }
        }
        internal bool VScrollBarVisible
        {
            get
            {
                return vScrollBar.Visible;
            }
        }
        private void UpdateCaretPos()
        {
            SetCaretPos((cursorXPos - hScrollBar.Value) * 8, ((cursorYPos - vScrollBar.Value + 1) * 12) - 3);
        }
        public int Read()
        {
            ReadBlock();
            return input.Read();
        }
        public string ReadLine()
        {
            ReadBlock();
            return input.ReadLine();
        }
        private void ReadBlock()
        {
            while (stdin.Position == stdin.Length)
            {
                Thread.Sleep(150);
            }
        }
        public void Clear()
        {
            cursorXPos = 0;
            cursorYPos = 0;
            ConsoleCharCell defaultCell = new ConsoleCharCell('\0', backColor, foreColor);
            using (Graphics graphics = Graphics.FromImage(bitmapBuffer))
            {
                graphics.Clear(drawBackColor);
            }
            for (int currCellIndex = 0; currCellIndex < cells.Length; currCellIndex++)
            {
                cells[currCellIndex] = defaultCell;
            }
            changedCells.Clear();
            UpdateCaretPos();
            this.Refresh();
        }
        public ConsoleKeyInfo ReadKey()
        {
            return ReadKey(false);
        }
        public ConsoleKeyInfo ReadKey(bool intercept)
        {
            interceptKey = intercept;
            readKey = true;
            while (!hasKey)
            {
                Thread.Sleep(250);
            }
            hasKey = false;
            return readKeyInfo;
        }
        public void ResetColor()
        {
            this.BackColor = defaultBackColor;
            this.ForeColor = defaultForeColor;
        }
        public void SetBufferSize(int width, int height)
        {
            updateBuffer = false;
            this.Columns = width;
            updateBuffer = true;
            this.Rows = height;
        }
        private void UpdateBufferSize()
        {
            ConsoleCharCell defaultCell = new ConsoleCharCell('\0', defaultBackColor, defaultForeColor);
            ConsoleCharCell[] newCells = new ConsoleCharCell[columns * rows];
            Bitmap newBitmapBuffer = new Bitmap(columns * 8, rows * 12);
            int copyingCells;
            if (cells != null)
            {
                copyingCells = Math.Min(cells.Length, newCells.Length);
                for (int currCellIndex = 0; currCellIndex < copyingCells; currCellIndex++)
                {
                    newCells[currCellIndex] = cells[currCellIndex];
                }
            }
            else
            {
                copyingCells = 0;
            }
            cells = newCells;
            for (int currCellIndex = copyingCells; currCellIndex < cells.Length; currCellIndex++)
            {
                cells[currCellIndex] = defaultCell;
            }
            using (Graphics graphics = Graphics.FromImage(newBitmapBuffer))
            {
                graphics.Clear(drawBackColor);
                if (bitmapBuffer != null)
                {
                    graphics.DrawImage(bitmapBuffer, 0, 0);
                }
            }
            bitmapBuffer = newBitmapBuffer;
            ScrollbarsCheck();
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            ScrollbarsCheck();
        }
        private void ScrollbarsCheck()
        {
            int visibleItems; //Colums OR Rows
            int visibleLength = this.Width;
            if (vScrollBar.Visible)
            {
                visibleLength -= 16;
            }
            visibleItems = visibleLength / 8;
            if (visibleItems < columns)
            {
                hScrollBar.Maximum = columns - visibleItems;
                hScrollBar.Visible = true;
            }
            else if (hScrollBar.Visible)
            {

                hScrollBar.Visible = false;
            }
            visibleLength = this.Height;
            if (hScrollBar.Visible)
            {
                visibleLength -= 16;
            }
            visibleItems = visibleLength / 12;
            if (visibleItems < rows)
            {
                vScrollBar.Maximum = rows - visibleItems;
                vScrollBar.Visible = true;
            }
            else if (vScrollBar.Visible)
            {

                vScrollBar.Visible = false;
            }
        }
        public void SetCursorPosition(int left, int top)
        {
            cursorXPos = left;
            cursorYPos = top;
            UpdateCaretPos();
        }
        public void SetError(TextWriter newError)
        {
            EventTextWriter eventTextWriter = new EventTextWriter(newError);
            eventTextWriter.OnDataWrite += TextWriters_OnDataWrite;
            error = eventTextWriter;
        }
        public void SetIn(TextReader newIn)
        {
            input = newIn;
        }
        public void SetOut(TextWriter newOut)
        {
            EventTextWriter eventTextWriter = new EventTextWriter(newOut);
            eventTextWriter.OnDataWrite += TextWriters_OnDataWrite;
            output = eventTextWriter;
        }
        private void TextWriters_OnDataWrite(TextWriter sender, string text)
        {
            int currCellIndex = (cursorYPos * columns) + cursorXPos;
            int startCellIndex = currCellIndex;
            for (int currCharIndex = 0; currCharIndex < text.Length; currCharIndex++)
            {
                if (text[currCharIndex] != 10 && text[currCharIndex] != 13 && text[currCharIndex].ToString() != sender.NewLine)
                {
                    cells[currCellIndex].Value = text[currCharIndex];
                    currCellIndex++;
                }
                else
                {
                    if (startCellIndex != currCellIndex)
                    {
                        changedCells.Add(new ChangedCellsInfo(startCellIndex, currCellIndex - 1));
                    }
                    currCellIndex += columns - (currCellIndex % columns);
                    startCellIndex = currCellIndex;
                }
            }
            if (startCellIndex != currCellIndex)
            {
                changedCells.Add(new ChangedCellsInfo(startCellIndex, currCellIndex - 1));
            }
            cursorXPos = currCellIndex % columns;
            cursorYPos = currCellIndex / columns;
            UpdateCaretPos();
            this.Refresh();
        }
        private void MoveCursorBackward()
        {
            if (cursorXPos != 0)
            {
                cursorXPos--;
            }
            else
            {
                cursorXPos = columns - 1;
                cursorYPos--;
            }
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Back:
                    if (inputIndex != 0)
                    {
                        bool atTheEnd = inputIndex == inputBuffer.Length;
                        inputBuffer.Remove(inputIndex - 1, 1);
                        if (atTheEnd)
                        {
                            MoveCursorBackward();
                            this.Write(" ");
                        }
                        else
                        {
                            ReprintInputBuffer(true);
                        }
                        inputIndex--;
                        MoveCursorBackward();
                        UpdateCaretPos();
                    }
                    e.SuppressKeyPress = true;
                break;
                case Keys.Delete:
                    if (inputIndex != inputBuffer.Length)
                    {
                        inputBuffer.Remove(inputIndex, 1);
                        ReprintInputBuffer(true);
                    }
                break;
                case Keys.Enter:
                    string inputString = inputBuffer.ToString();
                    long lastPosition = stdin.Position;
                    stdin.Position = stdin.Length;
                    stdin.Write(Encoding.ASCII.GetBytes(inputString + "\n"), 0, inputString.Length + 1);
                    stdin.Position = lastPosition;
                    if (inputBuffer.Length != 0 && (history.Count == 0 || inputString != history[history.Count - 1]) && (allowHistoryDups || !history.Contains(inputString)) )
                    {
                        history.Add(inputString);
                    }
                    e.SuppressKeyPress = true;
                    historyBrowseIndex = -1;
                    inputBuffer.Length = 0;
                    inputXStart = 0;
                    inputIndex = 0;
                    cursorXPos = 0;
                    inputYStart++;
                    cursorYPos++;
                    UpdateCaretPos();
                break;
                case Keys.Left:
                    if (inputIndex != 0)
                    {
                        if (cursorXPos != 0)
                        {
                            cursorXPos--;
                            UpdateCaretPos();
                        }
                        else if (cursorYPos != 0)
                        {
                            cursorYPos--;
                            cursorXPos = columns - 1;
                            UpdateCaretPos();
                        }
                        inputIndex--;
                    }
                break;
                case Keys.Up:
                    if (historyBrowseIndex != (history.Count - 1))
                    {
                        historyBrowseIndex++;
                        SetInputToHistory();
                    }
                break;
                case Keys.Right:
                    if (inputIndex != inputBuffer.Length)
                    {
                        if (cursorXPos != (columns - 1))
                        {
                            cursorXPos++;
                            UpdateCaretPos();
                        }
                        else if (cursorYPos != (rows - 1))
                        {
                            cursorYPos++;
                            cursorXPos = 0;
                            UpdateCaretPos();
                        }
                        inputIndex++;
                    }
                break;
                case Keys.Down:
                    if (historyBrowseIndex > 0)
                    {
                        historyBrowseIndex--;
                        SetInputToHistory();
                    }
                break;
            }
            keyModifiers = e.Modifiers;
            pressedKey = e.KeyCode;
        }
        private void SetInputToHistory()
        {
            string historyString = history[history.Count - historyBrowseIndex - 1];
            inputBuffer.Length = 0;
            inputBuffer.Append(historyString);
            inputIndex = historyString.Length;
            ReprintInputBuffer(true);
        }
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (inputBuffer.Length == 0)
            {
                inputXStart = cursorXPos;
                inputYStart = cursorYPos;
            }
            inputBuffer.Insert(inputIndex, e.KeyChar);
            inputIndex++;
            if (inputIndex == inputBuffer.Length)
            {
                this.Write(e.KeyChar);
            }
            else
            {
                ReprintInputBuffer(false);
            }
            readKeyInfo = new ConsoleKeyInfo(e.KeyChar, (ConsoleKey)pressedKey, (keyModifiers & Keys.Shift) == 0, (keyModifiers & Keys.Alt) == 0, (keyModifiers & Keys.Control) == 0);
            if (readKey)
            {
                e.Handled = interceptKey;
                readKey = false;
            }
            hasKey = true;
        }
        private void ReprintInputBuffer(bool addSpace)
        {
            int inputCellIndex = (inputYStart * columns) + inputXStart + inputIndex;
            cursorXPos = inputXStart;
            cursorYPos = inputYStart;
            if (!addSpace)
            {
                this.Write(inputBuffer.ToString());
            }
            else
            {
                this.Write(inputBuffer.ToString() + " ");
            }
            cursorXPos = inputCellIndex % columns;
            cursorYPos = inputCellIndex / columns;
            UpdateCaretPos();
       }
        protected override bool IsInputKey(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Left:
                case Keys.Up:
                case Keys.Right:
                case Keys.Down: return true;
            }
            return base.IsInputKey(keyData);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            if (changedCells.Count != 0)
            {
                int currX;
                int currY;
                ConsoleCharCell currCell;
                ColoredCharBitmap currCharBitmap;
                int xLimit = (columns - 1) * 8;
                using (Graphics graphics = Graphics.FromImage(bitmapBuffer))
                {
                    foreach (var currChangedCells in changedCells)
                    {
                        currX = (currChangedCells.Start % columns) * 8;
                        currY = (currChangedCells.Start / columns) * 12;
                        for (int currCellIndex = currChangedCells.Start; currCellIndex <= currChangedCells.End; currCellIndex++)
                        {
                            currCell = cells[currCellIndex];
                            graphics.FillRectangle(GetBrushFromConsoleColor(currCell.BackColor), currX, currY, 8, 12); //TODO: Batch this! (Since the changed cells are like a "print")
                            currCharBitmap = charBitmaps[currCell.Value];
                            currCharBitmap.Color = currCell.ForeColor;
                            graphics.DrawImage(currCharBitmap.CharBitmap, currX, currY);
                            if (currX < xLimit)
                            {
                                currX += 8;
                            }
                            else
                            {
                                currY += 12;
                                currX = 0;
                            }
                        }
                    }
                }
                changedCells.Clear();
            }
            e.Graphics.DrawImage(bitmapBuffer, hScrollBar.Value * -8, vScrollBar.Value * -12);
        }
        public Bitmap TMP(char d)
        {
            return charBitmaps[d].CharBitmap;
        }
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WINDOWPOS.WM_ENTERSIZEMOVE:
                    startLocation = this.Location;
                break;
                case WINDOWPOS.WM_MOVING:
                    checkPosChanging = false;
                break;
                case WINDOWPOS.WM_SIZING:
                    sizeOperation = (WMSZ)m.WParam.ToInt32();
                    checkPosChanging = true;
                break;
                case WINDOWPOS.WM_WINDOWPOSCHANGING:
                    if (checkPosChanging)
                    {
                        WINDOWPOS.FixWINDOWPOS(m.LParam, sizeOperation, startLocation, hScrollBar.Visible, vScrollBar.Visible);
                    }
                break;
            }
            base.WndProc(ref m);
        }
        protected override void OnGotFocus(EventArgs e)
        {
            CreateCaret(this.Handle, IntPtr.Zero, 8, cursorHeight);
            UpdateCaretPos();
            ShowCaret(this.Handle);
            base.OnGotFocus(e);
        }
        protected override void OnLostFocus(EventArgs e)
        {
            DestroyCaret();
            base.OnLostFocus(e);
        }
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            for (char currCharValue = (char)0x00; currCharValue <= 0xFF; currCharValue++)
            {
                charBitmaps[currCharValue].Dispose();
            }
            charBitmaps.Clear();
        }
        [Bindable(false)]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Text { get; set; }
    }
}
