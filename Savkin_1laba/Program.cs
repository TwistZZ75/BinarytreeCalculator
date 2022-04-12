using System;
using System.Collections.Generic;

namespace RPN
{
    class Program
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
                binarytree v = null;
                Console.WriteLine(sourse_string);
                string[] symb = sourse_string.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries); //разбиваем строку по пробелам
                for (int i = symb.Length - 1; i >= 0; i--)
                {
                    if (v == null)
                    {
                        v = CreateNode(symb[i], null, null);
                    }
                    else
                    {
                        v.insert(symb[i]);
                    }
                }
                return v;
            }

            public void insert(string symb)
            {
                double num;
                double value = 0; 
                bool isNum = double.TryParse(symb, out num);
                if (right != null || left != null)
                {
                    if (double.TryParse(right.data, out value) == false)
                    {
                        value = Priority(right.data);
                    }
                    else if (double.TryParse(right.data, out value) == true)
                    {
                        value = Convert.ToDouble(right.data);
                    }
                }
                if (num >= value)
                {
                    if (right == null)
                    {
                        right = CreateNode(symb, null, null);
                    }
                    else
                    {
                        right.insert(symb);
                    }
                }
                else
                {
                    if (left == null)
                    {
                        left = CreateNode(symb, null, null);
                    }
                    else
                    {
                        left.insert(symb);
                    }
                }
            }
        }
        static void Main(string[] source_strings)
        {
            binarytree str = new binarytree();
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
                        Console.WriteLine("Введите выражение польской записью (12 ~ 4 + 5 *):");//"~" - унарный минус
                        source_string = Console.ReadLine();
                        source_string = Usual_to_Polska(source_string, x);
                        str.add(source_string);
                        break;
                    case '2':
                        Console.WriteLine("Введите выражение обычной записью (~ 12 + 4 * 5):");//"~" - унарный минус
                        source_string = Console.ReadLine();
                        source_string = Usual_to_Polska(source_string, x);
                        str.add(source_string);
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
            string s = string.Empty;
            for (int i = 0; i < symb.Length; i++)
            {
                double num;
                bool isNum = double.TryParse(symb[i], out num);//проверяем текущий символ на то является он знаком или числом
                if (isNum) //если число, помещаем в строку
                    newStr += symb[i] + " ";
                else
                {
                    if (symb[i] == "(")
                    {
                        stz.Push(symb[i]);
                    }
                    else if (symb[i] == ")")
                    {
                        s = stz.Pop();
                        while (s != "(")
                        {
                            newStr += s + " ";
                            s = stz.Pop();
                        }
                    }
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
            }

            while (stz.Count > 0)
                  newStr += stz.Pop() + " ";//запись знаков из стека в строку(они выстроены по приоритету в итоге)
           // Console.WriteLine(newStr);
            return newStr;
        }


    static public int Priority(string s)
        {
            switch (s)
            {
                case "(":
                    return 0;
                case "~":
                    return 40;
                case "+": 
                    return 37;
                case "-": 
                    return 38;
                case "*": 
                    return 39;
                case "/": 
                    return 39;
                default: 
                    return 40;
            }
        }
        //static void Polska(string source_string)
        //{
        //    Stack<double> st = new Stack<double>();
        //    //пример выражения(обрати внимание на пробелы!!!):12 4 +
        //    string[] symb = source_string.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries); //разбиваем строку по пробелам
        //    foreach (var ch in symb)
        //    {
        //        double num;
        //        bool isNum = double.TryParse(ch, out num);//проверяем текущий символ на то является он знаком или числом
        //        if (isNum) //если число, помещаем в стек
        //            st.Push(num);
        //        else
        //        {
        //            double op2;
        //            if (st.Count > 0)
        //            {
        //                switch (ch)
        //                {
        //                    case "+":
        //                        st.Push(st.Pop() + st.Pop());
        //                        break;
        //                    case "*":
        //                        st.Push(st.Pop() * st.Pop());
        //                        break;
        //                    case "-":
        //                        op2 = st.Pop();//т.к. в вычитании важен порядок вычитания, то учитывая особенность стека,
        //                                       //мы из второго числа должны вычесть первое, поэтому первое сохраняем в переменную
        //                        st.Push(st.Pop() - op2);
        //                        break;
        //                    case "/":
        //                        op2 = st.Pop();//то же что и в вычитании
        //                        if (op2 != 0.0)
        //                            st.Push(st.Pop() / op2);
        //                        else
        //                            Console.WriteLine("Ошибка. Деление на ноль");
        //                        break;
        //                    case "~":
        //                        st.Push(st.Pop() * -1);//унарный минус
        //                        break;
        //                    default:
        //                        Console.WriteLine("Ошибка. Неизвестная команда");//если неизвестный символ выводится сообщение
        //                        break;
        //                }
        //            }
        //        }
        //    }

        //    Console.WriteLine("Результат: " + st.Pop());
        //}
    }
}