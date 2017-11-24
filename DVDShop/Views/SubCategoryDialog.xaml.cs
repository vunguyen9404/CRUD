using DVDShop.Models;
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
    public sealed partial class SubCategoryDialog : ContentDialog
    {
        private SqlConnection conn { get; set; }
        private SubCategory SubCategory { get; set; }
        private ObservableCollection<Category> Category { get; set; }
        public SubCategoryDialog()
        {
            this.InitializeComponent();
        }

        public SubCategoryDialog(SqlConnection conn)
        {
            this.InitializeComponent();
            this.conn = conn;
            Category = new ObservableCollection<Models.Category>();
            this.GetCategory();
        }

        public SubCategoryDialog(SqlConnection conn, SubCategory category)
        {
            this.InitializeComponent();
            this.conn = conn;
            Category = new ObservableCollection<Models.Category>();
            this.GetCategory();
            this.SubCategory = category;
            this.Title = "Edit Category";
            SubCategoryName.Text = this.SubCategory.SubCategoryName;
            PrimaryButtonText = "Save";
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (this.SubCategory != null)
            {
                this.UpdateSubCategory();
            }
            else
            {
                this.AddSubCategory();
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {

        }

        private async void GetCategory()
        {
            try
            {
                var listData = conn.GetListCategory();
                Category.Clear();
                foreach (var item in listData)
                {
                    Category.Add(item);
                };
            }
            catch
            {
                await new MessageDialog("Something wrong !").ShowAsync();
            }
        }

        private async void UpdateSubCategory()
        {
            try
            {
                Category cate = CbCategory.SelectedItem as Category;
                conn.UpdateSubCategory(new SubCategory()
                {
                    SubCategoryID = this.SubCategory == null ? 1 : this.SubCategory.SubCategoryID,
                    SubCategoryName = SubCategoryName.Text,
                    Category = cate
                });
            }
            catch
            {
                await new MessageDialog("Something wrong !").ShowAsync();
            }
        }

        private async void AddSubCategory()
        {
            try
            {
                Category cate = CbCategory.SelectedItem as Category;
                conn.AddSubCategory(new SubCategory()
                {
                    SubCategoryID = this.SubCategory == null ? 1 : this.SubCategory.SubCategoryID,
                    SubCategoryName = SubCategoryName.Text,
                    Category = cate
                });
            }
            catch
            {
                await new MessageDialog("Something wrong !").ShowAsync();
            }
        }
    }
}
