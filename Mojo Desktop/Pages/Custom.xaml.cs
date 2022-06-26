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
    public sealed partial class Custom : Page
    {



        private VM vm;
        public Custom()
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
            var t=await  GlobalProps.resourceFile.ReadAsync(GlobalProps.Language+"/CustomCommands.txt");


            var lines = t.Split('\n');

            vm.Commands = new ObservableCollection<item>();
            for (int i = 0; i < lines.Length; i++)
            {
                vm.Commands.Add(new item { Desp = lines[i], cmd = lines[++i] });
            }
        }


        public class item
        {
            public string Desp { get; set; }
            public string cmd { get; set; }
        }


        public class VM :ObservableObject
        {
            public ICommand RunCMD { get; set; }
            public VM()
            {
                RunCMD = new RelayCommand<object>((cmd) => 
                {
                    GlobalProps.SetCMD(cmd as string);
                });
            }

            private ObservableCollection<item> _commands;

            public ObservableCollection<item> Commands
            {
                get { return _commands; }
                set { SetProperty(ref _commands, value); }
            }
        }


    }
}
