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
using Mojo_Desktop.Utils;
using static Mojo_Desktop.Utils.APIHandler;
// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Mojo_Desktop
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Widget1 : Page
    {
        ResourceHandler resourceHandler;
        Routes routes = new Routes();
        public Widget1()
        {
            this.InitializeComponent();

            webview.CoreWebView2Initialized += WebView2_CoreWebView2InitializationCompleted;
        }

        private void WebView2_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializedEventArgs e)
        {

            initWebView2();
            //webview.Source = new Uri(internalUrl);
            //webview.DefaultBackgroundColor = Color.Transparent;

        }

        void initWebView2()
        {
            //webview.CoreWebView2.Settings.IsPinchZoomEnabled = false;
            //webview.CoreWebView2.Settings.IsSwipeNavigationEnabled = false;




            webview.CoreWebView2.AddWebResourceRequestedFilter("*", CoreWebView2WebResourceContext.All);
            APIHandler.env = webview.CoreWebView2.Environment;
            resourceHandler = new ResourceHandler(webview.CoreWebView2.Environment);

            webview.CoreWebView2.WebResourceRequested += WebView_WebResourceRequested;



            //webview.CoreWebView2.NavigationStarting += NavigationStarting;



            //internalApi = new InternalApi(resourceHandler);

        }
        private void WebView_WebResourceRequested(object sender, CoreWebView2WebResourceRequestedEventArgs e)
        {
            var host = "https://gc-mojoconsole.github.io/";
            var nhost = "https://127.0.0.1:25565/";
            if (e.Request.Uri.StartsWith(host))
            {
                string path = e.Request.Uri.Substring(host.Length, e.Request.Uri.Length - host.Length);

                if (path.StartsWith("mojoplus/api"))
                {
                    //request.Uri = $"https://{nhost}{path}";


                    e.Response = routes.DoAction(path, e.Request);
                    //return internalApi.ApiReqHandler(request);
                }

            }

            e.GetDeferral().Complete();
        }

        //public CoreWebView2WebResourceResponse handleRequest(CoreWebView2WebResourceRequest request)
        //{
        //    var host = "https://gc-mojoconsole.github.io/";
        //    var nhost = "https://127.0.0.1:25565/";
        //    if (request.Uri.StartsWith(host))
        //    {
        //        string path = request.Uri.Substring(host.Length, request.Uri.Length - host.Length);

        //        if (path.StartsWith("mojoplus/api"))
        //        {
        //            //request.Uri = $"https://{nhost}{path}";


        //            return routes.DoAction(path, request);
        //            //return internalApi.ApiReqHandler(request);
        //        }

        //    }


        //    throw new Exception();
        //}

    }
}
