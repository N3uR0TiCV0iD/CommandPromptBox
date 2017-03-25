using System;
using System.Windows.Forms;
namespace ConsoleBoxDemo
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            HiT.CommandPromptBox.Console.Alloc();
            while (true)
            {
                MessageBox.Show(HiT.CommandPromptBox.Console.ReadLine());
            }
        }
    }
}
