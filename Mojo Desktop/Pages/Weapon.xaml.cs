using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Windows.Input;
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
    public sealed partial class Weapon : Page
    {



        private VM vm;
        public Weapon()
        {
            vm = new VM();
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = vm;
            LoadData();

        }



        private async Task LoadData()
        {
            var t=await  GlobalProps.resourceFile.ReadAsync(GlobalProps.Language+ "/Weapon.txt");


            var lines = t.Split('\n');

            vm.Items = new ObservableCollection<GlobalProps.SimpleItem>();

            foreach (var line in lines)
            {
                var item = line.Split(':');

                vm.Items.Add(new GlobalProps.SimpleItem { Name = item[1].TrimEnd(new char[] { '\r', '\n' }), Id = item[0] });
            }
        }



        public class VM : ObservableObject
        {
            public VM()
            {

            }


            public string ToCommand()
            {
               


                var cmd = $"/give {ID} x{NUM} lv{Level} r{JL}";

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

            public double NUM
            {
                get { return _value; }
                set { SetProperty(ref _value, value); ToCommand(); }
            }

            private double _level=1;

            public double Level
            {
                get { return _level; }
                set { SetProperty(ref _level, value); ToCommand(); }
            }

            private double _jl=1;

            public double JL
            {
                get { return _jl; }
                set { SetProperty(ref _jl, value); ToCommand(); }
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

        private void cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = sender as ListBox;
            vm.ID = cb.SelectedValue as string;
        }
    }
}
