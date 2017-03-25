using System;
using System.IO;
using System.Text;
using System.Runtime.Remoting;
namespace HiT.CommandPromptBox
{
    public delegate void EventTextWriterDataHandler(TextWriter sender, string text);
    public class EventTextWriter : TextWriter
    {
        TextWriter textWriter;
        public EventTextWriter(TextWriter textWriter)
        {
            this.textWriter = textWriter;
        }
        public override void Close()
        {
            textWriter.Close();
        }
        public override ObjRef CreateObjRef(Type requestedType)
        {
            return textWriter.CreateObjRef(requestedType);
        }
        public override bool Equals(object obj)
        {
            return textWriter.Equals(obj);
        }
        public override Encoding Encoding
        {
            get
            {
                return textWriter.Encoding;
            }
        }
        public override void Flush()
        {
            textWriter.Flush();
        }
        public override IFormatProvider FormatProvider
        {
            get
            {
                return textWriter.FormatProvider;
            }
        }
        public override int GetHashCode()
        {
            return textWriter.GetHashCode();
        }
        public override object InitializeLifetimeService()
        {
            return textWriter.InitializeLifetimeService();
        }
        public override string NewLine
        {
            get
            {
                return textWriter.NewLine;
            }
            set
            {
                textWriter.NewLine = value;
            }
        }
        public override string ToString()
        {
            return textWriter.ToString();
        }
        public override void Write(bool value)
        {
            textWriter.Write(value);
            OnDataWrite?.Invoke(this, value.ToString());
        }
        public override void Write(char value)
        {
            textWriter.Write(value);
            OnDataWrite?.Invoke(this, value.ToString());
        }
        public override void Write(char[] buffer)
        {
            textWriter.Write(buffer);
            OnDataWrite?.Invoke(this, new String(buffer));
        }
        public override void Write(char[] buffer, int index, int count)
        {
            textWriter.Write(buffer, index, count);
            OnDataWrite?.Invoke(this, new String(buffer, index, count));
        }
        public override void Write(decimal value)
        {
            textWriter.Write(value);
            OnDataWrite?.Invoke(this, value.ToString());
        }
        public override void Write(double value)
        {
            textWriter.Write(value);
            OnDataWrite?.Invoke(this, value.ToString());
        }
        public override void Write(float value)
        {
            textWriter.Write(value);
            OnDataWrite?.Invoke(this, value.ToString());
        }
        public override void Write(int value)
        {
            textWriter.Write(value);
            OnDataWrite?.Invoke(this, value.ToString());
        }
        public override void Write(long value)
        {
            textWriter.Write(value);
            OnDataWrite?.Invoke(this, value.ToString());
        }
        public override void Write(object value)
        {
            textWriter.Write(value);
            OnDataWrite?.Invoke(this, value.ToString());
        }
        public override void Write(string format, object arg0)
        {
            textWriter.Write(format, arg0);
            OnDataWrite?.Invoke(this, String.Format(format, arg0));
        }
        public override void Write(string format, object arg0, object arg1)
        {
            textWriter.Write(format, arg0, arg1);
            OnDataWrite?.Invoke(this, String.Format(format, arg0, arg1));
        }
        public override void Write(string format, object arg0, object arg1, object arg2)
        {
            textWriter.Write(format, arg0, arg1, arg2);
            OnDataWrite?.Invoke(this, String.Format(format, arg0, arg1, arg2));
        }
        public override void Write(string format, params object[] arg)
        {
            textWriter.Write(format, arg);
            OnDataWrite?.Invoke(this, String.Format(format, arg));
        }
        public override void Write(string value)
        {
            textWriter.Write(value);
            OnDataWrite?.Invoke(this, value);
        }
        public override void Write(uint value)
        {
            textWriter.Write(value);
            OnDataWrite?.Invoke(this, value.ToString());
        }
        public override void Write(ulong value)
        {
            textWriter.Write(value);
            OnDataWrite?.Invoke(this, value.ToString());
        }        
        public event EventTextWriterDataHandler OnDataWrite;
    }
}
