using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mojo_Desktop
{
    class GlobalProps
    {
        public static string server= "https://127.0.0.1:25565";
        public static string key= "10001:1656154379:f1b4ff1c7df56e677bd811324e2c1e04bcf80e28b1da164957a339d9195f01d4";
        public static string home="";

        public delegate void navigateTo(string url);

        public static navigateTo NavigateTo;

        public static string MojoServer;

    }
}
