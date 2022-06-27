using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mojo_Desktop.DataTemplates
{
    public class QuestList
    {
        public string GroupName { get; set; }

        public ObservableCollection<GlobalProps.SimpleItem> Items { get; set; }
    }
}
