using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

//https://go.microsoft.com/fwlink/?LinkId=234236 上介绍了“用户控件”项模板

namespace Mojo_Desktop.Controls
{
    public sealed partial class ListCard : UserControl
    {
        public ListCard()
        {
            this.InitializeComponent();
        }



        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Description.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DescriptionProperty =
            DependencyProperty.Register("Description", typeof(string), typeof(ListCard), 
                new PropertyMetadata(null,new PropertyChangedCallback((s,e)=>
                { 

                })
                
                ));



        public bool CanInput
        {
            get { return (bool)GetValue(CanInputProperty); }
            set { SetValue(CanInputProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CanInput.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CanInputProperty =
            DependencyProperty.Register("CanInput", typeof(bool), typeof(ListCard), new PropertyMetadata(default(bool)));




        public object Parameter
        {
            get { return (object )GetValue(ParameterProperty); }
            set { SetValue(ParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Parameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ParameterProperty =
            DependencyProperty.Register("Parameter", typeof(object ), typeof(ListCard), new PropertyMetadata(default(object)));




        public ICommand BtnCommand
        {
            get { return (ICommand)GetValue(BtnCommandProperty); }
            set { SetValue(BtnCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BtnCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BtnCommandProperty =
            DependencyProperty.Register("BtnCommand", typeof(ICommand), typeof(ListCard), new PropertyMetadata(0));




    }
}
