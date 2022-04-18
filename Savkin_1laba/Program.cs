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

        int level = 0;
        int count_left = 1;
        int count_right = 1;
         
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
            Console.WriteLine(sourse_string);
            string[] symb = sourse_string.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries); //разбиваем строку по пробелам
            Stack<binarytree> st = new Stack<binarytree>();
            double num;
            binarytree root = null;
            for (int i = 0; i < symb.Length; i++)
            {
                bool isNum = double.TryParse(symb[i], out num);
                if (isNum)
                {
                    root = CreateNode(symb[i], null, null);
                    st.Push(root);
                }
                else
                {
                    if (symb[i] == "~")
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
            print_tree(root);
            return root;
        }

        public int calculate(binarytree root)
        {
            int ans = 0;
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
                else if (root.data == "*")
                {
                    ans = Convert.ToInt32((root.left).data) * Convert.ToInt32((root.right).data);
                    root.data = Convert.ToString(ans);
                    root.left = null;
                    root.right = null;
                }
                else if (root.data == "-")
                {
                    ans = Convert.ToInt32((root.right).data) - Convert.ToInt32((root.left).data);
                    root.data = Convert.ToString(ans);
                    root.left = null;
                    root.right = null;
                }
                else if (root.data == "/")
                {
                    if (Convert.ToInt32((root.left).data) != 0)
                    {
                        ans = Convert.ToInt32((root.right).data) / Convert.ToInt32((root.left).data);
                        root.data = Convert.ToString(ans);
                        root.left = null;
                        root.right = null;
                    }
                    else
                    {
                        Console.WriteLine("Ошибка. Деление на ноль");
                    }
                }
            }
            return ans;
        }

        public void print_tree(binarytree root)
        {
            binarytree last_root = null;
            if (root != null)
            {
                if (level == 0)
                {
                    Console.SetCursorPosition(Console.WindowWidth / 2 + level, Console.WindowHeight / 2 + level);
                    Console.WriteLine(root.data);
                    level++;
                }
                if (root.left != null && root.right != null)
                {
                    if (level == 1)
                    {
                        Console.SetCursorPosition(Console.WindowWidth / 2 - 4, Console.WindowHeight / 2 + level);
                        Console.WriteLine(root.left.data);
                        Console.SetCursorPosition(Console.WindowWidth / 2 + 4, Console.WindowHeight / 2 + level);
                        Console.WriteLine(root.right.data);
                    }
                    if (level == 2)
                    {
                        Console.SetCursorPosition(Console.WindowWidth / 2 - 6, Console.WindowHeight / 2 + level);
                        Console.WriteLine(root.left.data);
                        Console.SetCursorPosition(Console.WindowWidth / 2 - 2, Console.WindowHeight / 2 + level);
                        Console.WriteLine(root.right.data);
                    }
                    if (level == 3)
                    {
                        Console.SetCursorPosition(Console.WindowWidth / 2 - 8, Console.WindowHeight / 2 + level);
                        Console.WriteLine(root.left.data);
                        Console.SetCursorPosition(Console.WindowWidth / 2 - 4, Console.WindowHeight / 2 + level);
                        Console.WriteLine(root.right.data);
                    }
                    if (root.left.left != null || root.left.right != null || root.right.left != null || root.right.right != null)
                    {
                        level++;
                        print_tree(root.left);
                        print_tree(root.right);
                    }
                }
            }
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
                        Console.WriteLine("Введите выражение польской записью (12 ~ 4 + 5 *):");//"~" - унарный минус
                        source_string = Console.ReadLine();
                        source_string = Usual_to_Polska(source_string, x);
                        root = str.add(source_string);
                        Console.WriteLine(str.calculate(root));
                        break;
                    case '2':
                        Console.WriteLine("Введите выражение обычной записью (~ 12 + 4 * 5):");//"~" - унарный минус
                        source_string = Console.ReadLine();
                        source_string = Usual_to_Polska(source_string, x);
                        root = str.add(source_string);
                        Console.WriteLine(str.calculate(root));
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
                    return -1;
                case "+": 
                    return -4;
                case "-": 
                    return -3;
                case "*": 
                    return -2;
                case "/": 
                    return -2;
                default: 
                    return -1;
            }
        }
    }
}