using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Exam
{
    public partial class MainWindow : Window
    {
        public static Models.User User;
        public static MainWindow Main;
        public MainWindow()
        {
            InitializeComponent();
            Main = this;
            MainFrame.Navigate(new Pages.Authorization());
        }

        public static string GetImagePathByFileName(string fileName) =>
            $"{Directory.GetCurrentDirectory()}/Images/{fileName}";

    }
}