using Microsoft.Maui.Controls;
using Microsoft.Maui.Devices.Sensors;
using System.Diagnostics;

namespace Flashcards
{
    public partial class FlashcardPlayPage : ContentPage
    {
        private DateTime lastShakeTime = DateTime.MinValue;
        private const double ShakeThreshold = 2.0;
        private FlashcardPlayViewModel viewModel;
        private double lastAcceleration = 0;
        private const double AccelerationChangeThreshold = 0.8;

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
                    Accelerometer.Default.Start(SensorSpeed.UI);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Failed to start accelerometer: {ex.Message}");
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
                catch (Exception ex)
                {
                    Debug.WriteLine($"Failed to stop accelerometer: {ex.Message}");
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
            double acceleration = Math.Sqrt(
                reading.Acceleration.X * reading.Acceleration.X +
                reading.Acceleration.Y * reading.Acceleration.Y +
                reading.Acceleration.Z * reading.Acceleration.Z);

            double accelerationChange = Math.Abs(acceleration - lastAcceleration);
            lastAcceleration = acceleration;

            if (acceleration > ShakeThreshold || accelerationChange > AccelerationChangeThreshold)
            {
                HandleShake();
            }
        }

        private void HandleShake()
        {
            if ((DateTime.Now - lastShakeTime).TotalMilliseconds < 500)
            {
                return;
            }

            lastShakeTime = DateTime.Now;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (viewModel.ShowAnswerButtons)
                {
                    viewModel.HandleCorrectAnswer();
                }
            });
        }
    }
} 