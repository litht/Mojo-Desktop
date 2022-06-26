using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mojo_Desktop.DataTemplates
{
    public class ReliList
    {
        public class Item
        {
            /// <summary>
            /// 
            /// </summary>
            public int id { get; set; }
            /// <summary>
            /// (杯)勇士的壮行
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int main { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int append { get; set; }
        }

        public class Root
        {
            public int id { get; set; }
            /// <summary>
            /// 行者之心
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Dictionary<string, List<Item>> contains { get; set; }
        }



    }
}
