using System;

using System.Collections;
using System.IO;
using System.Text;
using System.Runtime;

namespace TinyListener
{
    public class JsonSerializer
    {
        private const byte FnuttByte = (byte)'"';
        private const byte ColonByte = (byte)':';
        private const byte TrueByte = (byte)'1';
        private const byte FalseByte = (byte)'0';
        private const byte CommaByte = (byte)',';
        private const byte ObjectStart = (byte)'{';
        private const byte ObjectEnd = (byte)'}';
        private const byte ArrayStart = (byte)'[';
        private const byte ArrayEnd = (byte)']';

        public static string Serialize(object value)
        {
            using (var ms = new MemoryStream())
            {
				StreamReader reader = new StreamReader(ms);

                WriteValue(ms, value);
                ms.Flush();
                ms.Position = 0;

                return reader.ReadToEnd();
            }
        }

        public static void WriteValue(Stream output, object value)
        {
            if (value is string valueString)
            {
                output.WriteByte(FnuttByte);
                WriteString(output, valueString.Replace("\n", "\\n").Replace("\r", "\\r").Replace("\t", "").Replace("\"", "\\\""));
                output.WriteByte(FnuttByte);
            }
            else if (value is bool b)
            {
                output.WriteByte(b ? TrueByte : FalseByte);
            }
            else if (value is DateTime dt)
            {
                WriteString(output, dt.Ticks.ToString());
            }
            else if (value is int || value is float || value is double)
            {
                WriteString(output, value.ToString());
            }
            else if (value is IEnumerable array)
            {
                var isFirst = true;
                output.WriteByte(ArrayStart);
                foreach (object item in array)
                {
                    if (!isFirst)
                    {
                        output.WriteByte(CommaByte);
                    }
                 
                    WriteKey(output, null, item);
                    isFirst = false;
                }
                output.WriteByte(ArrayEnd);
            }
            else
            {
                var prps = value.GetProperties();
                output.WriteByte(ObjectStart);
                var isFirst = true;
                foreach (var p in prps)
                {

                    if (!isFirst)
                    {
                        output.WriteByte(CommaByte);
                    }
                    WriteKey(output, p.Key, p.Value);
                    isFirst = false;

                }
                output.WriteByte(ObjectEnd);
            }
        }

        private static void WriteKey(Stream output, string str, object value)
        {
            if (value == null)
            {
				return;
            }

            if (str != null)
            {
                output.WriteByte(FnuttByte);
                WriteString(output, str);
                output.WriteByte(FnuttByte);
                output.WriteByte(ColonByte);
            }

            WriteValue(output, value);
        }

        private static void WriteString(Stream s, string v)
        {
            var sendData = Encoding.UTF8.GetBytes(v);
            s.Write(sendData, 0, sendData.Length);
        }
    }
}
