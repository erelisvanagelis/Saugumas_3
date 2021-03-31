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

        public static bool IsItPrimary(BigInteger number)
        {
            if (number <= 1)
            {
                return false;
            }
            else
            {
                for (BigInteger i = 2; i <= number / 2; i++)
                {
                    if (number % i == 0)
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
            BigInteger[] values = CheckEncryptionVariables(Q, P);
            BigInteger q = values[0];
            BigInteger p = values[1];

            BigInteger n = q * p;
            BigInteger t = (q - 1) * (p - 1);

            BigInteger e = SetE(t);
            if (e == 0)
                throw new Exception("failed to find e");


            string x = n + ";\n" + e + ";\n";
            foreach(char c in y)
            {
                int temp = Encrypt(c, n, e);
                x += Encrypt(c, n, e);
                x += ';';
            }
            x = x.Remove(x.Length - 1, 1);

            return x;
        }

        public static BigInteger[] CheckEncryptionVariables(string q, string p)
        {
            BigInteger[] values = new BigInteger[2];
            values[0] = BigInteger.Parse(q);
            values[1] = BigInteger.Parse(p);

            if (!IsItPrimary(values[0]))
                throw new Exception("q: is not a primary number");

            if (!IsItPrimary(values[1]))
                throw new Exception("p: is not a primary number");

            return values;
        }

        public static BigInteger SetE(BigInteger t)
        {
            BigInteger e = 0;
            for(BigInteger i = 2; i < t; i++)
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

        public static bool Eucledian(BigInteger t, BigInteger i)
        {
            BigInteger temp = t % i;
            if (temp == 1)
                return true;

            if (temp == 0)
                return false;

            return Eucledian(i, temp);
        }

        public static int Encrypt(char x, BigInteger n, BigInteger e)
        {
            int charInAscii = (int)x;
            BigInteger powered = BigInteger.Pow(charInAscii, (int)e);
            powered %= n;
            Console.WriteLine($"en: x={x}, charInAscii={charInAscii}, n={n}, e={e}, powered={powered}");
            return (int)powered;
        }

        public static BigInteger[] GetPrimaries(BigInteger n)
        {
            BigInteger[] bigs = new BigInteger[2];
            for(BigInteger i = 2; i < n / 2; i++)
            {
                if(IsItPrimary(i))
                {
                    BigInteger another = n / i;

                    if(IsItPrimary(another) && i * another == n)
                    {
                        bigs[0] = i;
                        bigs[1] = another;
                        Console.WriteLine(i + " " + another);
                        break;
                    }
                }
            }

            return bigs;
        }

        public static string DecryptSequence(string N, string y)
        {
            BigInteger n = BigInteger.Parse(N);
            BigInteger[] primaries = GetPrimaries(n);
            BigInteger t = (primaries[0] - 1) * (primaries[1] - 1);
            BigInteger e = SetE(t);
            Console.WriteLine("e=" + e.ToString());
            BigInteger d;

            BigInteger i = 2;
            while (true)
            {
                BigInteger temp = i * e;
                if (temp % t == 1)
                {
                    d = i;
                    break;
                }
                i++;
            }

            string x = "";
            string[] ySplit = y.Split(';');
            foreach(string s in ySplit)
            {
                x += Decrypt(BigInteger.Parse(s), n, d);
                Console.WriteLine(x);
            }
            return x;
        }

        public static char Decrypt(BigInteger x, BigInteger n, BigInteger d)
        {
            BigInteger powered = BigInteger.Pow(x, (int)d);
            powered %= n;
            Console.WriteLine($"de: x={x}, n={(int)n}, d={(int)d}, powered={(int)powered}");
            return (char)powered;
        }

        public static void WriteToAFile(string path, string contents)
        {
            File.WriteAllText(path, contents);
        }
    }
}
