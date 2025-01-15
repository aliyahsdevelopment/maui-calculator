namespace Calculator
{
    public partial class MainPage : ContentPage
    {
        private double sumTemp = 0;
        private string operation = "";
        private double operand = 0;

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
            EntryCalculation.Text = "0";
            sumTemp = 0;
        }


        private void NumberButton(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            EntryCalculation.Text += button.Text;
            operand += double.Parse(button.Text);
        }

        private void OperandButton(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            EntryCalculation.Text += button.Text;
            operation = button.Text;

        }
        private void EqualsButton(object sender, EventArgs e)
        {
            Calculate();
        }

        public void Calculate()
        {
            switch (operation)
            {
                case "+":
                    sumTemp += operand;
                    break;
                case "-":
                    sumTemp -= operand;
                    break;
                case "*":
                    sumTemp *= operand;
                    break;
                case "/":
                    if (operand == 0)
                    {
                        DisplayAlert("Fel", "Det går ej att dividera något med 0.", "Ok");
                        return;
                    }
                    sumTemp /= operand;
                    break;
            }
        }
    }
}
