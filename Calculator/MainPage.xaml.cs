using System.Diagnostics;

namespace Calculator
{
    public partial class MainPage : ContentPage
    {
        private double sumTemp = 0;
        private double operand = 0;
        private string operation = "";
        private bool firstCalculation = true;

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

        private void CommaButton(object sender, EventArgs e)
        {
            
            //EntryCalculation.Text += ",";
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
                input = input.Replace('.', ',');
                double result = Evaluate(input);

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

        private double Evaluate(string expression)
        {
            List<double> numbers = new List<double>();
            List<char> operators = new List<char>();

            string currentNumber = "";
            foreach (char c in expression)
            {
                if (char.IsDigit(c) || c == ',')
                {
                    currentNumber += c;
                }
                else if ("+-*/".Contains(c))
                {
                    numbers.Add(double.Parse(currentNumber));
                    currentNumber = "";
                    operators.Add(c);
                }
            }
            numbers.Add(double.Parse(currentNumber));

            // Go by math rules and handle the * and / first.
            for (int i = 0; i < operators.Count; i++)
            {
                if (operators[i] == '*' || operators[i] == '/')
                {
                    double result = ApplyOperation(numbers[i], numbers[i + 1], operators[i]);
                    numbers[i] = result;
                    numbers.RemoveAt(i + 1);
                    operators.RemoveAt(i);
                    i--;
                }
            }

            // Go by math rules and handle the + and - lastly.
            double finalResult = numbers[0];
            for (int i = 0; i < operators.Count; i++)
            {
                finalResult = ApplyOperation(finalResult, numbers[i + 1], operators[i]);
            }

            return finalResult;
        }
        private double ApplyOperation(double a, double b, char op)
        {
            if (op == '+') return a + b;
            if (op == '-') return a - b;
            if (op == '*') return a * b;
            if (op == '/')
            {
                if (b != 0)
                {
                    return a / b;
                }
                else
                {
                    DisplayAlert("Error", "Unable to divide a number by zero.", "Ok");
                }
            }
            return 0;
        }

    }
}