namespace Flashcards;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(ModifyCardPage), typeof(ModifyCardPage));
        Routing.RegisterRoute(nameof(AddCardPage), typeof(AddCardPage));
        Routing.RegisterRoute(nameof(AnswerPage), typeof(AnswerPage));
        Routing.RegisterRoute(nameof(ResultsPage), typeof(ResultsPage));
    }
}
