using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BUAProgramv5.ServerFunctions
{
    internal static class ExtensionTools
    {
        /// <summary>
        /// Gets values based from enum data.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        internal static string GetDescriptionAttribute<T>(this T source)
        {
            FieldInfo fieldInfo = source.GetType().GetField(source.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return source.ToString();
            }
        }
    }
}
