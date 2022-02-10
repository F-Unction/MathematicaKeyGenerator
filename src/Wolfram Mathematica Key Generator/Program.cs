using System;

namespace MathematicaKG
{
    internal class Program
    {
        static void Main()
        {
            GenKey();
            Console.WriteLine("Press any key to exit.");
            Console.Read();
        }

        static void GenKey()
        {
            Console.Write("MathId > ");
            var mathId = Console.ReadLine().Trim();
            if (!CheckMathId(mathId))
            {
                Console.WriteLine("Bad MathId");
                return;
            }
            else
            {
                var activationKey = GenActivationKey();
                int[] magicNumbers;
                Console.Write("Product(mma11, mma12, mma13, sm12, sm13) > ");
                var software = Console.ReadLine();
                if (software == "mma11" || software == "mma12" || software == "mma13")
                {
                    magicNumbers = new int[] { 10690, 12251, 17649, 24816, 33360, 35944, 36412, 42041, 42635, 44011, 53799, 56181, 58536, 59222, 61041 };
                }
                else if (software == "sm12")
                {
                    magicNumbers = new int[] { 4912, 4961, 22384, 24968, 30046, 31889, 42446, 43787, 48967, 61182, 62774 };
                }
                else
                {
                    Console.WriteLine($"Unknown product: {software}.");
                    return;
                }
                Random rnd = new();
                var magicNumber = magicNumbers[(int)Math.Floor(rnd.NextDouble() * magicNumbers.Length)];
                var password = GenPassword(mathId + "$1&" + activationKey, magicNumber);
                Console.WriteLine($"Activation Key:\t {activationKey}");
                Console.WriteLine($"Password:\t {password}");
            }
        }

        static string GenPassword(string str, int hash)
        {
            for (var byteIndex = str.Length - 1; byteIndex >= 0; byteIndex--)
            {
                hash = F(hash, str[byteIndex], 0x105C3);
            }
            var n1 = 0;
            while (F(F(hash, n1 & 0xFF, 0x105C3), n1 >> 8, 0x105C3) != 0xA5B6)
            {
                if (++n1 >= 0xFFFF)
                {
                    return "Error";
                }
            }
            n1 = (int)Math.Floor(((n1 + 0x72FA) & 0xFFFF) * 99999.0 / 0xFFFF);
            var n1str = ("0000" + n1.ToString())[(n1.ToString().Length - 1/* (+ 4 - 5) */)..];
            var temp = int.Parse(n1str[..(n1str.Length - 3)] + n1str[(n1str.Length - 2)..] + n1str[(n1str.Length - 3)..(n1str.Length - 2)]);//
            temp = (int)Math.Ceiling((temp / 99999.0) * 0xFFFF);
            temp = F(F(0, temp & 0xFF, 0x1064B), temp >> 8, 0x1064B);
            for (var byteIndex = str.Length - 1; byteIndex >= 0; byteIndex--)
            {
                temp = F(temp, str[byteIndex], 0x1064B);
            }
            var n2 = 0;
            while (F(F(temp, n2 & 0xFF, 0x1064B), n2 >> 8, 0x1064B) != 0xA5B6)
            {
                if (++n2 >= 0xFFFF)
                {
                    return "Error";
                }
            }
            n2 = (int)Math.Floor((n2 & 0xFFFF) * 99999.0 / 0xFFFF);
            var n2str = ("0000" + n2.ToString())[(n2.ToString().Length - 1 /* (+ 4 - 5) */)..];
            return n2str[3].ToString() + n1str[3].ToString() + n1str[1].ToString() + n1str[0].ToString() + "-"
                + n2str[4].ToString() + n1str[2].ToString() + n2str[0].ToString() + "-"
                + n2str[2].ToString() + n1str[4].ToString() + n2str[1].ToString() + "::1";
        }
        static string GenActivationKey()
        {
            var s = "";
            Random rnd = new();
            for (var i = 0; i < 14; i++)
            {
                s += Math.Floor(rnd.NextDouble() * 10);
                if (i == 3 || i == 7)
                    s += "-";
            }
            return s;
        }
        static bool CheckMathId(string s)
        {
            if (s.Length != 16)
            {
                return false;
            }
            for (var i = 0; i < s.Length; i++)
            {
                if (i == 4 || i == 10)
                {
                    if (s[i] != '-')
                        return false;
                }
                else
                {
                    try
                    {
                        Convert.ToInt32(i);
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        static int F(int n, byte Byte, int c)
        {
            for (var bitIndex = 0; bitIndex <= 7; bitIndex++)
            {
                var bit = (Byte >> bitIndex) & 1;
                if (bit + ((n - bit) & ~1) == n)
                {
                    n = (n - bit) >> 1;
                }
                else
                {
                    n = ((c - bit) ^ n) >> 1;
                }
            }
            return n;
        }
        static int F(int n, char Byte, int c)
        {
            return F(n, (byte)Byte, c);
        }
        static int F(int n, int Byte, int c)
        {
            return F(n, (byte)Byte, c);
        }
    }
}