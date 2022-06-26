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
    public sealed partial class Spawn : Page
    {
        VM vm;

        public Spawn()
        {
            this.InitializeComponent();
            vm = new VM();
            DataContext = vm;

            LoadData();

        }

        private async Task LoadData(string type= "Monster.txt")
        {
            var t = await GlobalProps.resourceFile.ReadAsync(GlobalProps.Language + "/"+type);


            var lines = t.Split('\n');

            vm.Monsters = new ObservableCollection<GlobalProps.SimpleItem>();

            foreach (var line in lines)
            {
                var item = line.Split(':');

                

                vm.Monsters.Add(new GlobalProps.SimpleItem { Name = item[1].TrimEnd(new char[] { '\r', '\n' }), Id = item[0] });
            }
        }

        public class VM : ObservableObject
        {

            private ObservableCollection<GlobalProps.SimpleItem> _monsters;

            public ObservableCollection<GlobalProps.SimpleItem> Monsters
            {
                get { return _monsters; }
                set { SetProperty(ref _monsters, value); }
            }

            public VM()
            {

            }

            public string ToCommand()
            {
                var cmd = $"/spawn {Id} {Num} {Level}";

                GlobalProps.SetCMD(cmd);
                return cmd;
            }

            private string id;

            public string Id
            {
                get { return id; }
                set { SetProperty(ref id, value); ToCommand(); }
            }

            private double _num=1;

            public double Num
            {
                get { return _num; }
                set { SetProperty(ref _num, value); ToCommand(); }
            }
            private double _level=1;

            public double Level
            {
                get { return _level; }
                set { SetProperty(ref _level, value); ToCommand(); }
            }

            private int _selectIndex;

            public int selectIndex
            {
                get { return _selectIndex; }
                set { SetProperty(ref _selectIndex, value); ToCommand(); }
            }




        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = sender as ListBox;
            vm.Id = cb.SelectedValue as string;
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            LoadData("Animal.txt");
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            LoadData();

        }

        private void RadioButton_Checked_2(object sender, RoutedEventArgs e)
        {
            LoadData("NPC.txt");

        }

        private void RadioButton_Checked_3(object sender, RoutedEventArgs e)
        {
            LoadData("Ornament.txt");

        }
    }
}
