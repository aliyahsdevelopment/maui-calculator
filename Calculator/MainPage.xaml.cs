using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Calculator
{
    public partial class MainPage : ContentPage
    {
        double sumTemp = 0;
        double operand = 0;
        string operation = "";
        bool firstCalculation = true;

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.Window.Height = 675;
            this.Window.Width = 475;

            this.Window.MinimumHeight = 450;
            this.Window.MinimumWidth = 300;
        }

        private void ClearButton_Clicked(object sender, EventArgs e)
        {
            EntryCalculation.Text = "0";
            ResultLabel.Text = "0";
            sumTemp = 0;
            operation = "";
        }

        private void NumberButton(object sender, EventArgs e)
        {
            Button button = (Button)sender;

            if (EntryCalculation.Text == "0")
            {
                EntryCalculation.Text = button.Text;
            }
            else
            {
                EntryCalculation.Text += button.Text;
            }

            if (firstCalculation)
            {
                if (double.TryParse(EntryCalculation.Text, out double sumTempNew))
                {
                    sumTemp = sumTempNew;
                }
            }
        }

        private void EraseButton(object sender, EventArgs e)
        {
            if (EntryCalculation.Text.Length > 0)
                EntryCalculation.Text = EntryCalculation.Text.Substring(0, EntryCalculation.Text.Length - 1);
        }

        private void CommaButton(object sender, EventArgs e)
        {
            EntryCalculation.Text += ",";
        }

        private void OperandButton(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            string op = button.Text;

            if (string.IsNullOrEmpty(EntryCalculation.Text))
            {
                return;
            }

            operation = button.Text;
            EntryCalculation.Text += op;
            firstCalculation = false;
        }

        private void EqualsButton(object sender, EventArgs e)
        {
            Calculate();
        }

        private async void Calculate()
        {
            try
            {
                string input = EntryCalculation.Text;
                if (string.IsNullOrWhiteSpace(input))
                {
                    await DisplayAlert("Error", "Invalid calculation format!", "Ok");
                    return;
                }

                input = input.Replace("X", "*");
                input = input.Replace(',', '.');

                double result = CalculateInput(input);

                sumTemp = result;
                ResultLabel.Text = sumTemp.ToString();
                EntryCalculation.Text = sumTemp.ToString();

                Debug.WriteLine($"Result: {sumTemp}");
                firstCalculation = true;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Invalid input!", "Ok");
                Debug.WriteLine($"Calculation Error: {ex.Message}");
            }
        }

        private double CalculateInput(string expression)
        {
            string[] calculationArray = Regex.Split(expression, @"(?<=[+\-*/])|(?=[+\-*/])");
            double result = double.Parse(calculationArray[0]);

            for (int i = 1; i < calculationArray.Length; i += 2)
            {
                char op = char.Parse(calculationArray[i]);
                double num = double.Parse(calculationArray[i + 1]);

                result = ApplyOperation(result, num, op);
            }

            return result;
        }

        private double ApplyOperation(double a, double b, char op)
        {
            switch (op)
            {
                case '+':  return a + b;
                case '-': return a - b;
                case '*':  return a * b;
                case '/':
                    if (a != 0 && b != 0) return a / b;
                    else
                    {
                        DisplayAlert("Error", "Unable to divide by zero.", "Ok");
                        return 0;
                    }
                default:
                    throw new InvalidOperationException($"Unsupported operator: {op}");
            }
        }
    }
}
