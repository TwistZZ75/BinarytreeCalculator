using System;
using System.Collections.Generic;

namespace RPN
{
    public partial class binarytree
    {
        public string data { get; private set; }//?-обнуляемый тип данных(переменная может принимать значение null)
                                                //get set - свойства экземпляра класса(что с ними можно делать)
        public binarytree right { get; private set; }
        public binarytree left { get; private set; }

        public binarytree CreateNode(string value, binarytree left, binarytree right)
        {
            var node = new binarytree();
            node.data = value;
            node.left = null;
            node.right = null;
            return node;
        }

        public binarytree add(string sourse_string)
        {
            string alphabet = "ABCDEFGabcdefg";
            string[] symb = sourse_string.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries); //разбиваем строку по пробелам
            Stack<binarytree> st = new Stack<binarytree>();
            string num = string.Empty;
            binarytree root = null;
            for (int i = 0; i < symb.Length; i++)
            {
                if (alphabet.Contains(symb[i]))
                {
                    string x = symb[i];
                    switch(x)
                    {
                        case "A":
                            num = "1";
                            break;
                        case "a":
                            num = "1";
                            break;
                        case "b":
                            num = "2";
                            break;
                        case "B":
                            num = "2";
                            break;
                        case "C":
                            num = "3";
                            break;
                        case "c":
                            num = "3";
                            break;
                        case "D":
                            num = "4";
                            break;
                        case "d":
                            num = "4";
                            break;
                        case "E":
                            num = "5";
                            break;
                        case "e":
                            num = "5";
                            break;
                        case "F":
                            num = "6";
                            break;
                        case "f":
                            num = "6";
                            break;
                    }
                    root = CreateNode(num, null, null);
                    st.Push(root);
                }
                else
                {
                    if (symb[i] == "!")
                    {
                        binarytree unar_minus;
                        unar_minus = st.Pop();
                        unar_minus.data = unar_minus.data.Insert(0, "-");
                        st.Push(unar_minus);
                    }
                    else
                    {
                        root = CreateNode(symb[i], null, null);
                        binarytree l, r;
                        l = st.Pop();
                        r = st.Pop();
                        root.right = r;
                        root.left = l;
                        st.Push(root);
                    }
                }
            }
            root = st.Pop();
            return root;
        }

        public int calculate(binarytree root)
        {
            int ans = 0;
            Stack<string> st = new Stack<string>();
            if (root != null)
            {
                calculate(root.left);
                calculate(root.right);

                if (root.data == "+")
                {
                    ans = Convert.ToInt32((root.left).data) + Convert.ToInt32((root.right).data);
                    root.data = Convert.ToString(ans);
                    root.left = null;
                    root.right = null;
                }
            }
            return ans;
        }
    }


    class Program
    {

        static void Main(string[] source_strings)
        {
            binarytree str = new binarytree();
            binarytree root = null;
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
                        Console.WriteLine("Введите выражение польской записью:");//"!" - унарный минус
                        source_string = Console.ReadLine();
                        source_string = Usual_to_Polska(source_string, x);
                        root = str.add(source_string);
                        Console.WriteLine();
                        if (str.calculate(root) != 0)
                        {
                            Console.WriteLine("Невозможно вывести резулятивный ноль");
                        }
                        else 
                        {
                            Console.WriteLine("Возможно вывести резулятивный ноль");
                        }
                        Console.WriteLine();
                        break;
                    case '2':
                        Console.WriteLine("Введите выражение обычной записью:");//"!" - унарный минус
                        source_string = Console.ReadLine();
                        source_string = Usual_to_Polska(source_string, x);
                        root = str.add(source_string);
                        Console.WriteLine();
                        if (str.calculate(root) != 0)
                        {
                            Console.WriteLine("Невозможно вывести резулятивный ноль");
                        }
                        else
                        {
                            Console.WriteLine("Возможно вывести резулятивный ноль");
                        }
                        Console.WriteLine();
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

                    if (symb[i] == "!" && newStr != string.Empty && x == '1')
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
                case "!":
                    return 4;
                case "+":
                    return 1;
                default:
                    return 4;
            }
        }
    }
}
