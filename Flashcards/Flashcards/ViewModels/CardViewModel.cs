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

        public CardViewModel()
        {
            Cards = new ObservableCollection<Card>
            {
                new Card { Title = "Sample Card 1", Description = "This is a sample description." },
                new Card { Title = "Sample Card 2", Description = "Another sample description." }
            };

            AddCardCommand = new Command(async () =>
            {
                SelectedCard = new Card();
                await Application.Current.MainPage.Navigation.PushAsync(new ModifyCardPage(this));
            });

            EditCardCommand = new Command<Card>(async (card) =>
            {
                SelectedCard = card; // Use the original card reference
                await Application.Current.MainPage.Navigation.PushAsync(new ModifyCardPage(this));
            });

            DeleteCardCommand = new Command<Card>(async (card) =>
            {
                SelectedCard = card;
                await Application.Current.MainPage.Navigation.PushAsync(new DeleteCardPage(this));
            });

            SaveCardCommand = new Command(async () =>
            {
                if (!string.IsNullOrWhiteSpace(SelectedCard.Title))
                {
                    if (!Cards.Contains(SelectedCard)) // Add new card if not in collection
                    {
                        Cards.Add(SelectedCard);
                    }
                    // Updates are handled by Card's INotifyPropertyChanged
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
                    Cards.Remove(card); // Remove the card from the collection
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