using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
namespace _3UserInteraction.ViewModels
{
    public partial class Mvvm1ViewModel : ObservableObject
    {
        [ObservableProperty]
        private int counter = 0;

        [RelayCommand]
        private void Increment(int incrementValue)
        {
            Counter += incrementValue;
        }
    }
}
