using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

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
        }

        public ICommand FlipCardCommand { get; }
        public ICommand IncorrectAnswerCommand { get; }

        public FlashcardPlayViewModel(ObservableCollection<Card> cards)
        {
            _cards = new ObservableCollection<Card>(cards);
            if (_cards.Any())
            {
                _currentCard = _cards[0];
                CurrentText = _currentCard.Question;
            }

            FlipCardCommand = new Command(FlipCard);
            IncorrectAnswerCommand = new Command(HandleIncorrectAnswer);
        }

        private async void FlipCard()
        {
            if (_currentCard == null) return;

            for (int i = 0; i <= 18; i++)
            {
                Rotation = i * 10;
                await Task.Delay(20);
            }

            _showingQuestion = !_showingQuestion;
            CurrentText = _showingQuestion ? _currentCard.Question : _currentCard.Answer;
            OnPropertyChanged(nameof(ShowAnswerButtons));

            for (int i = 18; i <= 36; i++)
            {
                Rotation = i * 10;
                await Task.Delay(20);
            }

            Rotation = 0;
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
                await Application.Current.MainPage.Navigation.PopAsync();
            }
        }

        public void HandleCorrectAnswer()
        {
            MoveToNextCard();
        }

        private void HandleIncorrectAnswer()
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