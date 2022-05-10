using System;
using System.Collections.Generic;

namespace RPN
{
    public partial class logic
    {

        public string add(string sourse_string)
        {
            Console.WriteLine(sourse_string);
            string alphabet = "ABCDEFGabcdefg";
            string[] symb = sourse_string.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries); //разбиваем строку по пробелам
            Stack<string> st = new Stack<string>();
            double num;
            string root = null;
            for (int i = 0; i < symb.Length; i++)
            {
                if (alphabet.Contains(symb[i]))
                {
                    root = symb[i];
                    st.Push(root);
                }
                else
                {
                    if (symb[i] == "~")
                    {
                        string unar_minus;
                        unar_minus = st.Pop();
                        unar_minus = unar_minus.Insert(0, "-");
                        st.Push(unar_minus);
                    }
                    else
                    {
                        root = st.Pop() + " + " + st.Pop();
                        st.Push(root);
                    }
                }
            }
            root = st.Pop();
            return root;
        }

        public string calculate(string root)
        {
            return root;
        }
    }
    class Program
    {

        static void Main(string[] source_strings)
        {
            logic str = new logic();
            string root = null;
            char x;
            string source_string;//исходная строка
            do
            {
                Console.WriteLine("0.Выход");
                Console.WriteLine("1.Польская запись");
                Console.WriteLine("2.Обычная запись, преобразуемая в польскую");
                x = Convert.ToChar(Console.ReadLine());
                switch (x)
                {
                    case '0':
                        break;
                    case '1':
                        Console.WriteLine("Введите выражение польской записью:");//"~" - унарный минус
                        source_string = Console.ReadLine();
                        source_string = Usual_to_Polska(source_string, x);
                        Console.WriteLine();
                        Console.WriteLine();
                        break;
                    case '2':
                        Console.WriteLine("Введите выражение обычной записью:");//"~" - унарный минус
                        source_string = Console.ReadLine();
                        source_string = Usual_to_Polska(source_string, x);
                        root = str.add(source_string);
                        Console.WriteLine();
                        Console.WriteLine(root);
                        break;
                    default:
                        break;
                }
            } while (x != '0');
        }

        static string Usual_to_Polska(string source_string, int x)
        {
            Stack<string> stz = new Stack<string>();//стек для знаков
            string[] symb = source_string.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries); //разбиваем строку по пробелам
            string newStr = string.Empty;//присваиваем строке значение пустой (аналог int x = 0;), чтобы отладчик не ругался на не присвоенное значение
            string alphabet = "ABCDEFGabcdefg";
            for (int i = 0; i < symb.Length; i++)
            {
                if (alphabet.Contains(symb[i])) //если число, помещаем в строку
                    newStr += symb[i] + " ";
                else
                {
                        if (stz.Count > 0)
                        {
                            if (Priority(symb[i]) <= Priority(stz.Peek())) //если приоритет знака в строке ниже или равен приоритету знака на вершине стека
                                                                           //тогда записываем знак с вершины стека в строку
                            {
                                newStr += stz.Pop() + " ";
                            }
                        }
                        stz.Push(symb[i]);//в противном случае сохраняем знак из строки в стек

                        if (symb[i] == "~" && newStr != string.Empty && x == '1')
                            newStr += stz.Pop() + " ";
                }
            }

            while (stz.Count > 0)
                newStr += stz.Pop() + " ";//запись знаков из стека в строку(они выстроены по приоритету в итоге)
            return newStr;
        }


        static public int Priority(string s)
        {
            switch (s)
            {
                case "~":
                    return 4;
                case "+":
                    return 1;
                default:
                    return 4;
            }
        }
    }
}
