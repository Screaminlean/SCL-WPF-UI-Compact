namespace SCL_WPF_UI_Compact.ViewModels.Pages
{
    public partial class HomeViewModel : ObservableObject
    {
        [ObservableProperty]
        private int _counter = 0;

        [RelayCommand]
        private void OnCounterIncrement()
        {
            Counter++;
        }
    }
}
