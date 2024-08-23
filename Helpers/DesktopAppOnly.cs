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
        public static async Task<bool> PutToStreamBlock(NICController niccString)
        {
            bool done = false;

            await Task.Run(
                () =>
                {
                    XmlWriterSettings xmlWriterSettings = new XmlWriterSettings()
                    {
                        Indent = true,
                        IndentChars = "\t",
                        NewLineOnAttributes = true
                    };


                    var dir = Path.Combine(Environment
                               .GetFolderPath(Environment.SpecialFolder.ApplicationData), "inDarkSneaky\\scheme.nic");

                    XmlWriter writer = XmlWriter.Create(dir, xmlWriterSettings);
                    writer.WriteStartElement("NICControllerAsString");
                    
                    writer.WriteElementString("Nic_Index", $"{niccString.Nic_Index}");
                    writer.WriteElementString("FriendlyName", $"{niccString.FriendlyName}");
                    writer.WriteElementString("Address", $"{niccString.Address}");
                    writer.WriteElementString("Gate", $"{niccString.Gate}");
                    writer.WriteElementString("Mask", $"{niccString.Mask}");
                    writer.WriteElementString("PhysicalAdress", $"{niccString.PhysicalAdress}");
                    writer.WriteElementString("Manufacture", "no_data");
                    writer.WriteElementString("Broadcast", $"{niccString.Broadcast}");
                    writer.WriteElementString("Active", $"{niccString.Active}");




                    writer.WriteEndElement();
                    writer.Flush();

                    done = true;
                    return done;
                }
            );
            return done;
        }
    }
}
