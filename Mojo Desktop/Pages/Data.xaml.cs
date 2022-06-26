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
    public sealed partial class Data : Page
    {
        VM vm;

        public Data()
        {
            this.InitializeComponent();
            vm = new VM();
            DataContext = vm;

            LoadData();

        }
        public class AvatarProp
        {
            /// <summary>
            /// 当前生命值
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string ArgName { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string Percent { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string Tip { get; set; }
        }

        private async Task LoadData()
        {
            var t = await GlobalProps.resourceFile.ReadAsync(GlobalProps.Language+ "/AvatarStats.json");

            vm.AllAvatarProps = JsonConvert.DeserializeObject<ObservableCollection<AvatarProp>>(t);
            vm.Items = vm.AllAvatarProps;


        }
        public class VM : ObservableObject
        {
            public VM()
            {

            }
            public ObservableCollection<AvatarProp> AllAvatarProps { get; set; }

            public string ToCommand()
            {
                var cmd = $"/setstats {ID} {Value}";

                GlobalProps.SetCMD(cmd);
                return cmd;
            }

            public string ToSkillCommand()
            {
                var cmd = $"/talent {Skill} {SValue}";

                GlobalProps.SetCMD(cmd);
                return cmd;
            }

            private string skill;

            public string Skill
            {
                get { return skill; }
                set { SetProperty(ref skill, value); ToSkillCommand(); }
            }

            private double sValue = 1;

            public double SValue
            {
                get { return sValue; }
                set { SetProperty(ref sValue, value); ToSkillCommand(); }
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




            private ObservableCollection<AvatarProp> _items;

            public ObservableCollection<AvatarProp> Items
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

        private void skillSel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string ele = (sender as RadioButtons).SelectedItem as string;
            switch (ele)
            {
                case "元素爆发":
                    vm.Skill = "q";

                    break;
                case "元素战技":
                    vm.Skill = "e";

                    break;
                case "风":
                default:
                    vm.Skill = "n";
                    break;


            }
        }
    }
}
