using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Calculator_CSharp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        String text = "";
        Char[] delimiters = { '-', '+', 'x', '/', '(', ')' };//Разделителя выражения
        String delimeters_operation = "-+x/";
        String first_operation = "+x/)";// Чтобы первым в строке не был знак

        public MainWindow()
        {
            InitializeComponent();
        }

        //Метод нажатия кнопок
        private void OnClick(Object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource == Btn_Result) //Если нажата кнопка равно
            {
                try
                {
                    Txt_Result.Content = "= " + Expression.Calculate(text); //Запускаем вычисление и Отображаем результат в Lable 
                }
                catch (Exception ex) { }
            }
            else
                if (e.OriginalSource == Btn_AC) //Если нажата кнопка отчистки
            {
                if (text.Length == 0) { text = ""; }
                else
                {
                    text = text.Substring(0, text.Length - 1); //Удаляем один знак
                    Txt_Operation.Content = text;
                }
            }
            else
                if (e.OriginalSource == Btn_Dot) //если начата точка
            {
                String[] s = text.Split(delimiters);
                if (!(s[s.Length - 1].Contains(","))) { text = text + ((Button)sender).Content; Txt_Operation.Content = text; } //проверяем чтобы нельзя было поставить несколько точек одновременно
            }
            else
            { //Нажаты все остальные кнопки
                try
                {
                    if ((delimeters_operation.Contains(((Button)sender).Content.ToString())) && delimeters_operation.Contains(text.Substring(text.Length - 1))) { }  //Проверяем чтобы нельзя было поставить несколько операций одновременно
                    else
                    {
                        if ((text == "") && (first_operation.Contains(((Button)sender).Content.ToString()))) { }  //Чтобы нельзя было поставить первым операцию
                        else
                        {
                            text = text + ((Button)sender).Content.ToString();
                            Txt_Operation.Content = text;
                        }
                    }
                }
                catch (Exception ex) { }

            }
            ((Button)sender).Focusable = false;
        }

        //нажатие клавиш на клавиатуры
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.NumPad0) { Btn_0.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)); }
            if (e.Key == Key.NumPad1) { Btn_1.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)); }
            if (e.Key == Key.NumPad2) { Btn_2.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)); }
            if (e.Key == Key.NumPad3) { Btn_3.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)); }
            if (e.Key == Key.NumPad4) { Btn_4.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)); }
            if (e.Key == Key.NumPad5) { Btn_5.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)); }
            if (e.Key == Key.NumPad6) { Btn_6.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)); }
            if (e.Key == Key.NumPad7) { Btn_7.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)); }
            if (e.Key == Key.NumPad8) { Btn_8.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)); }
            if (e.Key == Key.NumPad9) { Btn_9.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)); }
            if (e.Key == Key.Multiply) { Btn_Multiply.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)); }
            if (e.Key == Key.Add) { Btn_Plus.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)); }
            if (e.Key == Key.Divide) { Btn_Devide.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)); }
            if (e.Key == Key.Subtract) { Btn_Minus.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)); }
            if (e.Key == Key.Decimal) { Btn_Dot.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)); }
            if (e.Key == Key.Enter) { Btn_Result.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)); }
            if (e.Key == Key.Back) { Btn_AC.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)); }
            if (e.Key == Key.D9) { Btn_Left.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)); }
            if (e.Key == Key.D0) { Btn_Right.RaiseEvent(new RoutedEventArgs(Button.ClickEvent)); }

        }
    }


    //Алгоритм Обратной Польской Нотации
    public class Expression
    {
        static public double Calculate(string input)
        {
            string output = GetExpression(input); //Преобразовываем выражение в постфиксную запись
            double result = Counting(output); //Решаем полученное выражение
            return result; //Возвращаем результат
        }

        static private string GetExpression(string input)
        {
            string output = string.Empty; //Строка для хранения выражения
            Stack<char> operStack = new Stack<char>(); //Стек для хранения операторов

            for (int i = 0; i < input.Length; i++) //Для каждого символа в входной строке
            {
                //Разделители пропускаем
                if (IsDelimeter(input[i]))
                    continue; //Переходим к следующему символу

                //Если символ - цифра, то считываем все число
                if (Char.IsDigit(input[i])) //Если цифра
                {
                    //Читаем до разделителя или оператора, что бы получить число
                    while (!IsDelimeter(input[i]) && !IsOperator(input[i]))
                    {
                        output += input[i]; //Добавляем каждую цифру числа к нашей строке
                        i++; //Переходим к следующему символу

                        if (i == input.Length) break; //Если символ - последний, то выходим из цикла
                    }

                    output += " "; //Дописываем после числа пробел в строку с выражением
                    i--; //Возвращаемся на один символ назад, к символу перед разделителем
                }

                //Если символ - оператор
                if (IsOperator(input[i])) //Если оператор
                {
                    if (input[i] == '(') //Если символ - открывающая скобка
                        operStack.Push(input[i]); //Записываем её в стек
                    else if (input[i] == ')') //Если символ - закрывающая скобка
                    {
                        //Выписываем все операторы до открывающей скобки в строку
                        char s = operStack.Pop();

                        while (s != '(')
                        {
                            output += s.ToString() + ' ';
                            s = operStack.Pop();
                        }
                    }
                    else //Если любой другой оператор
                    {
                        if (operStack.Count > 0) //Если в стеке есть элементы
                            if (GetPriority(input[i]) <= GetPriority(operStack.Peek())) //И если приоритет нашего оператора меньше или равен приоритету оператора на вершине стека
                                output += operStack.Pop().ToString() + " "; //То добавляем последний оператор из стека в строку с выражением

                        operStack.Push(char.Parse(input[i].ToString())); //Если стек пуст, или же приоритет оператора выше - добавляем операторов на вершину стека

                    }
                }
            }

            //Когда прошли по всем символам, выкидываем из стека все оставшиеся там операторы в строку
            while (operStack.Count > 0)
                output += operStack.Pop() + " ";

            return output; //Возвращаем выражение в постфиксной записи
        }

        static private double Counting(string input)
        {
            double result = 0; //Результат
            Stack<double> temp = new Stack<double>(); //Dhtvtyysq стек для решения

            for (int i = 0; i < input.Length; i++) //Для каждого символа в строке
            {
                //Если символ - цифра, то читаем все число и записываем на вершину стека
                if (Char.IsDigit(input[i]))
                {
                    string a = string.Empty;

                    while (!IsDelimeter(input[i]) && !IsOperator(input[i])) //Пока не разделитель
                    {
                        a += input[i]; //Добавляем
                        i++;
                        if (i == input.Length) break;
                    }

                    temp.Push(Double.Parse(a)); //Записываем в стек
                    i--;
                }
                else if (IsOperator(input[i])) //Если символ - оператор
                {
                    //Берем два последних значения из стека
                    double a = temp.Pop();
                    double b = temp.Pop();

                    switch (input[i]) //И производим над ними действие, согласно оператору
                    {
                        case '+': result = b + a; break;
                        case '-': result = b - a; break;
                        case 'x': result = b * a; break;
                        case '/': result = b / a; break;
                            //case '^': result = double.Parse(Math.Pow(double.Parse(b.ToString()), double.Parse(a.ToString())).ToString()); break;
                    }
                    temp.Push(result); //Результат вычисления записываем обратно в стек
                }
            }
            return temp.Peek(); //Забираем результат всех вычислений из стека и возвращаем его
        }

        //Метод возвращает приоритет оператора
        static private byte GetPriority(char s)
        {
            switch (s)
            {
                case '(': return 0;
                case ')': return 1;
                case '+': return 2;
                case '-': return 3;
                case 'x': return 4;
                case '/': return 4;
                case '^': return 5;
                default: return 6;
            }
        }

        //Метод возвращает true, если проверяемый символ - оператор
        static private bool IsOperator(char с)
        {
            if (("+-/x^()".IndexOf(с) != -1))
                return true;
            return false;
        }

        //Метод возвращает true, если проверяемый символ - разделитель ("пробел" или "равно")
        static private bool IsDelimeter(char c)
        {
            if ((" =".IndexOf(c) != -1))
                return true;
            return false;
        }

    }
}
