using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using System.Diagnostics;
using System.Linq;

namespace Flashcards
{
    public class FlashcardPlayViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Card> _cards;
        private Card _currentCard;
        private bool _showingQuestion = true;
        private string _currentText;
        private int _currentIndex = 0;
        private double _rotation = 0;
        private System.Random _random = new System.Random();
        private int _correctAnswers = 0;
        private DateTime _startTime;

        public string CurrentText
        {
            get => _currentText;
            set
            {
                _currentText = value;
                OnPropertyChanged(nameof(CurrentText));
            }
        }

        public double Rotation
        {
            get => _rotation;
            set
            {
                _rotation = value;
                OnPropertyChanged(nameof(Rotation));
            }
        }

        public bool ShowAnswerButtons
        {
            get => !_showingQuestion;
            private set
            {
                _showingQuestion = !value;
                OnPropertyChanged(nameof(ShowAnswerButtons));
            }
        }

        public ICommand FlipCardCommand { get; }
        public ICommand CorrectAnswerCommand { get; }

        public FlashcardPlayViewModel(ObservableCollection<Card> cards)
        {
            _cards = new ObservableCollection<Card>(cards.OrderBy(x => _random.Next()));
            _startTime = DateTime.Now;
            
            if (_cards.Any())
            {
                _currentCard = _cards[0];
                CurrentText = _currentCard.Question;
            }

            FlipCardCommand = new Command(FlipCard);
            CorrectAnswerCommand = new Command(HandleCorrectAnswer);
        }

        private async void FlipCard()
        {
            if (_currentCard == null) return;

            // First half of the flip animation
            for (int i = 0; i <= 18; i++)
            {
                Rotation = i * 10;
                await Task.Delay(16); // 60 FPS
            }

            _showingQuestion = !_showingQuestion;
            CurrentText = _showingQuestion ? _currentCard.Question : _currentCard.Answer;
            OnPropertyChanged(nameof(ShowAnswerButtons));

            // Second half of the flip animation
            for (int i = 18; i <= 36; i++)
            {
                Rotation = i * 10;
                await Task.Delay(16); // 60 FPS
            }

            Rotation = 0;
            await Task.Delay(200); // Shorter delay after flip
        }

        private async void MoveToNextCard()
        {
            if (_currentIndex < _cards.Count - 1)
            {
                _currentIndex++;
                _currentCard = _cards[_currentIndex];
                _showingQuestion = true;
                CurrentText = _currentCard.Question;
                OnPropertyChanged(nameof(ShowAnswerButtons));
            }
            else
            {
                var timeSpent = DateTime.Now - _startTime;
                Debug.WriteLine($"Moving to ResultsPage with: CorrectAnswers={_correctAnswers}, TotalCards={_cards.Count}, TimeSpent={timeSpent}");
                try
                {
                    var resultsPage = new ResultsPage(_correctAnswers, _cards.Count, timeSpent);
                    Debug.WriteLine("ResultsPage created successfully");
                    await Application.Current.MainPage.Navigation.PushAsync(resultsPage);
                    Debug.WriteLine("Navigation completed");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error navigating to ResultsPage: {ex}");
                }
            }
        }

        public void HandleCorrectAnswer()
        {
            _correctAnswers++;
            MoveToNextCard();
        }

        public void HandleWrongAnswer()
        {
            MoveToNextCard();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
} 