using SCL_WPF_UI_Compact.ViewModels.Pages;
using Wpf.Ui.Abstractions.Controls;

namespace SCL_WPF_UI_Compact.Views.Pages
{
    public partial class SettingsPage : INavigableView<SettingsViewModel>
    {
        public SettingsViewModel ViewModel { get; }

        public SettingsPage(SettingsViewModel viewModel)
        {
            ViewModel = viewModel;
            DataContext = this;

            InitializeComponent();
        }
    }
}
