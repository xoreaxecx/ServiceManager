using ServiceManager.ViewModels;
using System.Windows;

namespace ServiceManager.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new MainVM();
            InitializeComponent();
        }
    }
}
