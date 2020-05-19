using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TtfLoader;

namespace TtfLoaderSandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            var font = TrueTypeFont.Load("E:\\Fonts\\In Popular Culture\\The Martian\\CarbonRegular.ttf");
        }
    }
}
