using System;
namespace HiT.CommandPromptBox
{
    public partial class CommandPromptBox
    {
        public void Write(bool value)
        {
            output.Write(value);
        }
        public void Write(char value)
        {
            output.Write(value);
        }
        public void Write(char[] buffer)
        {
            output.Write(buffer);
        }
        public void Write(decimal value)
        {
            output.Write(value);
        }
        public void Write(double value)
        {
            output.Write(value);
        }
        public void Write(float value)
        {
            output.Write(value);
        }
        public void Write(int value)
        {
            output.Write(value);
        }
        public void Write(long value)
        {
            output.Write(value);
        }
        public void Write(object value)
        {
            output.Write(value);
        }
        public void Write(string value)
        {
            output.Write(value);
        }
        public void Write(uint value)
        {
            output.Write(value);
        }
        public void Write(ulong value)
        {
            output.Write(value);
        }
        public void Write(string format, object arg0)
        {
            output.Write(format, arg0);
        }
        public void Write(string format, params object[] arg)
        {
            output.Write(format, arg);
        }
        public void Write(char[] buffer, int index, int count)
        {
            output.Write(buffer, index, count);
        }
        public void Write(string format, object arg0, object arg1)
        {
            output.Write(format, arg0, arg1);
        }
        public void Write(string format, object arg0, object arg1, object arg2)
        {
            output.Write(format, arg0, arg1, arg2);
        }
        public void Write(string format, object arg0, object arg1, object arg2, object arg3)
        {
            output.Write(format, arg0, arg1, arg2, arg3);
        }
        public void WriteLine()
        {
            Write(output.NewLine);
        }
        public void WriteLine(bool value)
        {
            Write(value + output.NewLine);
        }
        public void WriteLine(char value)
        {
            Write(value + output.NewLine);
        }
        public void WriteLine(char[] buffer)
        {
            Write(buffer + output.NewLine);
        }
        public void WriteLine(decimal value)
        {
            Write(value + output.NewLine);
        }
        public void WriteLine(double value)
        {
            Write(value + output.NewLine);
        }
        public void WriteLine(float value)
        {
            Write(value + output.NewLine);
        }
        public void WriteLine(int value)
        {
            Write(value + output.NewLine);
        }
        public void WriteLine(long value)
        {
            Write(value + output.NewLine);
        }
        public void WriteLine(object value)
        {
            Write(value + output.NewLine);
        }
        public void WriteLine(string value)
        {
            Write(value + output.NewLine);
        }
        public void WriteLine(uint value)
        {
            Write(value + output.NewLine);
        }
        public void WriteLine(ulong value)
        {
            Write(value + output.NewLine);
        }
        public void WriteLine(string format, object arg0)
        {
            Write(format + output.NewLine, arg0);
        }
        public void WriteLine(string format, params object[] arg)
        {
            Write(format + output.NewLine, arg);
        }
        public void WriteLine(char[] buffer, int index, int count)
        {
            Write(buffer, index, count);
            WriteLine();
        }
        public void WriteLine(string format, object arg0, object arg1)
        {
            Write(format + output.NewLine, arg0, arg1);
        }
        public void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            Write(format + output.NewLine, arg0, arg1, arg2);
        }
        public void WriteLine(string format, object arg0, object arg1, object arg2, object arg3)
        {
            Write(format + output.NewLine, arg0, arg1, arg2, arg3);
        }
    }
}
