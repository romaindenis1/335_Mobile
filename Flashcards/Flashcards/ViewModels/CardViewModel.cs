using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using Flashcards;

namespace Flashcards
{
    public class CardViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Card> _cards;
        private Card _selectedCard;

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
                await Application.Current.MainPage.Navigation.PushAsync(new ModifyCardPage(this));
            });

            EditCardCommand = new Command<Card>(async (card) =>
            {
                SelectedCard = card;
                await Application.Current.MainPage.Navigation.PushAsync(new ModifyCardPage(this));
            });

            DeleteCardCommand = new Command<Card>(async (card) =>
            {
                SelectedCard = card;
                await Application.Current.MainPage.Navigation.PushAsync(new DeleteCardPage(this));
            });

            SaveCardCommand = new Command(async () =>
            {
                if (!string.IsNullOrWhiteSpace(SelectedCard.Question))
                {
                    if (!Cards.Contains(SelectedCard))
                    {
                        Cards.Add(SelectedCard);
                    }
                    
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
            });

            CancelCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PopAsync();
            });

            DeleteCardCommand = new Command<Card>(async (card) =>
            {
                if (card != null)
                {
                    Cards.Remove(card);
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}