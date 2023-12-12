using Amicitia.IO.Binary;
using System.Xml.Serialization;

namespace SonicColorsXTBConv.XTB
{
    public class Style
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }

        [XmlAttribute("size")]
        public uint Size { get; set; }

        [XmlAttribute("red_color")]
        public byte ColorRed { get; set; }

        [XmlAttribute("green_color")]
        public byte ColorGreen { get; set; }

        [XmlAttribute("blue_color")]
        public byte ColorBlue { get; set; }

        [XmlAttribute("horizontal_alignment")]
        public HorizontalAlignment HorizontalAlignment { get; set; }

        public void Read(BinaryValueReader reader)
        {
            byte styleNameLength = reader.ReadByte();
            Name = AmicitiaHelpers.ReadName(reader, styleNameLength);
            reader.ReadByte(); // Always 1

            Size = reader.ReadUInt32();

            ColorRed = reader.ReadByte();
            ColorGreen = reader.ReadByte();
            ColorBlue = reader.ReadByte();

            HorizontalAlignment = (HorizontalAlignment)reader.ReadInt16();
        }

        public void Write(BinaryValueWriter writer)
        {
            writer.Write((byte)Name!.Length);
            writer.WriteString(StringBinaryFormat.FixedLength, Name, Name.Length);
            writer.Write((byte)1);
            writer.Write(Size);
            writer.Write(ColorRed);
            writer.Write(ColorGreen);
            writer.Write(ColorBlue);
            writer.Write((short)HorizontalAlignment);
        }
    }
}
