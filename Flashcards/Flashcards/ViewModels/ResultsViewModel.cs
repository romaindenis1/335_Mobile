using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace Flashcards
{
    public class ResultsViewModel : INotifyPropertyChanged
    {
        private int _correctAnswers;
        private int _totalCards;
        private TimeSpan _timeSpent;

        public int CorrectAnswers
        {
            get => _correctAnswers;
            set
            {
                if (_correctAnswers != value)
                {
                    Debug.WriteLine($"Setting CorrectAnswers to: {value}");
                    _correctAnswers = value;
                    OnPropertyChanged(nameof(CorrectAnswers));
                    OnPropertyChanged(nameof(CorrectPercentage));
                }
            }
        }

        public int TotalCards
        {
            get => _totalCards;
            set
            {
                if (_totalCards != value)
                {
                    Debug.WriteLine($"Setting TotalCards to: {value}");
                    _totalCards = value;
                    OnPropertyChanged(nameof(TotalCards));
                    OnPropertyChanged(nameof(CorrectPercentage));
                }
            }
        }

        public TimeSpan TimeSpent
        {
            get => _timeSpent;
            set
            {
                if (_timeSpent != value)
                {
                    Debug.WriteLine($"Setting TimeSpent to: {value}");
                    _timeSpent = value;
                    OnPropertyChanged(nameof(TimeSpent));
                    OnPropertyChanged(nameof(FormattedTime));
                }
            }
        }

        public string FormattedTime
        {
            get
            {
                string result;
                if (TimeSpent.TotalHours >= 1)
                {
                    result = $"{TimeSpent.Hours}h {TimeSpent.Minutes}m {TimeSpent.Seconds}s";
                }
                else if (TimeSpent.TotalMinutes >= 1)
                {
                    result = $"{TimeSpent.Minutes}m {TimeSpent.Seconds}s";
                }
                else
                {
                    result = $"{TimeSpent.Seconds}s";
                }
                Debug.WriteLine($"FormattedTime: {result}");
                return result;
            }
        }

        public string CorrectPercentage
        {
            get
            {
                var percentage = TotalCards > 0 ? (int)((double)CorrectAnswers / TotalCards * 100) : 0;
                var result = $"{percentage}%";
                Debug.WriteLine($"CorrectPercentage: {result}");
                return result;
            }
        }

        public ICommand BackToMainCommand { get; }

        public ResultsViewModel(int correctAnswers, int totalCards, TimeSpan timeSpent)
        {
            Debug.WriteLine($"Creating ResultsViewModel with: CorrectAnswers={correctAnswers}, TotalCards={totalCards}, TimeSpent={timeSpent}");
            
            _correctAnswers = correctAnswers;
            _totalCards = totalCards;
            _timeSpent = timeSpent;

            OnPropertyChanged(nameof(CorrectAnswers));
            OnPropertyChanged(nameof(TotalCards));
            OnPropertyChanged(nameof(TimeSpent));
            OnPropertyChanged(nameof(CorrectPercentage));
            OnPropertyChanged(nameof(FormattedTime));

            BackToMainCommand = new Command(async () =>
            {
                await Application.Current.MainPage.Navigation.PopToRootAsync();
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            Debug.WriteLine($"PropertyChanged: {propertyName}");
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
} 