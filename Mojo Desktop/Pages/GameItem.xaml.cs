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
    public sealed partial class GameItem : Page
    {
        VM vm;

        public GameItem()
        {
            this.InitializeComponent();
            vm = new VM();
            DataContext = vm;

            LoadData();

        }

        private async Task LoadData(string type= "Item.txt")
        {
            var t = await GlobalProps.resourceFile.ReadAsync(GlobalProps.Language + "/"+type);


            var lines = t.Split('\n');

            vm.GameItems = new ObservableCollection<GlobalProps.SimpleItem>();

            foreach (var line in lines)
            {
                var item = line.Split(':');

                

                vm.GameItems.Add(new GlobalProps.SimpleItem { Name = item[1].TrimEnd(new char[] { '\r', '\n' }), Id = item[0] });
            }
        }

        public class VM : ObservableObject
        {

            private ObservableCollection<GlobalProps.SimpleItem> _monsters;

            public ObservableCollection<GlobalProps.SimpleItem> GameItems
            {
                get { return _monsters; }
                set { SetProperty(ref _monsters, value); }
            }

            public VM()
            {

            }

            public string ToCommand()
            {
                var cmd = "";
                if (IsDrop)
                {
                    cmd = $"/drop {Id} {Num}";

                }
                else
                {
                    cmd = $"/give {Id} {Num} {Level}";

                }


                GlobalProps.SetCMD(cmd);
                return cmd;
            }

            private bool isDrop =false;

            public bool IsDrop
            {
                get { return isDrop; }
                set { SetProperty(ref isDrop, value); ToCommand(); }
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

    }
}
