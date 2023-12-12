using Amicitia.IO.Binary;
using System.Text;
using System.Xml.Serialization;

namespace SonicColorsXTBConv.XTB
{
    [XmlRoot("XTBContainer")]
    public class XTBContainer
    {
        public List<Style> Styles = new List<Style>();
        public List<Category> Categories = new List<Category>();

        [XmlIgnore]
        public Version Version;

        public XTBContainer()
        {

        }

        public void Load(string filepath)
        {
            Endianness endianness = Endianness.Little;
            switch (Version)
            {
                case Version.sonic2010:
                case Version.william:
                    endianness = Endianness.Little;
                    break;
                case Version.blueblur:
                    endianness = Endianness.Big;
                    break;

            }

            using (BinaryValueReader reader = new BinaryValueReader(filepath, endianness, Encoding.UTF8))
            {
                reader.ReadInt64();
                reader.ReadInt16();

                long stylesCount = reader.ReadInt64();

                for (int i = 0; i < stylesCount; i++)
                {
                    Style style = new Style();

                    style.Read(reader);

                    Styles.Add(style);
                }

                long categoriesCount = reader.ReadInt64();

                for (int i = 0; i < categoriesCount; i++)
                {
                    Category category = new Category();

                    category.Read(reader, Version);

                    Categories.Add(category);
                }
            }
        }

        public void Save(string filepath)
        {
            // If a file with the same name exists, delete it to avoid errors
            if (File.Exists(filepath))
                File.Delete(filepath);

            Endianness endianness = Endianness.Little;
            switch (Version)
            {
                case Version.sonic2010:
                case Version.william:
                    endianness = Endianness.Little;
                    break;
                case Version.blueblur:
                    endianness = Endianness.Big;
                    break;
            }

            using (BinaryValueWriter writer = new BinaryValueWriter(filepath, endianness))
            {
                writer.Write(0xFFFFFFFFFFFFFFFF);
                writer.Write((short)2);

                writer.Write((long)Styles.Count);
                foreach (Style style in Styles)
                {
                    style.Write(writer);
                }
                
                writer.Write((long)Categories.Count);
                foreach (Category category in Categories)
                {
                    category.Write(writer, Version);
                }
            }
        }
    }
}
