using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System.Diagnostics;
using Flashcards;

namespace Flashcards
{
    public class CardViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Card> _cards;
        private Card _selectedCard;
        private Card _originalCard; // Store original card for cancellation

        public ObservableCollection<Card> Cards
        {
            get => _cards;
            set
            {
                _cards = value;
                OnPropertyChanged(nameof(Cards));
            }
        }

        public Card SelectedCard
        {
            get => _selectedCard;
            set
            {
                _selectedCard = value;
                OnPropertyChanged(nameof(SelectedCard));
            }
        }

        public ICommand AddCardCommand { get; }
        public ICommand EditCardCommand { get; }
        public ICommand DeleteCardCommand { get; }
        public ICommand SaveCardCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand PlayCommand { get; }

        public CardViewModel()
        {
            Cards = new ObservableCollection<Card>
            {
                new Card { Question = "Quelle est la capitale du kenya?", Answer = "Nairobi" },
                new Card { Question = "Quelle est la capitale du danemark?", Answer = "Copenhagen" }
            };

            PlayCommand = new Command(async () =>
            {
                if (Cards.Any())
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new FlashcardPlayPage(this));
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("No Cards", "Please add some flashcards first!", "OK");
                }
            });

            AddCardCommand = new Command(async () =>
            {
                SelectedCard = new Card();
                _originalCard = null; // No original card for new cards
                await Application.Current.MainPage.Navigation.PushAsync(new ModifyCardPage(this));
            });

            EditCardCommand = new Command<Card>(async (card) =>
            {
                if (card != null)
                {
                    SelectedCard = new Card 
                    { 
                        Question = card.Question,
                        Answer = card.Answer
                    };
                    _originalCard = card; // Store original card
                    await Application.Current.MainPage.Navigation.PushAsync(new ModifyCardPage(this));
                }
            });

            DeleteCardCommand = new Command<Card>(async (card) =>
            {
                if (card != null)
                {
                    Cards.Remove(card);
                }
            });

            SaveCardCommand = new Command(async () =>
            {
                if (SelectedCard == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No card selected!", "OK");
                    return;
                }

                if (string.IsNullOrWhiteSpace(SelectedCard.Question))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Question cannot be empty!", "OK");
                    return;
                }

                if (string.IsNullOrWhiteSpace(SelectedCard.Answer))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Answer cannot be empty!", "OK");
                    return;
                }

                SelectedCard.Question = SelectedCard.Question.Trim();
                SelectedCard.Answer = SelectedCard.Answer.Trim();

                if (_originalCard != null)
                {
                    _originalCard.Question = SelectedCard.Question;
                    _originalCard.Answer = SelectedCard.Answer;
                }
                else if (!Cards.Contains(SelectedCard))
                {
                    Cards.Add(SelectedCard);
                }
                
                await Application.Current.MainPage.Navigation.PopAsync();
            });

            CancelCommand = new Command(async () =>
            {
                if (_originalCard != null)
                {
                    SelectedCard.Question = _originalCard.Question;
                    SelectedCard.Answer = _originalCard.Answer;
                }
                await Application.Current.MainPage.Navigation.PopAsync();
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}