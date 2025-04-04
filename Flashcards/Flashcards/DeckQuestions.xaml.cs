namespace Flashcards;

public partial class DeckQuestions : ContentPage
{
	public DeckQuestions()
	{
		InitializeComponent();
	}
    private async void BackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MainPage());
    }
}