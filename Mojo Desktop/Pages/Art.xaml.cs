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
            var subAttr = await LoadFromFile("ArtifactSubAttribution.txt");
            vm.SubAttrs = new ObservableCollection<SubAttr>();
            vm.SubAttrName = new ObservableCollection<string>();
            foreach (var item in subAttr)
            {
                if (!vm.SubAttrName.Contains(item.Name.Split('+')[0]))
                {
                    vm.SubAttrName.Add(item.Name.Split('+')[0]);
                }


                vm.SubAttrs.Add(new SubAttr()
                {
                    id=item.Id,
                    name=item.Name.Split('+')[0],
                    value = item.Name

                });
            }


        }

        public class SubAttr
        {
            public string name { get; set; }

            public string value { get; set; }

            public string id { get; set; }
        }

        public class VM : ObservableObject
        {
            public VM()
            {

            }


            private string selectedArt;

            public string SelectedArt
            {
                get { return selectedArt; }
                set { SetProperty(ref selectedArt, value);ToCommand(); }
            }

            private double selectedLevel;

            public double SelectedLevel
            {
                get { return selectedLevel; }
                set { SetProperty(ref selectedLevel, value);ToCommand(); }
            }

            private string _SelectedMainAttr;

            public string SelectedMainAttr
            {
                get { return _SelectedMainAttr; }
                set { SetProperty(ref _SelectedMainAttr, value); ToCommand(); }
            }

            private ObservableCollection<SubAttr> subAttrs;

            public ObservableCollection<SubAttr> SubAttrs
            {
                get { return subAttrs; }
                set { SetProperty(ref subAttrs, value); }
            }

            private ObservableCollection<string> subAttrName;

            public ObservableCollection<string> SubAttrName
            {
                get { return subAttrName; }
                set { SetProperty(ref subAttrName, value); }
            }


            private ObservableCollection<ReliList.Root> artData;

            public ObservableCollection<ReliList.Root> ArtData
            {
                get { return artData; }
                set { SetProperty(ref artData, value); }
            }

            public string ToCommand()
            {
                //var cmd = $"/givechar {ID} {Value}";
                string buildedSubAttr="";

                if (targetSubAttr!=null)
                {

                    foreach (var item in targetSubAttr)
                    {
                        buildedSubAttr += $" {item.id},{item.count}";
                    }
                }

                var cmd = $"/give {SelectedArt} lv{SelectedLevel} {SelectedMainAttr}{buildedSubAttr}";

                GlobalProps.SetCMD(cmd);
                return cmd;
            }

            //private string _id;

            //public string ID
            //{
            //    get { return _id; }
            //    set { SetProperty(ref _id, value); ToCommand(); }
            //}

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

            private ObservableCollection<GlobalProps.SimpleItem> subAttrValues;

            public ObservableCollection<GlobalProps.SimpleItem> SubAttrValues
            {
                get { return subAttrValues; }
                set { SetProperty(ref subAttrValues, value); }
            }


            private string selectedSubAttrName;

            public string SelectedSubAttrName
            {
                get { return selectedSubAttrName; }
                set 
                { 
                    SetProperty(ref selectedSubAttrName, value);

                    SubAttrValues = new ObservableCollection<GlobalProps.SimpleItem>();
                    foreach (var item in SubAttrs)
                    {
                        if (item.name==value)
                        {
                            SubAttrValues
                                .Add(new GlobalProps.SimpleItem() { Id=item.id,Name=item.value });
                        }
                    }




                }
            }


            private double _SubattrCount;

            public double SubattrCount
            {
                get { return _SubattrCount; }
                set { SetProperty(ref _SubattrCount, value); }
            }



            private GlobalProps.SimpleItem selectedSubAttrValue;

            public GlobalProps.SimpleItem SelectedSubAttrValue
            {
                get { return selectedSubAttrValue; }
                set { SetProperty(ref selectedSubAttrValue, value); }
            }

            public class SelectedSubAttr
            {
                public string id { get; set; }
                public double count { get; set; }

                public string name { get; set; }
            }

            private ObservableCollection<SelectedSubAttr> targetSubAttr;

            public ObservableCollection<SelectedSubAttr> TargetSubAttr
            {
                get { return targetSubAttr; }
                set { SetProperty(ref targetSubAttr, value); }
            }





            public Dictionary<string, List<ReliList.Item>> selectedArts;
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

        private void AddSubAttr(object sender, RoutedEventArgs e)
        {

            if (vm.TargetSubAttr==null)
            {
                vm.TargetSubAttr = new ObservableCollection<VM.SelectedSubAttr>();
            }

            if (vm.SelectedSubAttrName!=null&&vm.SelectedSubAttrValue!=null)
            {
                vm.TargetSubAttr.Add(new VM.SelectedSubAttr()
                { count = vm.SubattrCount, id = vm.SelectedSubAttrValue.Id, name = vm.SelectedSubAttrValue.Name }); ;
            }
            vm.ToCommand();
        }

        private void ClearSubAttr(object sender, RoutedEventArgs e)
        {
            vm.TargetSubAttr = new ObservableCollection<VM.SelectedSubAttr>();
            vm.ToCommand();

        }

    }
}
