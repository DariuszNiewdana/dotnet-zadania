using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Zadanie2
{
    public partial class MainWindow : Window
    {
        private string currentOperation = string.Empty;
        private double firstOperand;
        private double secondOperand;
        private bool isEnteringSecondOperand;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnDigitClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                string digit = button.Content.ToString();
                if (Display.Text == "0" || isEnteringSecondOperand)
                {
                    Display.Text = digit;
                    isEnteringSecondOperand = false;
                }
                else
                {
                    Display.Text += digit;
                }
            }
        }

        private void OnClearClick(object sender, RoutedEventArgs e)
        {
            ResetCalculator();
        }

        private void ResetCalculator()
        {
            Display.Text = "0";
            PreviousOperation.Text = string.Empty;
            currentOperation = string.Empty;
            isEnteringSecondOperand = false;
        }

        private void OnOperationClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                try
                {
                    // Zamienia przecinek na kropkę dla obsługi międzynarodowego formatu liczbowego
                    string input = Display.Text.Replace(',', '.');
                    firstOperand = double.Parse(input, System.Globalization.CultureInfo.InvariantCulture);
                    currentOperation = button.Content.ToString();
                    PreviousOperation.Text = $"{firstOperand} {currentOperation}";
                    isEnteringSecondOperand = true;
                }
                catch (FormatException)
                {
                    MessageBox.Show("Wprowadzono nieprawidłowy format liczby. Proszę wprowadzić liczbę jeszcze raz.");
                    ResetCalculator();
                }
                catch (OverflowException)
                {
                    MessageBox.Show("Wprowadzona liczba jest za duża lub za mała.");
                    ResetCalculator();
                }
            }
        }


        private void OnEqualsClick(object sender, RoutedEventArgs e)
        {
            secondOperand = double.Parse(Display.Text);
            double result = CalculateResult();
            Display.Text = result.ToString();
            PreviousOperation.Text = $"{firstOperand} {currentOperation} {secondOperand} = {result}";
            isEnteringSecondOperand = true;
        }

        private double CalculateResult()
        {
            return currentOperation switch
            {
                "+" => firstOperand + secondOperand,
                "-" => firstOperand - secondOperand,
                "*" => firstOperand * secondOperand,
                "/" => firstOperand / secondOperand,
                "^" => Math.Pow(firstOperand, secondOperand),
                "mod" => firstOperand % secondOperand,
                "%" => (firstOperand / 100) * secondOperand,
                _ => 0
            };
        }

        private void OnFunctionClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button button)
            {
                double operand = double.Parse(Display.Text);
                double result = PerformFunction(button.Content.ToString(), operand);
                Display.Text = result.ToString();
                PreviousOperation.Text = $"{button.Content}({operand}) = {result}";
            }
        }

        private double PerformFunction(string function, double operand)
        {
            return function switch
            {
                "√" => Math.Sqrt(operand),
                "1/x" => 1 / operand,
                "!" => Factorial(operand),
                "log" => Math.Log10(operand),
                "ln" => Math.Log(operand),
                "log2" => Math.Log2(operand),
                "⌊x⌋" => Math.Floor(operand),
                "⌈x⌉" => Math.Ceiling(operand),
                _ => 0
            };
        }

        private double Factorial(double number)
        {
            if (number < 0)
                throw new ArgumentException("Liczba nie może być ujemna.");
            if (number != (int)number)
                throw new ArgumentException("Silnia zdefiniowana tylko dla liczb całkowitych.");
            return Enumerable.Range(1, (int)number).Aggregate(1, (product, item) => product * item);
        }
    }
}
