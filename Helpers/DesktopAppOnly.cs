using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DarkArmor.Helpers
{
    public static class DesktopAppOnly
    {
        public static class PathFinder
        {
            /// <summary>
            /// get the root relativeely to the current exe.
            /// </summary>
            /// <returns></returns>
            public static string GetApplicationRoot()
            {
                var exePath = new Uri(System.Reflection.
                Assembly.GetExecutingAssembly().CodeBase).LocalPath;

                return new FileInfo(exePath).DirectoryName;

            }

        }
    }
}
