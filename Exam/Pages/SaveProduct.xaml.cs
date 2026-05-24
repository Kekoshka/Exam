using Exam.Elements;
using Exam.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
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
    public partial class SaveProduct : Page
    {
        Models.Product _product;
        public SaveProduct(Models.Product product = null)
        {
            InitializeComponent();
            _product = product;
            LoadBaseInterface();
            if (product is not null)
                LoadInterface();
        }

        private void LoadInterface()
        {
            if (!string.IsNullOrEmpty(_product.Image))
            {
                Image.Source = Image.Source = new BitmapImage(new Uri($"{Directory.GetCurrentDirectory()}/Images/{_product.Image}"));
                Image.Tag = _product.Image;
            }
            Article.Text = _product.Article;
            Name.Text = _product.Name;
            Description.Text = _product.Description;
            Priсe.Text = _product.Price.ToString();
            Quantity.Text = _product.QuantityStock.ToString();
            Discount.Text = _product.Discount.ToString();
            Units.SelectedValue = _product.UnitId;
            Categories.SelectedValue = _product.CategoryId;
            Manufacturers.SelectedValue = _product.ManufacturerId;
            Providers.SelectedValue = _product.ProviderId;
        }
        private void LoadBaseInterface()
        {
            using var context = new Context.AppDbContext();
            var units = context.Units.ToList();
            var categories = context.Categories.ToList();
            var manufacturers = context.Manufacturers.ToList();
            var providers = context.Providers.ToList();
            Units.ItemsSource = units;
            Categories.ItemsSource = categories;
            Manufacturers.ItemsSource = manufacturers;
            Providers.ItemsSource = providers;
        }
        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Article.Text))
            {
                MessageBox.Show("Введите название");
                return false;
            }
            if (string.IsNullOrWhiteSpace(Name.Text))
            {
                MessageBox.Show("Введите название");
                return false;
            }
            if (string.IsNullOrWhiteSpace(Description.Text))
            {
                MessageBox.Show("Введите описание");
                return false;
            }
            if (!int.TryParse(Priсe.Text, out int price) || price < 0)
            {
                MessageBox.Show("Введите цену от 0");
                return false;
            }
            if (!int.TryParse(Quantity.Text, out int quantity) || quantity < 0)
            {
                MessageBox.Show("Введите количество на складе от 0");
                return false;
            }
            if (!int.TryParse(Discount.Text, out int discount) || discount < 0 || discount > 100)
            {
                MessageBox.Show("Введите скидку от 0 до 100");
                return false;
            }
            if (Units.SelectedItem is null)
            {
                MessageBox.Show("Выберите единицу измерения");
                return false;
            }
            if (Categories.SelectedItem is null)
            {
                MessageBox.Show("Выберите категорию");
                return false;
            }
            if (Manufacturers.SelectedItem is null)
            {
                MessageBox.Show("Выберите производителя");
                return false;
            }
            if (Providers.SelectedItem is null)
            {
                MessageBox.Show("Выберите поставщика");
                return false;
            }
            return true;
        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (!Validate())
                return;
            
            using var context = new Context.AppDbContext();
            
            if (_product is null)
            {
                Models.Product product = new()
                {
                    Article = Article.Text,
                    Name = Name.Text,
                    Description = Description.Text,
                    Price = int.Parse(Priсe.Text),
                    QuantityStock = int.Parse(Quantity.Text),
                    Discount = int.Parse(Discount.Text),
                    UnitId = (int)Units.SelectedValue,
                    CategoryId = (int)Categories.SelectedValue,
                    ManufacturerId = (int)Manufacturers.SelectedValue,
                    ProviderId = (int)Providers.SelectedValue,
                    Image = Image.Tag as string ?? string.Empty
                };
                context.Products.Add(product);
            }
            else
            {
                var oldProduct = context.Products.Find(_product.Id);
                if (oldProduct is null)
                {
                    MessageBox.Show("Товар не найден");
                    return;
                }
                oldProduct.Article = Article.Text;
                oldProduct.Name = Name.Text;
                oldProduct.Description = Description.Text;
                oldProduct.Price = int.Parse(Priсe.Text);
                oldProduct.QuantityStock = int.Parse(Quantity.Text);
                oldProduct.Discount = int.Parse(Discount.Text);
                oldProduct.UnitId = (int)Units.SelectedValue;
                oldProduct.CategoryId = (int)Categories.SelectedValue;
                oldProduct.ManufacturerId = (int)Manufacturers.SelectedValue;
                oldProduct.ProviderId = (int)Providers.SelectedValue;
                oldProduct.Image = Image.Tag.ToString() ?? string.Empty;
            }
            context.SaveChanges();
            MainWindow.Main.MainFrame.Navigate(new Products());
        }
        private void SelectImage(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            if (ofd.ShowDialog() == false)
                return;

            var imageName = Guid.NewGuid().ToString();
            var newImagePath = MainWindow.GetImagePathByFileName(imageName);

            File.Copy(ofd.FileName, newImagePath);
            //if (_product is not null && !string.IsNullOrEmpty(_product.Image))
            //    File.Delete(MainWindow.GetImagePathByFileName(_product.Image)); доделать, выскакивает ошибка
            Image.Source = new BitmapImage(new Uri(newImagePath));
            Image.Tag = imageName;
        }

        private void OpenProductsPage(object sender, RoutedEventArgs e) =>
            MainWindow.Main.MainFrame.Navigate(new Products());
        
    }
}
