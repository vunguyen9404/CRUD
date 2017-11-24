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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace DVDShop.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CategoryViews : Page
    {
        public SqlConnection conn { get; set; }
        private ObservableCollection<Category> ListCategory { get; set; }
        private ObservableCollection<SubCategory> ListSubCategory { get; set; }
        public CategoryViews()
        {
            this.InitializeComponent();
            ListCategory = new ObservableCollection<Category>();
            ListSubCategory = new ObservableCollection<SubCategory>();
        }

        private void SelectListCategory_Click(object sender, RoutedEventArgs e)
        {
            if (lvCategory.SelectionMode == ListViewSelectionMode.Multiple)
            {
                lvCategory.SelectionMode = ListViewSelectionMode.Single;
                DeleteMultiCategory.IsEnabled = false;
            } else
            {
                lvCategory.SelectionMode = ListViewSelectionMode.Multiple;
                DeleteMultiCategory.IsEnabled = true;
            }
        }

        private async void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            var cate = new CategoryDialog(conn);
            cate.Closing += this.UpdateListCategory;
            await cate.ShowAsync();
        }

        private void UpdateListCategory(ContentDialog a, ContentDialogClosingEventArgs args)
        {
            this.GetListCategory();
        }

        private void UpdateListSubCategory(ContentDialog a, ContentDialogClosingEventArgs args)
        {
            this.GetListSubCategory();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            conn = e.Parameter as SqlConnection;
            this.GetListCategory();
            this.GetListSubCategory();
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
                await new MessageDialog("Something wrong !").ShowAsync();
            }
        }

        private async void GetListSubCategory()
        {
            try
            {
                var listData = conn.GetListSubCategory();
                ListSubCategory.Clear();
                foreach (var item in listData)
                {
                    ListSubCategory.Add(item);
                };
            }
            catch
            {
                await new MessageDialog("Something wrong !").ShowAsync();
            }
        }


        private async void DeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            try
            {
                conn.DeleteCategory(new Category()
                {
                    CategoryID = Int32.Parse(btn.Tag.ToString())
                });
                ListCategory.Where(x => x.CategoryID == int.Parse(btn.Tag.ToString())).ToList().All(i => ListCategory.Remove(i));
            } catch
            {
                await new MessageDialog("Something wrong !").ShowAsync();
            }
        }

        private void DeleteMultiCategory_Click(object sender, RoutedEventArgs e)
        {
            foreach(Category item in lvCategory.Items)
            {
                ListCategory.Remove(item);
                conn.DeleteCategory(item);
            }
        }

        private async void EditCategory_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Category category = ListCategory.Where(x => x.CategoryID == int.Parse(btn.Tag.ToString())).ToList().ElementAt(0);
            var cate = new CategoryDialog(conn, category);
            cate.Closing += this.UpdateListCategory;
            await cate.ShowAsync();
        }

        private void DeleteMultiSubCategory_Click(object sender, RoutedEventArgs e)
        {
            foreach (SubCategory item in lvSubCategory.SelectedItems)
            {
                ListSubCategory.Remove(item);
                conn.DeleteSubCategory(item);
            }
        }

        private void SelectListSubCategory_Click(object sender, RoutedEventArgs e)
        {
            if (lvSubCategory.SelectionMode == ListViewSelectionMode.Multiple)
            {
                lvSubCategory.SelectionMode = ListViewSelectionMode.Single;
                DeleteMultiCategory.IsEnabled = false;
            }
            else
            {
                lvSubCategory.SelectionMode = ListViewSelectionMode.Multiple;
                DeleteMultiCategory.IsEnabled = true;
            }
        }

        private async void AddSubCategory_Click(object sender, RoutedEventArgs e)
        {
            var cate = new SubCategoryDialog(conn);
            cate.Closing += this.UpdateListSubCategory;
            await cate.ShowAsync();
        }

        private async void EditSubCategory_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            SubCategory subCategory = ListSubCategory.Where(x => x.SubCategoryID == int.Parse(btn.Tag.ToString())).ToList().ElementAt(0);
            var cate = new SubCategoryDialog(conn, subCategory);
            cate.Closing += this.UpdateListSubCategory;
            await cate.ShowAsync();
        }

        private async void DeleteSubCategory_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            try
            {
                conn.DeleteSubCategory(new SubCategory()
                {
                    SubCategoryID = Int32.Parse(btn.Tag.ToString())
                });
                ListSubCategory.Where(x => x.SubCategoryID == int.Parse(btn.Tag.ToString())).ToList().All(i => ListSubCategory.Remove(i));
            }
            catch
            {
                await new MessageDialog("Something wrong !").ShowAsync();
            }
        }

        private async void lvCategory_ItemClick(object sender, ItemClickEventArgs e)
        {
            Category cat = e.ClickedItem as Category;
            try
            {
                var listData = conn.GetListSubCategory();
                ListSubCategory.Clear();
                listData.FindAll(x => x.Category.CategoryID == cat.CategoryID).ToList().ForEach(i => {
                    ListSubCategory.Add(i);
                });
            }
            catch
            {
                await new MessageDialog("Something wrong !").ShowAsync();
            }
        }
    }
}
