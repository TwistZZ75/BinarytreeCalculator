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
        int item;

        public binarytree CreateNode(string value, binarytree left, binarytree right)
        {
            var node = new binarytree();
            node.data = value;
            node.left = null;
            node.right = null;
            return node;
        }

        public binarytree add(string source_string)
        {
            Console.WriteLine(source_string);
            string[] symb = source_string.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries); //разбиваем строку по пробелам
            Stack<binarytree> st = new Stack<binarytree>();
            string alphabet = "ABCDEFGabcdefg";
            binarytree root = null;
            for (int i = 0; i < symb.Length; i++)
            {
                if (alphabet.Contains(symb[i]))
                {
                    root = CreateNode(symb[i], null, null);
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
            print_tree(root);
            return root;
        }

        public string calculate(binarytree root)
        {
            string ans = string.Empty;
            if (root != null)
            {
                calculate(root.left);
                calculate(root.right);

                if (root.data == "+")
                {
                    ans = (root.left).data + " " + (root.right).data;
                    root.data = Convert.ToString(ans);
                    root.left = null;
                    root.right = null;
                }
            }
            return ans;
        }
        class rootInfo
        {
            public binarytree Node;
            public string Text;
            public int StartPos;
            public int Size { get { return Text.Length; } }
            public int EndPos { get { return StartPos + Size; } set { StartPos = value - Size; } }
            public rootInfo Parent, Left, Right;
        }
        public void print_tree(binarytree root, string textFormat = "0", int spacing = 1, int topMargin = 2, int leftMargin = 2)
        {
            if (root == null)
                return;
            int rootTop = Console.CursorTop + topMargin;
            var last = new List<rootInfo>();
            var next = root;
            for (int level = 0; next != null; level++)
            {
                var item = new rootInfo { Node = next, Text = next.data };
                if (level < last.Count)
                {
                    item.StartPos = last[level].EndPos + spacing;
                    last[level] = item;
                }
                else
                {
                    item.StartPos = leftMargin;
                    last.Add(item);
                }
                if (level > 0)
                {
                    item.Parent = last[level - 1];
                    if (next == item.Parent.Node.left)
                    {
                        item.Parent.Left = item;
                        item.EndPos = Math.Max(item.EndPos, item.Parent.StartPos - 1);
                    }
                    else
                    {
                        item.Parent.Right = item;
                        item.StartPos = Math.Max(item.StartPos, item.Parent.EndPos + 1);
                    }
                }
                next = next.left ?? next.right;
                for (; next == null; item = item.Parent)
                {
                    int top = rootTop + 2 * level;
                    Print(item.Text, top, item.StartPos);
                    if (item.Left != null)
                    {
                        Print("/", top + 1, item.Left.EndPos);
                        Print("_", top, item.Left.EndPos + 1, item.StartPos);
                    }
                    if (item.Right != null)
                    {
                        Print("_", top, item.EndPos, item.Right.StartPos - 1);
                        Print("\\", top + 1, item.Right.StartPos - 1);
                    }
                    if (--level < 0) break;
                    if (item == item.Parent.Left)
                    {
                        item.Parent.StartPos = item.EndPos + 1;
                        next = item.Parent.Node.right;
                    }
                    else
                    {
                        if (item.Parent.Left == null)
                            item.Parent.EndPos = item.StartPos - 1;
                        else
                            item.Parent.StartPos += (item.StartPos - 1 - item.Parent.EndPos) / 2;
                    }
                }
            }
            Console.SetCursorPosition(0, rootTop + 2 * last.Count - 1);
        }
        private static void Print(string s, int top, int left, int right = -1)
        {
            Console.SetCursorPosition(left, top);
            if (right < 0) right = left + s.Length;
            while (Console.CursorLeft < right) Console.Write(s);
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
                Console.WriteLine("1.Выход");
                Console.WriteLine("2.Ввести выражение");
                x = Convert.ToChar(Console.ReadLine());
                switch (x)
                {
                    case '1':
                        break;
                    case '2':
                        Console.WriteLine("Введите выражение: ");//"!" - унарный минус
                        source_string = Console.ReadLine();
                        source_string = Usual_to_Polska(source_string, x);
                        root = str.add(source_string);
                        source_string = str.calculate(root);
                        if (diz_calc(source_string) == "")
                        {
                            Console.WriteLine("Возможно вывести результивный ноль");
                        }
                        else
                        {
                            Console.WriteLine("Невозможно вывести результивный ноль");
                        }
                        break;
                    default:
                        break;
                }
            } while (x != '0');
        }

        static string diz_calc(string source_string)
        {
            string[] symb = source_string.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries); //разбиваем строку по пробелам
            List<string> perms = new List<string>();
            string newstr = string.Empty;
            for (int i = 0; i < symb.Length; i++)
            {
                perms.Add(symb[i]);
            }
            for (int i = 0; i < perms.Count - 1; i++)
            {
                int j = 0;
                do
                {
                    if (perms[i].Contains("-" + perms[j]))
                    {
                        perms.Remove(perms[i]);
                        perms.Remove(perms[j - 1]);
                        if (perms.Count == 0)
                        {
                            break;
                        }
                        j = 0;
                    }
                    if (perms[i].Contains(perms[j]) && perms[i] != perms[j])
                    {
                        perms.Remove(perms[i]);
                        perms.Remove(perms[j - 1]);
                        if (perms.Count == 0)
                        {
                            break;
                        }
                        j = 0;
                    }
                    else
                    {
                        j++;
                    }
                } while (j != perms.Count);
            }
            for (int i = 0; i < perms.Count; i++)
            {
                newstr += perms[i];
            }
            return newstr;
        }

        static string Usual_to_Polska(string source_string, int x)
        {
            Stack<string> stz = new Stack<string>();//стек для знаков
            string alphabet = "ABCDEFGabcdefg";
            string[] symb = source_string.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries); //разбиваем строку по пробелам
            string newStr = string.Empty;//присваиваем строке значение пустой (аналог int x = 0;), чтобы отладчик не ругался на не присвоенное значение
            string s = string.Empty;
            for (int i = 0; i < symb.Length; i++)
            {
                if (alphabet.Contains(symb[i])) //если число, помещаем в строку
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

                        if (symb[i] == "!" && newStr != string.Empty && x == '1')
                            newStr += stz.Pop() + " ";
                    }
                }
            }

            while (stz.Count > 0)
                newStr += stz.Pop() + " ";//запись знаков из стека в строку(они выстроены по приоритету в итоге)
            //Console.WriteLine(newStr);
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