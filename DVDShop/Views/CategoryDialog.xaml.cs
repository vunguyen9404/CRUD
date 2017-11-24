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

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DVDShop.Views
{
    public sealed partial class CategoryDialog : ContentDialog
    {
        public string result { get; set; }
        private SqlConnection conn { get; set; }
        private Category category { get; set; }
        public CategoryDialog()
        {
            this.InitializeComponent();
        }

        public CategoryDialog(SqlConnection conn)
        {
            this.InitializeComponent();
            this.conn = conn;
        }

        public CategoryDialog(SqlConnection conn, Category category)
        {
            this.InitializeComponent();
            this.conn = conn;
            this.category = category;
            this.Title = "Edit Category";
            CategoryName.Text = this.category.CategoryName;
            PrimaryButtonText = "Save";
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (this.category != null)
            {
                this.UpdateCategory();
            } else
            {
                this.AddCategory();
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }

        private async void UpdateCategory()
        {
            try
            {
                conn.UpdateCategory(new Category()
                {
                    CategoryID = this.category == null ? 1:this.category.CategoryID,
                    CategoryName = CategoryName.Text
                });
                result = "OK";
            } catch
            {
                await new MessageDialog("Something wrong !").ShowAsync();
            }
        }

        private async void AddCategory()
        {
            try
            {
                conn.AddCategory(new Category()
                {
                    CategoryID = this.category == null ? 1 : this.category.CategoryID,
                    CategoryName = CategoryName.Text
                });
                result = "OK";
            }
            catch
            {
                await new MessageDialog("Something wrong !").ShowAsync();
            }
        }
    }
}
