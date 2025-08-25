using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace BUAProgramv5.Models.Enum
{
    public static class DescriptionHelper
    {
        /// <summary>
        /// Gets values based from enum data.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this System.Enum value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fieldInfo.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return value.ToString();
            }
        }

        /// <summary>
        /// Gets values based from enum data.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static MultiDescriptionHelper GetMultiDescription(this System.Enum value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());

            MultiDescriptionHelper[] attributes =
                (MultiDescriptionHelper[])fieldInfo.GetCustomAttributes(
                typeof(MultiDescriptionHelper),
                false);

            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0];
            }
            else
            {
                return null;
            }
        }
    }
}
