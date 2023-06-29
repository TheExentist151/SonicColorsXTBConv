namespace SonicColorsXTBConv
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                Console.WriteLine("SonicColorsXTBConv v1.1\nUsage: SonicColorsXTBConv <source file>");
                return;
            }
            else
            {
                string file = Path.GetDirectoryName(args[0]) + "\\" + Path.GetFileName(args[0]);
                if(File.Exists(file))
                {
                    if(file.EndsWith(".xtb"))
                        XTB.XTBtoXML(args[0]);

                    if(file.EndsWith(".xml"))
                        XTB.XMLtoXTB(args[0]);
                }
                else
                {
                    Console.WriteLine($"Can't find file {file}, aborting");
                    return;
                }
            }
        }
    }
}
