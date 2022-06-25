using Microsoft.Web.WebView2.Core;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace Mojo_Desktop.Utils
{
    class APIHandler
    {
        public static CoreWebView2Environment env;



        public class ReqHandleAttribute : Attribute
        {
            public string ActionTypeName;

            public ReqHandleAttribute(string typeName)
            {
                this.ActionTypeName = typeName;
            }
        }

        public abstract class GenericBLL
        {
            public Hashtable GetMethodAttribute<T>(T t)
            {
                Hashtable ht = new Hashtable();

                Type type = t.GetType();
                foreach (MethodInfo mi in type.GetMethods())
                {
                    ReqHandleAttribute[] mis = (ReqHandleAttribute[])mi.GetCustomAttributes(typeof(ReqHandleAttribute), false);
                    foreach (ReqHandleAttribute actionMethodAttribute in mis)
                    {
                        string actionName = actionMethodAttribute.ActionTypeName;
                        ht.Add(actionName, mi);
                    }
                }

                return ht;
            }

            /// <summary>
            /// return message;
            /// </summary>
            /// <param name="actionName"></param>
            /// <returns></returns>
            public CoreWebView2WebResourceResponse DoAction(string actionName, CoreWebView2WebResourceRequest request)
            {
                string message;
                Hashtable ht = GetMethodAttribute(this);
                if (ht.Contains(actionName))
                {
                    message = ((MethodInfo)ht[actionName]).Invoke(this, new object[] { request }).ToString();
                }
                else
                {
                    message = string.Format("{0} Not Defined.!", actionName);

                }
                var contentType = "application/json";
                var encoding = Encoding.UTF8;
                //MemoryStream ms = new MemoryStream(encoding.GetBytes(message));
                var res= encoding.GetBytes(message). AsBuffer().AsStream().AsRandomAccessStream();
                return env.CreateWebResourceResponse(res, 200, "OK", "Content-Type: " + contentType + ", charset=" + encoding.EncodingName);
                //return env.CreateWebResourceResponse(null, 200, "OK", "Content-Type: " + contentType + ", charset=" + encoding.EncodingName);

            }
        }

        public class Routes : GenericBLL
        {
            static Request server;
            public Routes()
            {
                server = new Request();
            }

            [ReqHandle("mojoplus/api")]
            public static string GetA(CoreWebView2WebResourceRequest request)
            {

                if (string.IsNullOrEmpty(GlobalProps.server) |
                    //string.IsNullOrEmpty(GlobalProps.home) |
                    string.IsNullOrEmpty(GlobalProps.key)
                    )
                {
                    //throw new Exception();;
                    return "";
                }

                StreamReader reader = new StreamReader(request.Content.AsStream());
                string reqData = reader.ReadToEnd();

                JObject o = JObject.Parse(reqData);
                o["k2"] = GlobalProps.key;
                string r = "";
                if (request.Method == "POST")
                {
                    //server.PostJson($"https://127.0.0.1:25565/mojoplus/api", o.ToString());
                    r = Task.Run(() => server.PostJson($"{GlobalProps.server}/mojoplus/api", o.ToString()).Result).Result;

                }

                return r;
            }


        }
    }
}
