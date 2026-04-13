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
            _products = LoadData();
            SetData(_products);
        }

        private List<Models.Product> LoadData()
        {
            using var context = new AppDbContext();
            return context.Products
                .Include(p => p.Manufacturer)
                .Include(p => p.ProductType)
                .Include(p => p.Provider)
                .Include(p => p.Category)
                .Include(p => p.Unit)
                .Include(p => p.ProductType)
                .ToList();
        }
        private void SetData(List<Models.Product> products)
        {
            foreach(var product in products)
            {
                ProductsList.Children.Add(new Elements.Product(product));
            }
        }
        private void OpenAuthorizationPage(object sender, RoutedEventArgs e)
        {
            MainWindow.Main.MainFrame.Navigate(new Authorization());
        }
    }
}
