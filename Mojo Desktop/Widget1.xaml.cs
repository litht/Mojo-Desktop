﻿using System;
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
using Newtonsoft.Json.Linq;
using Windows.UI;
using Windows.System.UserProfile;
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
            var lang= GlobalizationPreferences.Languages.FirstOrDefault();


            if (lang.StartsWith("zh"))
            {

            }
            else if (lang.StartsWith("en"))
            {
                GlobalProps.Language = "en-us";
            }
            else if (lang.StartsWith("ru"))
            {
                GlobalProps.Language = "ru-ru";

            }


            RequestedTheme = ElementTheme.Dark;
            this.InitializeComponent();

            if (GlobalProps.resourceFile == null)
            {
                LoadResData();

            }
            GlobalProps.SetCMD = SetCmd;
            GlobalProps.SetStats = SetState;

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
        public async void Invoke(Action action, Windows.UI.Core.CoreDispatcherPriority Priority = Windows.UI.Core.CoreDispatcherPriority.Normal)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Priority, () => { action(); });
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

        private void ShowMsg(string title,string subtitle)
        {
            this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                commandResultTip.Title = title;
                commandResultTip.Subtitle = subtitle;
                commandResultTip.IsOpen = true;
            });
        }

        public void SetState(bool s=false)
        {
            this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                if (!s)
                {
                    stateLED.Background = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));

                }
                else
                {
                    stateLED.Background = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));

                }
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string cmd = commandInput.Text;
            Task.Run(async () =>
            {
                var resp=await GlobalProps.cilent.Invoke(cmd.Substring(1));

                JObject jo = JObject.Parse(resp);

                if ((int)jo["code"]==200)
                {
                    ShowMsg("执行结果", (string)jo["payload"]);


                }
                else if ((int)jo["code"] == 403)
                {
                    SetState();
                    ShowMsg("登录过期", "请重新登录");

                }


            });
        }
    }
}
