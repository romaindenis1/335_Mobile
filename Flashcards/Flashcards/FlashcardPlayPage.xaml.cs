using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices.Sensors;

namespace Flashcards
{
    public partial class FlashcardPlayPage : ContentPage
    {
        private DateTime lastShakeTime = DateTime.MinValue;
        private const double ShakeThreshold = 10.0;
        private FlashcardPlayViewModel viewModel;

        public FlashcardPlayPage(CardViewModel cardViewModel)
        {
            InitializeComponent();
            viewModel = new FlashcardPlayViewModel(cardViewModel.Cards);
            BindingContext = viewModel;

            if (Accelerometer.Default.IsSupported)
            {
                Accelerometer.Default.ShakeDetected += Accelerometer_ShakeDetected;
                Accelerometer.Default.ReadingChanged += Accelerometer_ReadingChanged;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (Accelerometer.Default.IsSupported)
            {
                try
                {
                    Accelerometer.Default.Start(SensorSpeed.Game);
                }
                catch (Exception)
                {
                    // Handle if accelerometer cannot be started
                }
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (Accelerometer.Default.IsSupported)
            {
                try
                {
                    Accelerometer.Default.Stop();
                }
                catch (Exception)
                {
                    // Handle if accelerometer cannot be stopped
                }
            }
        }

        private void Accelerometer_ShakeDetected(object sender, EventArgs e)
        {
            HandleShake();
        }

        private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            var reading = e.Reading;
            // Calculate total acceleration
            double acceleration = Math.Sqrt(
                reading.Acceleration.X * reading.Acceleration.X +
                reading.Acceleration.Y * reading.Acceleration.Y +
                reading.Acceleration.Z * reading.Acceleration.Z);

            if (acceleration > ShakeThreshold)
            {
                HandleShake();
            }
        }

        private void HandleShake()
        {
            // Prevent multiple shakes from being detected too quickly
            if ((DateTime.Now - lastShakeTime).TotalMilliseconds < 1000)
                return;

            lastShakeTime = DateTime.Now;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (!viewModel.ShowAnswerButtons)
                    return;

                viewModel.HandleCorrectAnswer();
            });
        }
    }
} 