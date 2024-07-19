using DarkArmor.Models.Skeleton;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;
using System.Xml.Serialization;
using System.Xml;

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
        public static bool CreateSomeStreamBlock()
        {
            bool creatednew = false;
            var dir = Path.Combine(Environment
                                   .GetFolderPath(Environment.SpecialFolder.ApplicationData), "inDarkSneaky");

            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
                File.Create(Path.Combine(dir, "scheme.nic"));
                creatednew = true;
            }
            return creatednew;
        }
        public static NICControllerAsString LoadFromStreamBlock()
        {
            var dir = Path.Combine(Environment
                                 .GetFolderPath(Environment.SpecialFolder.ApplicationData), "inDarkSneaky\\scheme.nic");
          
            XmlReader reader = XmlReader.Create(dir);
            XmlSerializer serializer = new XmlSerializer(typeof(NICControllerAsString));
            NICControllerAsString def_nic = (NICControllerAsString)serializer?.Deserialize(reader);


            return def_nic;
        }
    }
}
