using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml.Controls;
using Mojo_Desktop.DataTemplates;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Mojo_Desktop.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Quest : Page
    {
        VM vm;

        public Quest()
        {
            this.InitializeComponent();
            vm = new VM();
            DataContext = vm;

            LoadData();

        }

        private async Task LoadData()
        {
            var t = await GlobalProps.resourceFile.ReadAsync(GlobalProps.Language+"/quest_list.json");

            var r = JsonConvert.DeserializeObject
                <Dictionary<string, List<List<string>>>>
                (t);
            vm.AllQuests = new ObservableCollection<QuestList>();
            foreach (var item in r)
            {
                var sub = new QuestList();
                sub.GroupName = item.Key;

                sub.Items = new ObservableCollection<GlobalProps.SimpleItem>();
                foreach (var v in item.Value)
                {
                    try
                    {
                        sub.Items.Add(new GlobalProps.SimpleItem() { Id = v[0], Name = v[1] });

                    }
                    catch (Exception e)
                    {
                        continue;
                        //throw;
                    }

                }
                vm.AllQuests.Add(sub);

                
            }

            vm.AllGroups = vm.AllQuests;
            //vm.Items = vm.AllQuests;


        }
        public class VM : ObservableObject
        {
            public VM()
            {

            }
            public ObservableCollection <QuestList> AllQuests { get; set; }

            public string ToCommand()
            {
                string cmd;
                if (IsFinish)
                {
                    cmd = $"/quest finish {ID}";

                }
                else
                {
                    cmd = $"/quest add {ID}";

                }

                GlobalProps.SetCMD(cmd);
                return cmd;
            }

            private string _id;

            public string ID
            {
                get { return _id; }
                set { SetProperty(ref _id, value); ToCommand(); }
            }

            private double _value = 1;

            public double Value
            {
                get { return _value; }
                set { SetProperty(ref _value, value); ToCommand(); }
            }

            private bool isFinish;

            public bool IsFinish
            {
                get { return isFinish; }
                set { SetProperty(ref isFinish, value); ToCommand(); }
            }


            private ObservableCollection<QuestList> allGroups;

            public ObservableCollection<QuestList> AllGroups
            {
                get { return allGroups; }
                set { SetProperty(ref allGroups, value); }
            }


            private ObservableCollection<GlobalProps.SimpleItem> _items;

            public ObservableCollection<GlobalProps.SimpleItem> Items
            {
                get { return _items; }
                set { SetProperty(ref _items, value); }
            }



        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = sender as ComboBox;
            vm.Items = cb.SelectedValue as ObservableCollection<GlobalProps.SimpleItem>;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = sender as ListBox;
            vm.ID = cb.SelectedValue as string;
        }
    }
}
