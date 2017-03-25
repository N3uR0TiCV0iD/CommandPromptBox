using System;
using System.Drawing;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Runtime.InteropServices;
namespace HiT.CommandPromptBox
{
    internal partial class ConsoleForm : Form
    {
        [DllImport("shell32.dll")] private static extern uint ExtractIconEx(string szFileName, int nIconIndex, IntPtr[] phiconLarge, IntPtr[] phiconSmall, uint nIcons);
        [DllImport("user32.dll")] private static extern int DestroyIcon(IntPtr hIcon);
        WMSZ sizeOperation;
        Point startLocation;
        bool checkPosChanging;
        internal ConsoleForm()
        {
            IntPtr[] exeIcon = new IntPtr[1];
            string exeIconDLLPath;
            int exeIconIndex;
            int windowSize;
            int maxWidth;
            InitializeComponent();
            using (RegistryKey consoleRegistryKey = Registry.CurrentUser.OpenSubKey("Console"))
            {
                const int EXTRA_WIDTH = 16 + 16; //16 = Form Border, 17 = Scrollbar Width
                windowSize = (int)consoleRegistryKey.GetValue("WindowSize");
                this.Size = new Size(EXTRA_WIDTH + ((windowSize & 0xFF) * 8), 38 + ((windowSize >> 16) * 12));
                this.MaximumSize = new Size(EXTRA_WIDTH + (CommandPromptBox.DefaultColumns * 8), 38 + (CommandPromptBox.DefaultRows * 12));
                this.MinimumSize = new Size(136 + 1 - 1, 66 + 1 - 1);
            }
            /*
            if (Environment.OSVersion.Version.Major >= 6) //>= Windows Vista
            {
                exeIconDLLPath = "%SystemRoot%\\system32\\imageres.dll";
                exeIconIndex = 11;
            }
            else
            {
                exeIconDLLPath = "%SystemRoot%\\system32\\shell32.dll";
                exeIconIndex = 2;
            }
            ExtractIconEx(exeIconDLLPath, exeIconIndex, null, exeIcon, 1);
            this.Icon = (Icon)Icon.FromHandle(exeIcon[0]).Clone();
            */
            this.Text = Environment.GetCommandLineArgs()[0];
            //DestroyIcon(exeIcon[0]);
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.Text = this.Width + " x " + this.Height;
        }
        internal CommandPromptBox CommandPrompt
        {
            get
            {
                return commandPromptBox;
            }
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
                        WINDOWPOS.FixWINDOWPOS(m.LParam, sizeOperation, startLocation, commandPromptBox.HScrollBarVisible, commandPromptBox.VScrollBarVisible);
                    }
                break;
            }
            base.WndProc(ref m);
        }
    }
}
