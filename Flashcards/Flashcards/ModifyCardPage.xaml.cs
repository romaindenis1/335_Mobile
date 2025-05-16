using Microsoft.Maui.Controls;

namespace Flashcards
{
    public partial class ModifyCardPage : ContentPage
    {
        public ModifyCardPage(CardViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            
            //Texte dynamique pour pas avoir edit card partout
            TitleLabel.Text = viewModel.SelectedCard.Question == null ? "Add Card" : "Edit Card";
        }
    }
}