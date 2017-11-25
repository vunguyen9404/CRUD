using DVDShop.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DVDShop.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProductViews : Page
    {
        Models.SqlConnection conn { get; set; }
        ObservableCollection<Category> ListCategory { get; set; }
        ObservableCollection<SubCategory> ListSubCategory { get; set; }
        ObservableCollection<Product> ListProduct { get; set; }
        public ProductViews()
        {
            this.InitializeComponent();
            ListCategory = new ObservableCollection<Category>();
            ListProduct = new ObservableCollection<Product>();
            ListSubCategory = new ObservableCollection<SubCategory>();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            conn = e.Parameter as Models.SqlConnection;
            this.AddListCategoryAsync();
            this.AddListSubCategory();
            this.AddListProduct();
        }

        public void AddListProduct()
        {
            List<Product> list = conn.GetListProduct();
            foreach(var item in list)
            {
                ListProduct.Add(item);
            }
        }

        public void AddListCategoryAsync()
        {
            var List = conn.GetListCategory();
            ListCategory = new ObservableCollection<Category>(List);
        }

        public void AddListSubCategory()
        {
            ListSubCategory = new ObservableCollection<SubCategory>(conn.GetListSubCategory());
        }

        private void SelectList_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            var product = new ProductDialog(conn);
            product.Closing += this.UpdateListProduct;
            await product.ShowAsync();
        }

        private void UpdateListProduct(ContentDialog a, ContentDialogClosingEventArgs args)
        {
            ListProduct.Clear();
            this.AddListProduct();
        }

        private async void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var productSelect = ListProduct.First(x => x.ProductID == int.Parse(btn.Tag.ToString()));
            var product = new ProductDialog(conn, productSelect);
            product.Closing += this.UpdateListProduct;
            await product.ShowAsync();
        }

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var product = ListProduct.First(x => x.ProductID == int.Parse(btn.Tag.ToString()));
            ListProduct.Remove(product);
            conn.DeleteProduct(product);
        }
    }
}
