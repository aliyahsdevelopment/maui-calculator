using System.Diagnostics;
using System.Globalization;

namespace Calculator
{
    public partial class MainPage : ContentPage
    {
        double sumTemp = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Set minimum height and width to stop the window from getting TOO small where the text is unreadable.
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

            EntryCalculation.Text += op;
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
                input = input.Replace("x", "*");
                input = input.Replace(",", ".");
                input = input.Replace(" ", "");

                double result = CalculateInput(input);

                sumTemp = result;
                ResultLabel.Text = sumTemp.ToString();
                EntryCalculation.Text = sumTemp.ToString();

                Debug.WriteLine($"Result: {sumTemp}");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Invalid input!", "Ok");
                Debug.WriteLine($"Calculation Error: {ex.Message}");
            }
        }

        private double CalculateInput(string expression)
        {
            List<string> numbers = [];
            List<char> operands = [];

            string currentNumber = "";
            foreach (char ch in expression)
            {
                if (char.IsDigit(ch) || ch == '.')
                {
                    currentNumber += ch;
                }
                else if (ch == '+' || ch == '-' || ch == '%' || ch == '^' || ch == '*' || ch == '/')
                {
                    if (!string.IsNullOrEmpty(currentNumber)) // Reset the currentNumber value to not make it keep adding up.
                    {
                        numbers.Add(currentNumber);
                        currentNumber = "";
                    }
                    operands.Add(ch);
                }
                else if (!char.IsWhiteSpace(ch))
                {
                    DisplayAlert("Error", $"Invalid character in expression: \"{ch}\"", "Ok");
                    return 0;
                }
            }

            if (!string.IsNullOrEmpty(currentNumber))
            {
                numbers.Add(currentNumber);
            }
            double result = double.Parse(numbers[0], CultureInfo.InvariantCulture); // https://stackoverflow.com/questions/1354924/how-do-i-parse-a-string-with-a-decimal-point-to-a-double
            for (int i = 0; i < operands.Count; i++)
            {
                double nextNumber = double.Parse(numbers[i + 1]);
                char op = operands[i];
                Debug.WriteLine($"{result} {op} {nextNumber}");

                result = ApplyOperation(result, nextNumber, op);
            }

            return result;
        }

        private double ApplyOperation(double a, double b, char op)
        {
            switch (op)
            {
                case '+': return a + b;
                case '-': return a - b;
                case '%': return a % b;
                case '^': return Math.Pow(a, b);
                case '*': return a * b;
                case '/':
                    if (a != 0 && b != 0) return a / b;
                    else
                    {
                        DisplayAlert("Error", "Unable to divide by zero.", "Ok");
                        return 0;
                    }
                default:
                    return 0;
            }
        }
    }
}
