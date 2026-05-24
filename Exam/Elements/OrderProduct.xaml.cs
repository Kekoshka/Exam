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

namespace Exam.Elements
{
    /// <summary>
    /// Логика взаимодействия для OrderProduct.xaml
    /// </summary>
    public partial class OrderProduct : UserControl
    {
        public Models.OrderProduct _orderProduct;
        public OrderProduct(Models.OrderProduct orderProduct = null)
        {
            InitializeComponent();
            _orderProduct = orderProduct;
            LoadBaseInterface();
            if (orderProduct is not null)
                LoadInterface();
        }

        private void LoadBaseInterface()
        {
            using var context = new Context.AppDbContext();
            var products = context.Products.ToList();
            Products.ItemsSource = products;
        }
        private void LoadInterface()
        {
            Products.SelectedValue = _orderProduct.ProductId;
            Quantity.Text = _orderProduct.Quantity.ToString();
        }

        private void Delete(object sender, RoutedEventArgs e)
        {
            if(this.Parent is Panel panel)
                panel.Children.Remove(this);
        }
    }
}
