using Amicitia.IO.Binary;
using System.Xml.Serialization;

namespace SonicColorsXTBConv.XTB
{
    public class Category
    {
        [XmlAttribute("name")]
        public string? Name { get; set; }
        public List<Entry> Entries = new List<Entry>();

        public void Read(BinaryValueReader reader, Version version)
        {
            byte categoryNameLength = reader.ReadByte();
            Name = AmicitiaHelpers.ReadName(reader, categoryNameLength);

            var entryCount = 0;

            if (version == Version.william) entryCount = reader.ReadInt32();
            else entryCount = reader.ReadByte();

            for (int i = 0; i < entryCount; i++)
            {
                Entry entry = new Entry();

                entry.Read(reader, version);

                Entries.Add(entry);
            }
        }

        public void Write(BinaryValueWriter writer, Version version)
        {
            writer.Write((byte)Name!.Length);
            writer.WriteString(StringBinaryFormat.FixedLength, Name, Name.Length);

            if (version == Version.william)
                writer.Write(Entries.Count);
            else
                writer.Write((byte)Entries.Count);

            foreach (Entry entry in Entries)
            {
                entry.Write(writer, version);
            }
        }
    }
}
