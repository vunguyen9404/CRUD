using DVDShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DVDShop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        SqlConnection conn { get; set; }
        public MainPage()
        {
            this.InitializeComponent();
            conn = new SqlConnection();
            conn.LoadDatabase();
            this.ShowMes();
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {

            } else
            {
                NavigationViewItem item = args.SelectedItem as NavigationViewItem;
                switch (item.Tag)
                {
                    case "Product Manager":
                        MainFrame.Navigate(typeof(Views.ProductViews), conn);
                        break;
                    case "Category Manager":
                        MainFrame.Navigate(typeof(Views.CategoryViews), conn);
                        break;
                }
            }
        }

        public async void ShowMes()
        {
            var dialog = new MessageDialog(conn.path);
            await dialog.ShowAsync();
        }
    }
}
