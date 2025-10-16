using SCL_WPF_UI_Compact.ViewModels.Pages;
using Wpf.Ui.Abstractions.Controls;

namespace SCL_WPF_UI_Compact.Views.Pages
{
    public partial class DataPage : INavigableView<DataViewModel>
    {
        public DataViewModel ViewModel { get; }

        public DataPage(DataViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }
    }
}
