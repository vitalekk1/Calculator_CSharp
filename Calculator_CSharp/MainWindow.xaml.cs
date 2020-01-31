using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;


namespace Calculator_CSharp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        String text = "";
        Char[] delimiters = { '-', '+', 'x', '/', '(', ')' };
        String delimeters_operation = "-+x/";
        String first_operation = "+x/)";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnClick(Object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource == Btn_Result)
            {
                //try
                //{
                    Btn_Result.Content = "= " + Expression.calculateExpression(text);
               // }
               // catch (Exception ex) { }
            } else
                if (e.OriginalSource == Btn_AC) {
                if (text.Length == 0) { text = ""; }
                else
                {
                    text = text.Substring(0, text.Length - 1);
                    Txt_Operation.Content = text;
                }
            } else
                if (e.OriginalSource == Btn_Dot)
            {
                String[] s = text.Split(delimiters);
                if (!(s[s.Length - 1].Contains("."))) { text = text + ((Button)sender).Content; Txt_Operation.Content = text; }
            } else
            {
                try
                {
                    if ((delimeters_operation.Contains(((Button)sender).Content.ToString())) && delimeters_operation.Contains(text.Substring(text.Length - 1))) { } else
                    {
                        if ((text == "") && (first_operation.Contains(((Button)sender).Content.ToString()))) { }
                        else
                        {
                            text = text + ((Button)sender).Content.ToString();
                            Txt_Operation.Content = text;
                        }
                    }
                }
                catch (Exception ex) { }

            }
        }
    }



    public class Expression
    {

        public static Dictionary<String, Int32> MAIN_MATH_OPERATIONS;


        public Expression() {

            MAIN_MATH_OPERATIONS = new Dictionary<String, Int32>();
            MAIN_MATH_OPERATIONS.Add("x", 1);
            MAIN_MATH_OPERATIONS.Add("/", 1);
            MAIN_MATH_OPERATIONS.Add("+", 2);
            MAIN_MATH_OPERATIONS.Add("-", 2);
        }


        public static String sortingStation(String expression, Dictionary<String, Int32> operations, String leftBracket,
                                            String rightBracket)
        {
            if (expression == null || expression.Length == 0)
                throw new Exception("Expression isn't specified.");
            // Выходная строка, разбитая на "символы" - операции и операнды..
            List<String> out1 = new List<String>();
            // Стек операций.
            Stack<String> stack = new Stack<String>();

            // Удаление пробелов из выражения.
            expression = expression.Replace(" ", "");

            // Множество "символов", не являющихся операндами (операции и скобки).
            HashSet<String> operationSymbols = new HashSet<String>(operations.Keys);
            operationSymbols.Add(leftBracket);
            operationSymbols.Add(rightBracket);

            // Индекс, на котором закончился разбор строки на прошлой итерации.
            int index = 0;
            // Признак необходимости поиска следующего элемента.
            Boolean findNext = true;
            while (findNext)
            {
                int nextOperationIndex = expression.Length;
                String nextOperation = "";
                // Поиск следующего оператора или скобки.
                foreach (String operation in operationSymbols)
                {
                    int i = expression.IndexOf(operation, index);
                    if (i >= 0 && i < nextOperationIndex)
                    {
                        nextOperation = operation;
                        nextOperationIndex = i;
                    }
                }
                // Оператор не найден.
                if (nextOperationIndex == expression.Length)
                {
                    findNext = false;
                }
                else
                {
                    // Если оператору или скобке предшествует операнд, добавляем его в выходную строку.
                    if (index != nextOperationIndex)
                    {
                        out1.Add(expression.Substring(index, nextOperationIndex));
                    }
                    // Обработка операторов и скобок.
                    // Открывающая скобка.
                    if (nextOperation.Equals(leftBracket))
                    {
                        stack.Push(nextOperation);
                    }
                    // Закрывающая скобка.
                    else if (nextOperation.Equals(rightBracket))
                    {
                        while (!stack.Peek().Equals(leftBracket))
                        {
                            out1.Add(stack.Pop());
                            if (stack.Count == 0)
                            {
                                throw new Exception("Unmatched brackets");
                            }
                        }
                        stack.Pop();
                    }
                    // Операция.
                    else
                    {
                        while (stack.Count != 0 && !stack.Peek().Equals(leftBracket) &&
                                (operations[nextOperation] >= operations[stack.Peek()]))
                        {
                            out1.Add(stack.Pop());
                        }
                        stack.Push(nextOperation);
                    }
                    index = nextOperationIndex + nextOperation.Length;
                }
            }
            // Добавление в выходную строку операндов после последнего операнда.
            if (index != expression.Length)
            {
                out1.Add(expression.Substring(index));
            }
            // Пробразование выходного списка к выходной строке.
            while (stack.Count != 0)
            {
                out1.Add(stack.Pop());
            }
            StringBuilder result = new StringBuilder();
            if (out1.Count != 0)
                
                result.Append(out1[0]);
            while (out1.Count != 0)
                result.Append(" ").Append(out1[0]);

            return result.ToString();
        }


        public static String sortingStation(String expression, Dictionary<String, Int32> operations)
        {
            return sortingStation(expression, operations, "(", ")");
        }


        public static double calculateExpression(String expression)
        {
            int i = 0;
            String rpn = sortingStation(expression, MAIN_MATH_OPERATIONS);
            String[] tokenizer = rpn.Split(' ');
            Stack<Double> stack = new Stack<Double>();
            while (i < tokenizer.Length)
            {
                String token = tokenizer[i];
                // Операнд.
                
                if (!MAIN_MATH_OPERATIONS.Keys.Contains(token))
                {
                     stack.Push(Convert.ToDouble(token)); 
                }
                else
                {
                    double operand2 = stack.Pop();
                    double operand1 = stack.Count == 0 ? 0 : stack.Pop();
                    if (token.Equals("x"))
                    {
                        stack.Push(operand1 * operand2);
                    }
                    else if (token.Equals("/"))
                    {
                        stack.Push(operand1 / operand2);
                    }
                    else if (token.Equals("+"))
                    {
                        stack.Push(operand1 + operand2);
                    }
                    else if (token.Equals("-"))
                    {
                        stack.Push(operand1 - operand2);
                    }
                }
                i++;
            }
            if (stack.Count != 1)
                throw new Exception("Expression syntax error.");
            return stack.Pop();
        }

    } 
}
