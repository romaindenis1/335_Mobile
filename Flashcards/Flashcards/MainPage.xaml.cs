using Microsoft.Maui.Controls;

namespace Flashcards
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new CardViewModel();
        }
    }
}