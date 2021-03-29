using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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
    }
}
