using Microsoft.Maui.Controls;

namespace Flashcards
{
    public partial class DeleteCardPage : ContentPage
    {
        public DeleteCardPage(CardViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}