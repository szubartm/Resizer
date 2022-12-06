using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resizer
{
    public class Parser
    {
        public static int ParseToInt(string numberString)
        {
            int result=0;
            try
            {
                result  = Int32.Parse(numberString);
            }
            catch (FormatException)
            {
                Console.WriteLine($"Unable to parse '{numberString}'");
            }

            return result;
        }
    }
}
