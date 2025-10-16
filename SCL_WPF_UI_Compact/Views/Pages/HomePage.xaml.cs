using SCL_WPF_UI_Compact.ViewModels.Pages;
using Wpf.Ui.Abstractions.Controls;

namespace SCL_WPF_UI_Compact.Views.Pages
{
    public partial class HomePage : INavigableView<HomeViewModel>
    {
        public HomeViewModel ViewModel { get; }

        public HomePage(HomeViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
            
        }

    }
}
