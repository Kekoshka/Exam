using Exam.Context;
using Exam.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Exam.Elements
{
    /// <summary>
    /// Логика взаимодействия для Product.xaml
    /// </summary>
    public partial class Product : UserControl
    {
        Models.Product _product;
        public Product(Models.Product product)
        {
            InitializeComponent();
            _product = product;
            LoadInterfaceByRoles();
            if (product is not null)
            {
                SetData();
                SetPrice();
            }
                
        }

        private void SetData()
        {
            if (String.IsNullOrEmpty(_product.Image))
                Image.Source = new BitmapImage(new Uri($"{Directory.GetCurrentDirectory()}/Images/picture.png"));
            else
                Image.Source = new BitmapImage(new Uri($"{Directory.GetCurrentDirectory()}/Images/{_product.Image}"));
            CategoryAndName.Content = $"{_product.Category.Name} | {_product.Name}";
            Description.Content = $"Описание товара: {_product}";
            Manufacturer.Content = $"Производитель: {_product.Manufacturer.Name}";
            Provider.Content = $"Поставщик:{_product.Provider.Name}";
            Price.Text = $"Цена: {_product.Price}";
            Unit.Content = $"Еденица измерения: {_product.Unit.Name}";
            Quantity.Content = $"Количество на складе: {_product.QuantityStock}";
            Discount.Content = $"Скидка: {_product.Discount}%";
            if (_product.QuantityStock <= 0)
                BaseGrid.Background = Brushes.LightBlue;
            if (_product.Discount > 15)
                BaseGrid.Background = (Brush)new BrushConverter().ConvertFromString("#2E8B57")!;
        }

        private void LoadInterfaceByRoles()
        {
            if (MainWindow.User.Role.Id == 3)
            {
                EditBtn.Visibility = Visibility.Visible;
                DeleteBtn.Visibility = Visibility.Visible;
            }
        }

        private void SetPrice()
        {
            if (_product.Discount <= 0)
                return;
            var discountPrice = CalculatePrice(_product.Price, _product.Discount);

            var oldPrice = new Run(_product.Price.ToString())
            {
                TextDecorations = TextDecorations.Strikethrough,
                Foreground = Brushes.Red,
            };
            Price.Text = "";
            Price.Inlines.Add("Цена: ");
            Price.Inlines.Add(oldPrice);
            Price.Inlines.Add(" " + discountPrice.ToString());
        }
        private int CalculatePrice(int basePrice, int discount)
        {
            try
            {
                var discountPrice = basePrice - (basePrice / 100 * discount);
                return discountPrice;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        private void Edit(object sender, RoutedEventArgs e)
        {
            MainWindow.Main.MainFrame.Navigate(new Pages.SaveProduct(_product));
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы действительно хотите удалить товар?", "Предупреждение", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes)
                return;
            var context = new AppDbContext();
            context.Products
                .Where(p => p.Id == _product.Id)
                .ExecuteDelete();
            MainWindow.Main.MainFrame.Navigate(new Pages.Products());
        }
    }
}
