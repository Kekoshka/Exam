using Exam.Context;
using Exam.Elements;
using Exam.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Exam.Pages
{
    /// <summary>
    /// Логика взаимодействия для Products.xaml
    /// </summary>
    public partial class Products : Page
    {
        List<Models.Product> _products;
        public Products()
        {
            InitializeComponent();
            LoadInterfaceByRoles();
            LoadInterface();
        }

        private void LoadInterface()
        {
            using var context = new AppDbContext();
            _products = context.Products
                .Include(p => p.Manufacturer)
                .Include(p => p.Provider)
                .Include(p => p.Category)
                .Include(p => p.Unit)
                .ToList();

            foreach (var product in _products)
            {
                ProductsList.Children.Add(new Elements.Product(product));
            }
            UserName.Content = MainWindow.User.FIO;
            var manufacturers = context.Manufacturers.ToList();
            manufacturers.Insert(0, new Manufacturer() { Id = 0, Name = "Все поставщики" });
            Manufacturers.ItemsSource = manufacturers;
        }

        private void LoadInterfaceByRoles()
        {
            if(MainWindow.User.Role.Id == 3)
            {
                OpenAddProductPageBtn.Visibility = Visibility.Visible;
                OpenOrdersPageBtn.Visibility = Visibility.Visible;
            }
            if (MainWindow.User.Role.Id == 2)
            {
                OpenOrdersPageBtn.Visibility = Visibility.Visible;
            }
        }

        private void OpenAuthorizationPage(object sender, RoutedEventArgs e)
        {
            MainWindow.Main.MainFrame.Navigate(new Authorization());
        }

        private void OpenOrdersPage(object sender, RoutedEventArgs e)
        {
            MainWindow.Main.MainFrame.Navigate(new Orders());
        }

        private void OpenAddProductPage(object sender, RoutedEventArgs e)
        {
            MainWindow.Main.MainFrame.Navigate(new SaveProduct());
        }

        private void SortProducts()
        {
            using var context = new AppDbContext();
            var productsQuery = _products.AsQueryable();
            productsQuery = productsQuery.Where(p =>
                (p.Manufacturer.Name.Contains(Search.Text) ||
                p.Provider.Name.Contains(Search.Text) ||
                p.Category.Name.Contains(Search.Text) ||
                p.Unit.Name.Contains(Search.Text)));
            if(Manufacturers.SelectedIndex > 0)
                productsQuery = productsQuery.Where(p => p.ManufacturerId == (int)Manufacturers.SelectedValue);
            if(OrderBy.SelectedIndex == 1)
                productsQuery = productsQuery.OrderBy(p => p.QuantityStock);
            else if(OrderBy.SelectedIndex == 2)
                productsQuery = productsQuery.OrderByDescending(p => p.QuantityStock);

            var products = productsQuery.ToList();
            ProductsList.Children.Clear();
            foreach(var product in products)
                ProductsList.Children.Add(new Elements.Product(product));
        }

        private void SortProducts(object sender, TextChangedEventArgs e) =>
            SortProducts();

        private void SortProducts(object sender, SelectionChangedEventArgs e) =>
            SortProducts();
    }
}
