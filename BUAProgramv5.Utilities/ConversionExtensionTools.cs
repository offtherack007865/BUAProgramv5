using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace BUAProgramv5.Utilities
{
    public static class ConversionExtensionTools
    {
        /// <summary>
        /// Takes the string file and gets the values by each char. then after it is done, disposes of the string so that all references to it in memory are gone.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>SecureString</returns>
        public static SecureString ToSecureString(this string input)
        {
            SecureString secure = new SecureString();
            foreach (char c in input)
            {
                secure.AppendChar(c);
            }
            secure.MakeReadOnly();
            return secure;
        }

        /// <summary>
        /// Copies data from SecureString and returns a new binary string and then releases the binary string from the IinteropServices.Marshal Garbage Collector.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>string </returns>
        public static string ToInsecureString(SecureString input)
        {
            string returnValue = string.Empty;
            IntPtr ptr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(input);
            try
            {
                returnValue = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(ptr);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeBSTR(ptr);
            }
            return returnValue;
        }
    }
}
