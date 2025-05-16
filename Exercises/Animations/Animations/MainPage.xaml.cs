namespace Animations
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            var vm = new Animate2ViewModel();
            BindingContext = vm;

            vm.RotateBoxUIAction = RotateIt;
            vm.MoveBoxUIAction = MoveBox;
        }

        private void RotateIt(int angle)
        {
            if (this.box != null)
                this.box.RotateTo(angle);
        }

        private void MoveBox(int x)
        {
            if (this.box != null)
                this.box.TranslationX += x;
        }
    }



}
