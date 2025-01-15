namespace Calculator
{
    public partial class MainPage : ContentPage
    {
        private double sumTemp = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            int height = 675;
            int width = 475;
            //this.Window.MinimumHeight = height;
            //this.Window.MaximumHeight = height;

            //this.Window.MinimumWidth = width;
            //this.Window.MaximumWidth = width;

            this.Window.Height = height;
            this.Window.Width = width;
        }

        private void ClearButton_Clicked(object sender, EventArgs e)
        {

        }


        private void NumberButton(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            EntryCalculation.Text += button.Text;
        }

        private void OperandButton(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            EntryCalculation.Text = button.Text;
        }
    }

}
