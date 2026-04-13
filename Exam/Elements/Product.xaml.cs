using System.IO;
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
        public Product(Models.Product product)
        {
            InitializeComponent();
            if (product is not null)
            {
                SetData(product);
                SetPrice(product);
            }
                
        }

        private void SetData(Models.Product product)
        {
            if (String.IsNullOrEmpty(product.Image))
                Image.Source = new BitmapImage(new Uri($"{Directory.GetCurrentDirectory()}/Images/picture.png"));
            else
                Image.Source = new BitmapImage(new Uri($"{Directory.GetCurrentDirectory()}/Images/{product.Image}"));
            CategoryAndName.Content = $"{product.Category.Name} | {product.ProductType.Name}";
            Description.Content = $"Описание товара: {product}";
            Manufacturer.Content = $"Производитель: {product.Manufacturer.Name}";
            Provider.Content = $"Поставщик:{product.Provider.Name}";
            Price.Content = $"Цена: {product.Price}";
            Unit.Content = $"Еденица измерения: {product.Unit.Name}";
            Quantity.Content = $"Количество на складе: {product.QuantityStock}";
            Discount.Content = $"Скидка: {product.Discount}%";
            if (product.QuantityStock <= 0)
                BaseGrid.Background = Brushes.LightBlue;
            if (product.Discount > 15)
                BaseGrid.Background = (Brush)new BrushConverter().ConvertFromString("#2E8B57")!;
        }

        private void SetPrice(Models.Product product)
        {
            if (product.Discount <= 0)
                return;
            var discountPrice = product.Price - (product.Price / 100 * product.Discount);

            var oldPrice = new Run(product.Price.ToString())
            {
                TextDecorations = TextDecorations.Strikethrough,
                Foreground = Brushes.Red,
            };
            Price.Content = $"Цена: {oldPrice.Text} {discountPrice.ToString()}";
        }
    }
}
