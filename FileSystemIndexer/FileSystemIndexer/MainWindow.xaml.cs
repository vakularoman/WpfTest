using System.Windows;
using FileSystemIndexer.ViewModels;

namespace FileSystemIndexer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {
        public MainViewModel ViewModel { get; }
         
        public MainWindowView()
        {
            ViewModel = new MainViewModel();
            DataContext = this;

            InitializeComponent();
        }
    }
}