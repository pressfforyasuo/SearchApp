using SearchApp.Services;
using SearchApp.ViewModels;
using System.Windows;

namespace SearchApp
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainViewModel();
            DataContext = viewModel;
            Closed += MainWindow_Closed;
        }

        private void MainWindow_Closed(object? sender, EventArgs e)
        {
            MainModelSerializer serializer = new MainModelSerializer("MainModelData.xml");

            serializer.Serialize(viewModel.Model);
        }
    }
}