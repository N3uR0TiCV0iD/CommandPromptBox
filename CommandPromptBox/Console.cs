using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
namespace HiT.CommandPromptBox
{
    public static partial class Console
    {
        static ConsoleForm consoleForm;
        static CommandPromptBox console;
        public static void Alloc()
        {
            if (consoleForm == null)
            {
                if (Application.OpenForms.Count == 0)
                {
                    Thread applicationThread = new Thread(delegate()
                    {
                        consoleForm = new ConsoleForm();
                        Application.Run(consoleForm);
                    });
                    applicationThread.Start();
                    while (consoleForm == null)
                    {
                        Thread.Sleep(150);
                    }
                }
                else
                {
                    consoleForm = new ConsoleForm();
                    consoleForm.Show();
                }
                console = consoleForm.CommandPrompt;
            }
        }
        public static ConsoleColor BackgroundColor
        {
            get
            {
                return console.BackColor;
            }
            set
            {
                console.BackColor = value;
            }
        }
        public static void Beep()
        {
            System.Console.Beep();
        }
        public static void Beep(int frequency, int duration)
        {
            System.Console.Beep(frequency, duration);
        }
        public static int BufferHeight
        {
            get
            {
                return console.Rows;
            }
            set
            {
                console.Rows = value;
            }
        }
        public static int BufferWidth
        {
            get
            {
                return console.Columns;
            }
            set
            {
                console.Columns = value;
            }
        }
        public static event ConsoleCancelEventHandler CancelKeyPress;
        public static bool CapsLock
        {
            get
            {
                return false; //?
            }
        }
        public static void Clear()
        {
            console.Clear();
        }
        public static int CursorLeft
        {
            get
            {
                return console.CursorXPos;
            }
            set
            {
                console.CursorXPos = value;
            }
        }
        public static int CursorSize
        {
            get
            {
                return console.CursorHeight;
            }
            set
            {
                console.CursorHeight = value;
            }
        }
        public static int CursorTop
        {
            get
            {
                return console.CursorYPos;
            }
            set
            {
                console.CursorYPos = value;
            }
        }
        public static bool CursorVisible
        {
            get
            {
                return console.CursorVisible;
            }
            set
            {
                console.CursorVisible = value;
            }
        }
        public static TextWriter Error
        {
            get
            {
                return console.Error;
            }
        }
        public static ConsoleColor ForegroundColor
        {
            get
            {
                return console.ForeColor;
            }
            set
            {
                console.ForeColor = value;
            }
        }
        public static TextReader In
        {
            get
            {
                return console.In;
            }
        }
        public static Encoding InputEncoding
        {
            get
            {
                //TODO: FIX;
                throw new NotImplementedException();
            }
            set
            {
                //TODO: FIX;
                throw new NotImplementedException();
            }
        }
        public static bool KeyAvailable
        {
            get
            {
                return console.KeyAvailable;
            }
        }
        public static int LargestWindowHeight
        {
            get
            {
                return System.Console.LargestWindowHeight;
            }
        }
        public static int LargestWindowWidth
        {
            get
            {
                return System.Console.LargestWindowWidth;
            }
        }
        public static void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop)
        {

        }
        public static void MoveBufferArea(int sourceLeft, int sourceTop, int sourceWidth, int sourceHeight, int targetLeft, int targetTop, char sourceChar, ConsoleColor sourceForeColor, ConsoleColor sourceBackColor)
        {

        }
        public static bool NumberLock
        {
            get
            {
                return false; //?
            }
        }
        public static Stream OpenStandardError()
        {
            return System.Console.OpenStandardError();
        }
        public static Stream OpenStandardError(int bufferSize)
        {
            return System.Console.OpenStandardError(bufferSize);
        }
        public static Stream OpenStandardInput()
        {
            return System.Console.OpenStandardInput();
        }
        public static Stream OpenStandardInput(int bufferSize)
        {
            return System.Console.OpenStandardInput(bufferSize);
        }
        public static Stream OpenStandardOutput()
        {
            return System.Console.OpenStandardOutput();
        }
        public static Stream OpenStandardOutput(int bufferSize)
        {
            return System.Console.OpenStandardOutput(bufferSize);
        }
        public static TextWriter Out
        {
            get
            {
                return console.Out;
            }
        }
        public static Encoding OutputEncoding
        {
            get
            {
                //TODO: FIX;
                throw new NotImplementedException();
            }
            set
            {
                //TODO: FIX;
                throw new NotImplementedException();
            }
        }
        public static int Read()
        {
            return console.Read();
        }
        public static ConsoleKeyInfo ReadKey()
        {
            return ReadKey(false);
        }
        public static ConsoleKeyInfo ReadKey(bool intercept)
        {
            return console.ReadKey(intercept);
        }
        public static string ReadLine()
        {
            return console.ReadLine();
        }
        public static void ResetColor()
        {
            console.ResetColor();
        }
        public static void SetBufferSize(int width, int height)
        {
            console.SetBufferSize(width, height);
        }
        public static void SetCursorPosition(int left, int top)
        {
            console.SetCursorPosition(left, top);
        }
        public static void SetError(TextWriter newError)
        {
            console.SetError(newError);
        }
        public static void SetIn(TextReader newIn)
        {
            console.SetIn(newIn);
        }
        public static void SetOut(TextWriter newOut)
        {
            console.SetOut(newOut);
        }
        public static void SetWindowPosition(int left, int top)
        {
            consoleForm.Location = new Point(left, top);
        }
        public static void SetWindowSize(int width, int height)
        {
            consoleForm.Size = new Size(width * 8, height * 12);
        }
        public static string Title
        {
            get
            {
                return consoleForm.Text;
            }
            set
            {
                consoleForm.Text = value;
            }
        }
        public static bool TreatControlCAsInput
        {
            get
            {
                return console.TreatControlCAsInput;
            }
            set
            {
                console.TreatControlCAsInput = value;
            }
        }
        public static int WindowHeight
        {
            get
            {
                return consoleForm.Height / 12;
            }
            set
            {
                consoleForm.Height = value * 12;
            }
        }
        public static int WindowLeft
        {
            get
            {
                return consoleForm.Location.X;
            }
            set
            {
                consoleForm.Location = new Point(value, consoleForm.Location.Y);
            }
        }
        public static int WindowTop
        {
            get
            {
                return consoleForm.Location.Y;
            }
            set
            {
                consoleForm.Location = new Point(consoleForm.Location.X, value);
            }
        }
        public static int WindowWidth
        {
            get
            {
                return consoleForm.Width;
            }
            set
            {
                consoleForm.Width = value * 8;
            }
        }
    }
}
