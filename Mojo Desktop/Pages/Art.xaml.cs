using Microsoft.Toolkit.Mvvm.ComponentModel;
using Mojo_Desktop.DataTemplates;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    public sealed partial class Art : Page
    {
        VM vm;

        public Art()
        {
            this.InitializeComponent();
            vm = new VM();
            DataContext = vm;

            LoadData();

        }

        private async Task<ObservableCollection<GlobalProps.SimpleItem>>
            LoadFromFile(string type = "ArtifactCat.txt")
        {
            var t = await GlobalProps.resourceFile.ReadAsync(GlobalProps.Language + "/" + type);


            var lines = t.Split('\n');

            var Items = new ObservableCollection<GlobalProps.SimpleItem>();
            foreach (var line in lines)
            {
                var item = line.Split(':');



                Items.Add(new GlobalProps.SimpleItem { Name = item[1].TrimEnd(new char[] { '\r', '\n' }), Id = item[0] });
            }

            return Items;
        }


        private async Task LoadData()
        {
            var t = await GlobalProps.resourceFile.ReadAsync("reli_list.json");

            vm.ArtData = JsonConvert.DeserializeObject<ObservableCollection<ReliList.Root>>(t);

            vm.MainAttr = await LoadFromFile("ArtifactMainAttribution.txt");


        }

        public class VM : ObservableObject
        {
            public VM()
            {

            }

            private ObservableCollection<ReliList.Root> artData;

            public ObservableCollection<ReliList.Root> ArtData
            {
                get { return artData; }
                set { SetProperty(ref artData, value); }
            }

            public string ToCommand()
            {
                var cmd = $"/givechar {ID} {Value}";

                GlobalProps.SetCMD(cmd);
                return cmd;
            }

            private string _id;

            public string ID
            {
                get { return _id; }
                set { SetProperty(ref _id, value); ToCommand(); }
            }

            private double _value=1;

            public double Value
            {
                get { return _value; }
                set { SetProperty(ref _value, value); ToCommand(); }
            }


            private double artRating=1;

            public double ArtRating
            {
                get { return artRating; }
                set { SetProperty(ref artRating, value); DoFilter(); }
            }


            internal void DoFilter()
            {
                Items = new ObservableCollection<ReliList.Item>();

                if (selectedArts==null)
                {
                    return;
                }
                if (selectedArts.ContainsKey(ArtRating.ToString()))
                {

                    var r = selectedArts[ArtRating.ToString()];
                    r.ForEach((i) =>
                    {
                        Items.Add(i);

                    });
                }
                else
                {

                }
            }


            private ObservableCollection<ReliList.Item> items;

            public ObservableCollection<ReliList.Item> Items
            {
                get { return items; }
                set { SetProperty(ref items, value); }
            }


            private ObservableCollection<GlobalProps.SimpleItem> _mainAttr;

            public ObservableCollection<GlobalProps.SimpleItem> MainAttr
            {
                get { return _mainAttr; }
                set { SetProperty(ref _mainAttr, value); }
            }
            private ObservableCollection<GlobalProps.SimpleItem> _subAttr;

            public ObservableCollection<GlobalProps.SimpleItem> SubAttr
            {
                get { return _subAttr; }
                set { SetProperty(ref _subAttr, value); }
            }





            public Dictionary<string, List<ReliList.Item>> selectedArts;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = sender as ListBox;
            vm.ID = cb.SelectedValue as string;
        }


        private void CatsSelected(object sender, SelectionChangedEventArgs e)
        {
            var cb = sender as ListBox;
            vm.selectedArts = cb.SelectedValue as
                Dictionary<string, List<ReliList.Item>>;



            vm.DoFilter();



            //var r = vm.artTemplate.items
            //    .Where(item => 
            //    {
            //        return item.Id.StartsWith(selectedValue) 

            //        ; 
            //    })
            //    .ToList<GlobalProps.SimpleItem>();
            //vm.Items = new ObservableCollection<GlobalProps.SimpleItem>();
            //r.ForEach(p =>
            //{
            //    vm.Items.Add(p);
            //});

        }
    }
}
