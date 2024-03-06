using Amicitia.IO.Binary;
using System.Text;
using System.Xml.Serialization;

namespace SonicColorsXTBConv.XTB
{
    public class Entry
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("style")]
        public string? Style { get; set; }

        public string? Data { get; set; }

        [XmlAttribute("is_unicode")]
        public bool IsUnicode { get; set; }

        public void Read(BinaryValueReader reader, Version version)
        {
            byte entryNameLength = reader.ReadByte();
            if (entryNameLength != 0)
            {
                Name = AmicitiaHelpers.ReadName(reader, (int)entryNameLength);
            }
            else Name = String.Empty;

            byte entryStyleNameLength = reader.ReadByte();
            Style = AmicitiaHelpers.ReadName(reader, (int)entryStyleNameLength);

            int unicodeCharsCount = reader.ReadInt32();
            int utf8CharsCount = reader.ReadInt32();

            Encoding encoding;
            if (unicodeCharsCount == utf8CharsCount)
            {
                encoding = Encoding.GetEncoding("UTF-8");
            }
            else
            {
                encoding = Encoding.GetEncoding("Unicode");
                IsUnicode = true;
            }

            if (version == Version.blueblur && IsUnicode)
            {
                for (int i3 = 0; i3 < utf8CharsCount; i3++)
                {
                    byte[] character = new byte[2];
                    character[0] = reader.ReadByte();
                    character[1] = reader.ReadByte();
                    Array.Reverse(character);
                    Data += encoding.GetString(character);
                }
            }
            else
            {
                List<byte> tempBuf = new List<byte>();
                for (int i = 0; i < unicodeCharsCount; i++)
                {
                    tempBuf.Add(reader.ReadByte());
                }
                Data = encoding.GetString(tempBuf.ToArray());
            }
        }

        public void Write(BinaryValueWriter writer, Version version)
        {
            writer.Write((byte)Name!.Length);
            writer.WriteString(StringBinaryFormat.FixedLength, Name, Name.Length);

            writer.Write((byte)Style!.Length);
            writer.WriteString(StringBinaryFormat.FixedLength, Style, Style.Length);

            if (IsUnicode)
            {
                writer.Write(Data!.Length * 2);
                writer.Write(Data.Length);
            }
            else
            {
                writer.Write(Data!.Length);
                writer.Write(Data.Length);
            }

            if (IsUnicode)
            {
                byte[] dataBuf = Encoding.Unicode.GetBytes(Data);
                if (version == Version.blueblur)
                {
                    for (int i = 0; i < Data.Length * 2; i++)
                    {
                        byte[] tempBuf = { dataBuf[i], dataBuf[i + 1] };
                        Array.Reverse(tempBuf);
                        dataBuf[i] = tempBuf[0];
                        dataBuf[i + 1] = tempBuf[1];
                        writer.WriteArray(tempBuf);
                        i++;
                    }
                }
                else
                {
                    writer.WriteArray(dataBuf);
                }
            }
            else
            {
                byte[] dataBuf = Encoding.UTF8.GetBytes(Data);
                writer.WriteArray(dataBuf);
            }
        }
    }
}
