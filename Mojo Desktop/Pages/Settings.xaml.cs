using Microsoft.Toolkit.Mvvm.ComponentModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
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
    public sealed partial class Settings : Page
    {

        public Settings()
        {
            this.InitializeComponent();
            GlobalProps.ErrMsg = ErrMsg;

        }

        public void ErrMsg(string s)
        {

            //ContentDialog messageDialog = new ContentDialog();
            //messageDialog.Content = new TextBlock() { Text = s };
            //messageDialog.ShowAsync();

            this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                msgbox.Title = "错误";
                msgbox.ActionButtonContent = "确认";

                msgbox.Content = new TextBlock() { Text = s };
                msgbox.IsOpen = true;
            });
        }


        public async void Invoke(Action action, Windows.UI.Core.CoreDispatcherPriority Priority = Windows.UI.Core.CoreDispatcherPriority.Normal)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Priority, () => { action(); });
        }

        private string getErrInfo(int errcode)
        {
            switch (errcode)
            {
                case 200:
                    return "成功";
                case 201:
                    return "未确认";
                case 404:
                    return "玩家不存在/已离线";
                case 400:
                    return "不支持的操作";
                default:
                    return "未知错误";
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {

            GlobalProps.cilent = new GlobalProps.Cilent(serverTB.Text, uidTB.Text);
            Task.Run(async() =>
            {
                var r =await GlobalProps.cilent.Auth();


                try
                {
                    JObject jo = JObject.Parse(r);

                    await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        msgbox.Title= getErrInfo((int)jo["code"]);

                        if ((int)jo["code"]==201)
                        {
                            msgbox.ActionButtonContent = "尝试认证";
                            msgbox.Subtitle = (string)jo["key"] == null ? "" : "验证指令[" + (string)jo["key"] + "]\n已复制到剪贴板";

                            DataPackage dataPackage = new DataPackage();
                            dataPackage.SetText($"/mojo {(string)jo["key"]}");
                            Clipboard.SetContent(dataPackage);

                            GlobalProps.cilent.otp = (string)jo["key"];
                        }
                        else
                        {
                            msgbox.ActionButtonContent = "确认";
                            msgbox.Subtitle = "";

                        }
                        msgbox.IsOpen = true;
                        //ContentDialog dialog = new ContentDialog();
                        //dialog.Title = getErrInfo((int)jo["code"]);
                        //dialog.PrimaryButtonText = "尝试认证";
                        //dialog.CloseButtonText = "Cancel";
                        //dialog.DefaultButton = ContentDialogButton.Primary;
                        //dialog.Content = new TextBlock() { Text = (string)jo["key"]==null?"":(string)jo["key"] };


                        //var result = await dialog.ShowAsync();

                    });


                }
                catch (Exception ex)
                {
                    ErrMsg(ex.Message + ex.StackTrace);
                    throw;
                }


            });
        }

        private void msgbox_ActionButtonClick(Microsoft.UI.Xaml.Controls.TeachingTip sender, object args)
        {
            if( msgbox.ActionButtonContent == "确认")
            {
                msgbox.IsOpen = false;
                return;
            }

            Task.Run(async () =>
            {
                var r = await GlobalProps.cilent.Auth();



                try
                {
                    JObject jo = JObject.Parse(r);


                    await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        msgbox.Title = getErrInfo((int)jo["code"]);

                        if ((int)jo["code"] == 200)
                        {
                            msgbox.Subtitle ="认证成功";
                            msgbox.ActionButtonContent = "确认";
                            GlobalProps.cilent.k2 = (string)jo["key"];
                            GlobalProps.cilent.authStatue = true;

                            GlobalProps.SetStats(true);
                        }
                        else
                           
                        {
                            //msgbox.ActionButtonContent = "确认";
                            msgbox.Subtitle = "认证出错" + jo.ToString(); ;
                            msgbox.ActionButtonContent = "确认";

                        }
                        msgbox.IsOpen = true;
                        //ContentDialog dialog = new ContentDialog();
                        //dialog.Title = getErrInfo((int)jo["code"]);
                        //dialog.PrimaryButtonText = "尝试认证";
                        //dialog.CloseButtonText = "Cancel";
                        //dialog.DefaultButton = ContentDialogButton.Primary;
                        //dialog.Content = new TextBlock() { Text = (string)jo["key"]==null?"":(string)jo["key"] };


                        //var result = await dialog.ShowAsync();

                    });


                }
                catch (Exception ex)
                {
                    ErrMsg(ex.Message + ex.StackTrace);

                    throw;
                }
            });
        }
    }
}
