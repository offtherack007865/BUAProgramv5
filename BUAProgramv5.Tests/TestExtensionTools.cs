using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUAProgramv5.Tests
{
    internal static class TestExtensionTools
    {
        /// <summary>
        /// Dynamically convert item into csv file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="path"></param>
        internal static void WriteCSV<T>(this IEnumerable<T> items, string path)
        {
            Type itemType = typeof(T);
            var props = itemType.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).OrderBy(p => p.Name);
            using (StreamWriter streamwriter = new StreamWriter(string.Format(@"{0}\{1}_{2}.csv", path, itemType.Name, DateTime.Now.ToString("yyyyMMddHHmmss"))))
            {
                streamwriter.WriteLine(string.Join(", ", props.Select(p => p.Name)));

                foreach (T item in items)
                {
                    streamwriter.WriteLine(string.Join(", ", props.Select(p => p.GetValue(item, null))));
                }
            }
        }
    }
}
