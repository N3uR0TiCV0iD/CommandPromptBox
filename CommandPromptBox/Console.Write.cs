using System;
namespace HiT.CommandPromptBox
{
    public static partial class Console 
    {
        public static void Write(bool value)
        {
            console.Write(value);
        }
        public static void Write(char value)
        {
            console.Write(value);
        }
        public static void Write(char[] buffer)
        {
            console.Write(buffer);
        }
        public static void Write(decimal value)
        {
            console.Write(value);
        }
        public static void Write(double value)
        {
            console.Write(value);
        }
        public static void Write(float value)
        {
            console.Write(value);
        }
        public static void Write(int value)
        {
            console.Write(value);
        }
        public static void Write(long value)
        {
            console.Write(value);
        }
        public static void Write(object value)
        {
            console.Write(value);
        }
        public static void Write(string value)
        {
            console.Write(value);
        }
        public static void Write(uint value)
        {
            console.Write(value);
        }
        public static void Write(ulong value)
        {
            console.Write(value);
        }
        public static void Write(string format, object arg0)
        {
            console.Write(format, arg0);
        }
        public static void Write(string format, params object[] arg)
        {
            console.Write(format, arg);
        }
        public static void Write(char[] buffer, int index, int count)
        {
            console.Write(buffer, index, count);
        }
        public static void Write(string format, object arg0, object arg1)
        {
            console.Write(format, arg0, arg1);
        }
        public static void Write(string format, object arg0, object arg1, object arg2)
        {
            console.Write(format, arg0, arg1, arg2);
        }
        public static void Write(string format, object arg0, object arg1, object arg2, object arg3)
        {
            console.Write(format, arg0, arg1, arg2, arg3);
        }
        public static void WriteLine()
        {
            Write("\n");
        }
        public static void WriteLine(bool value)
        {
            Write(value + "\n");
        }
        public static void WriteLine(char value)
        {
            Write(value + "\n");
        }
        public static void WriteLine(char[] buffer)
        {
            Write(buffer + "\n");
        }
        public static void WriteLine(decimal value)
        {
            Write(value + "\n");
        }
        public static void WriteLine(double value)
        {
            Write(value + "\n");
        }
        public static void WriteLine(float value)
        {
            Write(value + "\n");
        }
        public static void WriteLine(int value)
        {
            Write(value + "\n");
        }
        public static void WriteLine(long value)
        {
            Write(value + "\n");
        }
        public static void WriteLine(object value)
        {
            Write(value + "\n");
        }
        public static void WriteLine(string value)
        {
            Write(value + "\n");
        }
        public static void WriteLine(uint value)
        {
            Write(value + "\n");
        }
        public static void WriteLine(ulong value)
        {
            Write(value + "\n");
        }
        public static void WriteLine(string format, object arg0)
        {
            Write(format + "\n", arg0);
        }
        public static void WriteLine(string format, params object[] arg)
        {
            Write(format + "\n", arg);
        }
        public static void WriteLine(char[] buffer, int index, int count)
        {
            Write(buffer, index, count);
            WriteLine();
        }
        public static void WriteLine(string format, object arg0, object arg1)
        {
            Write(format + "\n", arg0, arg1);
        }
        public static void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            Write(format + "\n", arg0, arg1, arg2);
        }
        public static void WriteLine(string format, object arg0, object arg1, object arg2, object arg3)
        {
            Write(format + "\n", arg0, arg1, arg2, arg3);
        }
    }
}
