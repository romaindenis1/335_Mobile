using Microsoft.Maui.Controls;

namespace Flashcards
{
    public partial class ModifyCardPage : ContentPage
    {
        public ModifyCardPage(CardViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}