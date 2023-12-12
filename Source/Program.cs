using System.Xml;
using System.Xml.Serialization;

namespace SonicColorsXTBConv
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filepathWithoutExtension = string.Empty;
            string formatMessage = "\n\tsonic2010/swa - Sonic Unleashed/Colors format\n" +
                "\twilliam - Mario & Sonic at the London 2012 Olympic Games format\n\tblueblur - Sonic Generations format";
            XTB.Version Version;
            XmlSerializer serializer = new XmlSerializer(typeof(XTB.XTBContainer));

            if (args.Length == 0)
            {
                Console.WriteLine("SonicColorsXTBConv 2.0\nUsage: SonicColorsXTBConv.exe <input file> <format version> (or you can just drag and drop a file)" +
                    "\n\nFormat versions:" + "\n" + formatMessage);
                return;
            }

            if (!File.Exists(args[0]))
            {
                Console.WriteLine($"Can't find file \"{args[0]}\", aborting");
                return; 
            }

            if (args.Length == 2)
            {
                filepathWithoutExtension = Path.GetDirectoryName(args[0]) + "\\" + Path.GetFileNameWithoutExtension(args[0]);
                switch (args[1])
                {
                    case "sonic2010":
                    case "swa":
                        Version = XTB.Version.sonic2010; 
                        break;
                    case "william":
                        Version = XTB.Version.william;
                        break;
                    case "blueblur":
                        Version = XTB.Version.blueblur;
                        break;
                    default:
                        Console.WriteLine($"Version {args[1]} does not exist, aborting");
                        return;
                }
            }
            else
            {
                filepathWithoutExtension = Path.GetDirectoryName(args[0]) + "\\" + Path.GetFileNameWithoutExtension(args[0]);
                Console.WriteLine("Please specify the format version: " + "\n" + formatMessage + "\n");
                Console.Write("Format version: ");
                string version = Console.ReadLine()!;
                switch (version)
                {
                    case "sonic2010":
                    case "swa":
                        Version = XTB.Version.sonic2010; 
                        break;
                    case "william":
                        Version = XTB.Version.william;
                        break;
                    case "blueblur":
                        Version = XTB.Version.blueblur;
                        break;
                    default:
                        Console.WriteLine($"Version {version} does not exist, aborting");
                        return;
                }
            }

            if (args[0].EndsWith(".xtb"))
            {
                XTB.XTBContainer xtb = new XTB.XTBContainer();
                xtb.Version = Version;
                xtb.Load(filepathWithoutExtension + ".xtb");

                // If a file with the same name exists, delete it to avoid errors
                if (File.Exists(filepathWithoutExtension + ".xml"))
                    File.Delete(filepathWithoutExtension + ".xml");

                using (FileStream fs = new FileStream(filepathWithoutExtension + ".xml", FileMode.Create, FileAccess.Write))
                {
                    serializer.Serialize(fs, xtb);
                }
            }
            else
            {
                XTB.XTBContainer xtb = new XTB.XTBContainer();

                using (FileStream fs = new FileStream(filepathWithoutExtension + ".xml", FileMode.Open, FileAccess.Read))
                {
                    XmlReader reader = XmlReader.Create(fs);
                    xtb = (XTB.XTBContainer)serializer.Deserialize(reader)!;
                }

                xtb.Version = Version;

                xtb.Save(filepathWithoutExtension + ".xtb");
            }
        }
    }
}
