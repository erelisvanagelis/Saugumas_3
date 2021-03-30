using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Numerics;

namespace Saugumas_3
{
    class RSATool
    {

        public static bool IsItPrimary(int number)
        {
            if (number <= 1)
            {
                return false;
            }
            else
            {
                for (int a = 2; a <= number / 2; a++)
                {
                    if (number % a == 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static string[] ReadFile(string path)
        {
            string allText = File.ReadAllText(path);
            string[] split = new string[3];
            split = allText.Split(';');
            return split;
        }

        public static string EncryptSequence(string Q, string P, string y)
        {
            int[] values = CheckEncryptionVariables(Q, P);
            int q = values[0];
            int p = values[1];

            int n = q * p;
            int t = (q - 1) * (p - 1);

            int e = SetE(t);
            if (e == 0)
                throw new Exception("failed to find e");

            string x = n + ";\n" + e + ";\n";
            foreach(char c in y)
            {
                x += Encrypt(c, n, e);
            }

            return x;
        }

        public static int[] CheckEncryptionVariables(string q, string p)
        {
            int[] values = new int[2];
            values[0] = Convert.ToInt32(q);
            values[1] = Convert.ToInt32(p);

            if (!IsItPrimary(values[0]))
                throw new Exception("q: is not a primary number");

            if (!IsItPrimary(values[1]))
                throw new Exception("p: is not a primary number");

            return values;
        }

        public static int SetE(int t)
        {
            int e = 0;
            for(int i = 2; i < t; i++)
            {
                if(Eucledian(t, i))
                {
                    e = i;
                    break;
                }
                else
                {
                    continue;
                }
            }
            return e;
        }

        public static bool Eucledian(int t, int i)
        {
            int temp = t % i;
            if (temp == 1)
                return true;

            if (temp == 0)
                return false;

            return Eucledian(i, temp);
        }

        public static char Encrypt(char x, int n, int e)
        {
            int charInAscii = (int)x;
            BigInteger powered = BigInteger.Pow(charInAscii, e);
            powered %= n;
            Console.WriteLine($"en: x={x}, charInAscii={charInAscii}, n={n}, e={e}, powered={powered}");
            powered %= 128;
            if (powered < 32)
                powered += 32;
            return (char)powered;
        }
    }
}
