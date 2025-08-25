using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BUAProgramv5.ServerFunctions.Screenshot
{
    public interface IScreenshotGenerator
    {
        MemoryStream CreateBitmapFromVisual(Visual target);
    }
}
