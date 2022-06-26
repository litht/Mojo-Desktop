using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mojo_Desktop
{
    public static class GlobalProps
    {
        public static ResourceFile resourceFile;

        public class ResourceFile
        {
            ZipFile file;
            public ResourceFile(ZipFile f)
            {
                this.file = f;
            }

            public async Task<string> ReadAsync(string path)
            {
                try
                {
                    ZipEntry zipEntry = file.GetEntry(path);
                    Stream stream = file.GetInputStream(zipEntry);

                    using (StreamReader sr = new StreamReader(stream))
                    {
                        string r = await sr.ReadToEndAsync();

                        return r;
                    }
                }
                catch
                {

                }
                return "";
            }



        }

        public delegate void setCMD(string cmd);

        public static setCMD SetCMD;

        public const string Language = "zh-cn";

        public class SimpleItem
        {
            public string Name { get; set; }
            public string Id { get; set; }
        }
    }
}
