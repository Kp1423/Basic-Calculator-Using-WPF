using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<string> _operands = new List<string>();
        private List<string> _operators = new List<string>();
        bool IsResultClick = false;
        bool isOutputShow = false;
        public MainWindow()
        {
            InitializeComponent();
            isOutputShow = true;
        }

        private void Operand_Click(object sender, RoutedEventArgs e)
        {
            IsResultClick = false;
            string btnText = Convert.ToString((sender as Button).Name.Substring(3));
            string preText = tbInput.Text;
           
            tbInput.Text = (tbInput.Text == "0") ? btnText: Convert.ToString(preText + btnText);
        }

        private void Operator_Click(object sender, RoutedEventArgs e)
        {
            IsResultClick = false;
            string btnText = Convert.ToString((sender as Button).Content);
            string preText = tbInput.Text.Trim();
            tbInput.Text = (tbInput.Text == "0") ? "0" + btnText : Convert.ToString(preText + btnText);
        }

        private void Result_Click(object sender, RoutedEventArgs e)
        {
            if(tbInput.Text != tbOutput.Text && !IsResultClick)
            {
                tbInput.Text = tbOutput.Text;
                tbOutput.Text = "0";
                IsResultClick = true;
            }
           
        }
        
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            tbInput.Text = "0";
            tbOutput.Text = "0";
            _operators = new List<string>();
            _operands = new List<string>();
        }

        private void Calculation()
        {
            _operators = new List<string>();
            _operands = new List<string>();

            char[] charArr = tbInput.Text.ToCharArray();
            string operand = "";
            foreach (char c in charArr)
            {
                if (Char.IsDigit(c) || c == '.')
                {
                    operand += Convert.ToString(c);
                }
                else if (c == '+' || c == '-' || c == '*' || c == '/')
                {
                    _operands.Add(operand);
                    _operators.Add(Convert.ToString(c));
                    operand = "";
                }
                else
                {
                    _operands.Add(operand);
                    operand = "";
                }
            }

            if (operand != "" && (!operand.Contains('+') || !operand.Contains('-') || !operand.Contains('*') || !operand.Contains('/')))
            {
                _operands.Add(operand);
                operand = "";
            }


            if (_operands.Count > 1 && _operators.Count > 0)
            {
                double sum = 0;

                for (int i = 1, j = 0; i < _operands.Count && j < _operators.Count; i++, j++)
                {
                    double op1 = i == 1 ? Convert.ToDouble(_operands[i - 1]) : sum;
                    double op2 = Convert.ToDouble(_operands[i]);
                    string _oper = _operators[j];

                    switch (_oper)
                    {
                        case "+":
                            sum = op1 + op2;
                            break;
                        case "-":
                            sum = op1 - op2;
                            break;
                        case "*":
                            sum = op1 * op2;
                            break;
                        case "/":
                            sum = op1 / op2;
                            break;
                        default:
                            break;

                    }
                }


                tbOutput.Text = Convert.ToString(Math.Round(sum, 3));
            }
            else if (_operands.Count > 0 && _operators.Count == 0)
            {
                tbOutput.Text = Convert.ToString(_operands[0]);
            }
            else
            {
                _operators = new List<string>();
                _operands = new List<string>();
            }
        }

        private void tbInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (isOutputShow)
            {
                Calculation();
            }
        }

        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            tbInput.Text = (tbInput.Text.Length > 1) ? 
                tbInput.Text.Substring(0, tbInput.Text.Length - 1) : "0";
        }

        private void Point_Click(object sender, RoutedEventArgs e)
        {
            string preText = tbInput.Text;
            string lastDigit = preText.Split(new char[] { '+', '-', '*', '/' }).Last();
            if (lastDigit.Length > 0 && !lastDigit.Contains('.'))
            {
                tbInput.Text = Convert.ToString(preText + ".");
            }
        }
    }
}
