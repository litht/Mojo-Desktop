using gpm;
using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        public static Cilent cilent;

        public class Cilent
        {
            public bool authStatue = false;

            public string server,uid,otp, k2;
            private Request request;
            public Cilent(string server= "https://yuanshen.com",string uid="10001")
            {
                this.server = server;
                this.uid = uid;
                request = new Request();
            }
            
            public async Task<string> Auth()
            {
                JObject jo = new JObject();
                jo["uid"] = uid;
                if (otp!=null)
                {
                    jo["otp"] = otp;
                }
                var r= await request.PostJson($"{server}/mojoplus/auth", jo.ToString());
                return r;
            }


            public class apiReq
            {
                /// <summary>
                /// 
                /// </summary>
                public string k { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string k2 { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string request { get; set; }
                /// <summary>
                /// 
                /// </summary>
                public string payload { get; set; }
            }

            public async Task<string> Invoke(string payload)
            {
                if (k2!=null&&authStatue)
                {

                    apiReq apiReq = new apiReq
                    {
                        k2=this.k2,
                        k="",
                        request= "invoke",
                        payload=payload
                    };

                    return await request.PostJson($"{server}/mojoplus/api",JsonConvert.SerializeObject(apiReq));

                }
                return "";
            }
        }



    }
}
