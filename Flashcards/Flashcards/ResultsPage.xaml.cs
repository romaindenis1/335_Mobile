using System.Diagnostics;

namespace Flashcards;

public partial class ResultsPage : ContentPage
{
	public ResultsPage(int correctAnswers, int totalCards, TimeSpan timeSpent)
	{
		Debug.WriteLine($"ResultsPage constructor called with: CorrectAnswers={correctAnswers}, TotalCards={totalCards}, TimeSpent={timeSpent}");
		
		try
		{
			InitializeComponent();
			Debug.WriteLine("InitializeComponent completed");
			
			var viewModel = new ResultsViewModel(correctAnswers, totalCards, timeSpent);
			Debug.WriteLine("ResultsViewModel created");
			
			BindingContext = viewModel;
			Debug.WriteLine($"BindingContext set to ResultsViewModel. CorrectAnswers={viewModel.CorrectAnswers}, TotalCards={viewModel.TotalCards}");
		}
		catch (Exception ex)
		{
			Debug.WriteLine($"Error in ResultsPage constructor: {ex}");
		}
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();
		Debug.WriteLine("ResultsPage OnAppearing called");
		if (BindingContext is ResultsViewModel viewModel)
		{
			Debug.WriteLine($"OnAppearing - ViewModel values: CorrectAnswers={viewModel.CorrectAnswers}, TotalCards={viewModel.TotalCards}");
		}
		else
		{
			Debug.WriteLine("OnAppearing - BindingContext is not ResultsViewModel");
		}
	}
}