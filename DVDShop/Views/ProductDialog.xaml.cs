﻿using DVDShop.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DVDShop.Views
{
    public sealed partial class ProductDialog : ContentDialog
    {
        private SqlConnection conn { get; set; }
        private Product product { get; set; }
        private ObservableCollection<Category> ListCategory { get; set; }
        private ObservableCollection<SubCategory> ListSubCategory { get; set; }
        public ProductDialog()
        {
            this.InitializeComponent();
        }

        public ProductDialog(SqlConnection conn)
        {
            this.InitializeComponent();
            this.conn = conn;
            ListCategory = new ObservableCollection<Category>();
            ListSubCategory = new ObservableCollection<SubCategory>();
            this.GetListCategory();


        }

        public ProductDialog(SqlConnection conn, Product product)
        {
            this.conn = conn;
            this.InitializeComponent();
            ListCategory = new ObservableCollection<Category>();
            ListSubCategory = new ObservableCollection<SubCategory>();
            this.GetListCategory();

            tbProductName.Text = product.ProductName;
            tbProductDesc.Text = product.ProductDesc;
            tbProductImage.Text = product.ProductImage;
            tbProductTrailer.Text = product.ProductTrailer;
            tbProductPrice.Text = product.ProductPrice + "";
        }

        private async void GetListCategory()
        {
            try
            {
                var listData = conn.GetListCategory();
                ListCategory.Clear();
                foreach (var item in listData)
                {
                    ListCategory.Add(item);
                };
            }
            catch
            {
                await new MessageDialog("Get Category Faild ! Something wrong !").ShowAsync();
            }
        }

        private async void GetListSubCategory()
        {
            try
            {
                var listData = conn.GetListSubCategory();
                ListSubCategory.Clear();
                var itemC = cbCategory.SelectedItem as Category;
                foreach (var item in listData)
                {
                    if (item.Category.CategoryID == itemC.CategoryID)
                    {
                        ListSubCategory.Add(item);
                    }
                };
            }
            catch
            {
                await new MessageDialog("Get SubCategory failed ! ..Something wrong !").ShowAsync();
            }
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (product != null)
            {
                this.EditProduct();
            } else
            {
                this.AddProduct();
            }
        }

        private async void AddProduct()
        {
            try
            {
                conn.AddProduct(new Product()
                {
                    ProductName = tbProductName.Text,
                    ProductDesc = tbProductDesc.Text,
                    ProductPrice = double.Parse(tbProductPrice.Text),
                    ProductImage = tbProductImage.Text,
                    ProductTrailer = tbProductTrailer.Text,
                    Category = cbCategory.SelectedItem as Category,
                    SubCategory = cbSubCategory.SelectedItem as SubCategory
                });
            }
            catch
            {
                await new MessageDialog("Add product failed !..Something wrong !").ShowAsync();
            }
        }

        private async void EditProduct()
        {
            try
            {
                conn.UpdateProduct(new Product()
                {
                    ProductID = product.ProductID,
                    ProductName = tbProductName.Text,
                    ProductDesc = tbProductDesc.Text,
                    ProductPrice = double.Parse(tbProductPrice.Text),
                    ProductImage = tbProductImage.Text,
                    ProductTrailer = tbProductTrailer.Text,
                    Category = cbCategory.SelectedItem as Category,
                    SubCategory = cbSubCategory.SelectedItem as SubCategory
                });
            }
            catch
            {
                await new MessageDialog("Edit product failed ! Something wrong !").ShowAsync();
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void cbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.GetListSubCategory();
        }
    }
}
