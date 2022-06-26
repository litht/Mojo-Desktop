using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Web.WebView2.Core;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Gaming.XboxGameBar;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;
// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Mojo_Desktop
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Widget1 : Page
    {

        public Widget1()
        {
            RequestedTheme = ElementTheme.Dark;
            this.InitializeComponent();

            if (GlobalProps.resourceFile == null)
            {
                LoadResData();

            }
            GlobalProps.SetCMD = SetCmd;

        }


        private void LoadResData()
        {

            var uri = new Uri("ms-appx:///Assets/Resources.zip");

            Task.Run(async () =>
            {
                var sampleFile = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(uri);

                var stream = await sampleFile.OpenStreamForReadAsync();
                ZipFile file = new ZipFile(stream);
                GlobalProps.resourceFile = new GlobalProps.ResourceFile(file);

                //var r = await GlobalProps.resourceFile.ReadAsync("WeaponColor.txt");

            });
        }

        public void SetCmd(string cmd)
        {
            commandInput.Text = cmd;
        }


        private void nvSample_SelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            var selectedItem = (Microsoft.UI.Xaml.Controls.NavigationViewItem)args.SelectedItem;
            string selectedItemTag = ((string)selectedItem.Tag);
            string pageName = "Mojo_Desktop.Pages." + selectedItemTag;
            Type pageType = Type.GetType(pageName);
            contentFrame.Navigate(pageType);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
