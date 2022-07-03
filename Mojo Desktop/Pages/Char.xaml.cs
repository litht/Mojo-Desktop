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
    public sealed partial class Char : Page
    {
        VM vm;

        public Char()
        {
            this.InitializeComponent();
            vm = new VM();
            DataContext = vm;

            LoadData();

        }

        private async Task LoadData()
        {
            var t = await GlobalProps.resourceFile.ReadAsync(GlobalProps.Language+"/avatar_list.json");

            vm.AllAvatars = JsonConvert.DeserializeObject<ObservableCollection<AvatarList.Item>>(t);
            vm.Items = vm.AllAvatars;


        }
        public class VM : ObservableObject
        {
            public VM()
            {

            }
            public ObservableCollection<AvatarList.Item> AllAvatars { get; set; }

            public string ToCommand()
            {
                var cmd = $"/give {ID} lv{Value}";

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




            private ObservableCollection<AvatarList.Item> _items;

            public ObservableCollection<AvatarList.Item> Items
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
            var cb = sender as ListBox;
            vm.ID = cb.SelectedValue as string;
        }

        private void DoFilter(string ele)
        {
            var r = vm.AllAvatars
                .Where(item =>
                {
                    return item.element == ele;

                    ;
                })
                .ToList<AvatarList.Item>();
            vm.Items = new ObservableCollection<AvatarList.Item>();
            r.ForEach(p =>
            {
                vm.Items.Add(p);
            });

        }

        private void eleFilter(object sender, SelectionChangedEventArgs e)
        {
            int ele = (sender as RadioButtons).SelectedIndex;
            switch (ele)
            {
                case 1:
                    DoFilter("Electric");
                    break;
                case 2:
                    DoFilter("Ice");
                    break;
                case 3:
                    DoFilter("Wind");
                    break;
                case 4:
                    DoFilter("Water");
                    break;
                case 5:
                    DoFilter("Fire");
                    break;
                case 6:
                    DoFilter("Rock");
                    break;
                case 7:
                    DoFilter("Grass");
                    break;
                default:
                    vm.Items = vm.AllAvatars;
                    break;


            }
        }
    }
}
