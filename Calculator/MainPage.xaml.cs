using System;
using System.Collections.Generic;
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

            int height = 675;
            int width = 475;

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

                input = input.Replace("X", "*");  // Convert 'X' to '*' for multiplication
                input = input.Replace('.', ',');  // Convert '.' to ',' if needed

                double result = CalculateResult(input);

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

        private double CalculateResult(string expression)
        {
            List<double> numbers = new List<double>();
            List<char> operators = new List<char>();

            string currentNumber = "";
            // Parse the expression into numbers and operators
            foreach (char c in expression)
            {
                if (char.IsDigit(c) || c == ',')  // Handle digits and decimal points
                {
                    currentNumber += c;
                }
                else if ("+-*/".Contains(c))  // Handle operators
                {
                    numbers.Add(double.Parse(currentNumber));
                    currentNumber = "";
                    operators.Add(c);
                }
            }
            numbers.Add(double.Parse(currentNumber)); // Add the last number

            // First pass: handle multiplication and division (higher precedence)
            for (int i = 0; i < operators.Count; i++)
            {
                if (operators[i] == '*' || operators[i] == '/')
                {
                    double result = ApplyOperation(numbers[i], numbers[i + 1], operators[i]);
                    numbers[i] = result;
                    numbers.RemoveAt(i + 1);
                    operators.RemoveAt(i);
                    i--;  // Recheck the modified list at the same index
                }
            }

            // Second pass: handle addition and subtraction (lower precedence)
            double finalResult = numbers[0];
            for (int i = 0; i < operators.Count; i++)
            {
                finalResult = ApplyOperation(finalResult, numbers[i + 1], operators[i]);
            }

            return finalResult;
        }

        private double ApplyOperation(double a, double b, char op)
        {
            switch (op)
            {
                case '+':
                    return a + b;
                case '-':
                    return a - b;
                case '*':
                    return a * b;
                case '/':
                    if (b != 0)
                    {
                        return a / b;
                    }
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
