using Amicitia.IO.Binary;

namespace SonicColorsXTBConv
{
    public static class AmicitiaHelpers
    {
        // TODO: remove this method and use normal ReadString() method from Amicitia.IO
        public static string ReadName(BinaryValueReader reader, int length)
        {
            string result = String.Empty;

            for (int i = 0; i < length; i++)
            {
                char tempChar = Convert.ToChar(reader.ReadByte());
                result += tempChar;
            }

            return result;
        }
    }
}
