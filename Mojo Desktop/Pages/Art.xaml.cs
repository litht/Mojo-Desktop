using Microsoft.Toolkit.Mvvm.ComponentModel;
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
            vm.artTemplate = new VM.ArtTemplate
            {
                cats = await LoadFromFile(),
                items = await LoadFromFile("Artifact.txt"),
                main = await LoadFromFile("ArtifactMainAttribution.txt"),
                sub = await LoadFromFile("ArtifactSubAttribution.txt")
            };

            vm.Cats = vm.artTemplate.cats;
            vm.MainAttr = vm.artTemplate.main;
            vm.SubAttr = vm.artTemplate.sub;

        }

        public class VM : ObservableObject
        {
            public ArtTemplate artTemplate;
            public VM()
            {

            }

            public class ArtTemplate
            {
                public ObservableCollection<GlobalProps.SimpleItem> cats;
                public ObservableCollection<GlobalProps.SimpleItem> items;
                public ObservableCollection<GlobalProps.SimpleItem> main;
                public ObservableCollection<GlobalProps.SimpleItem> sub;
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
                get { return artRating=1; }
                set { SetProperty(ref artRating, value); }
            }


            private ObservableCollection<GlobalProps.SimpleItem> _items;

            public ObservableCollection<GlobalProps.SimpleItem> Cats
            {
                get { return _items; }
                set { SetProperty(ref _items, value); }
            }

            private ObservableCollection<GlobalProps.SimpleItem> items;

            public ObservableCollection<GlobalProps.SimpleItem> Items
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
            var selectedValue = cb.SelectedValue as string;
            var r = vm.artTemplate.items
                .Where(item => 
                {
                    return item.Id.StartsWith(selectedValue) 

                    ; 
                })
                .ToList<GlobalProps.SimpleItem>();
            vm.Items = new ObservableCollection<GlobalProps.SimpleItem>();
            r.ForEach(p =>
            {
                vm.Items.Add(p);
            });

        }
    }
}
