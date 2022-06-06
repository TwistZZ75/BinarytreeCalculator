using System;
using System.Collections.Generic;

namespace RPN
{
    class Program
    {
        static void Main(string[] source_strings)
        {
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
                        source_string = remove_plus(source_string);
                        source_string = diz_calc(source_string);
                        if (diz_calc(source_string) == "")
                        {
                            source_string = "0";
                            Console.WriteLine(source_string);
                            Console.WriteLine("Возможно вывести результивный ноль");
                        }
                        else
                        {
                            Console.WriteLine(source_string);
                            Console.WriteLine("Невозможно вывести результивный ноль");
                        }
                        break;
                    default:
                        break;
                }
            } while (x != '1');
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
            for (int i = 0; i < perms.Count; i++)
            {
                for (int j = 0; j < perms.Count; j++)
                {
                    if (perms[j].Contains("-" + perms[i]))
                    {
                        if (i > j)
                        {
                            perms.RemoveAt(i);
                            perms.RemoveAt(j);
                        }
                        else
                        {
                            perms.RemoveAt(j);
                            perms.RemoveAt(i);
                        }
                    }
                    if (i >= perms.Count)
                    {
                        break;
                    }
                    if (j >= perms.Count)
                    {
                        continue;
                    }
                    if (perms[i].Contains(perms[j]) && i != j && perms[i] != perms[j])
                    {
                        if (i > j)
                        {
                            perms.RemoveAt(i);
                            perms.RemoveAt(j);
                        }
                        else
                        {
                            perms.RemoveAt(j);
                            perms.RemoveAt(i);
                        }
                    }
                }
            }
            for (int i = 0; i < perms.Count; i++)
            {
                newstr += perms[i];
            }
            return newstr;
        }
        static string remove_plus(string source_string)
        {
            string[] symb = source_string.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries); //разбиваем строку по пробелам
            string newstr = string.Empty;
            for (int i = 0; i < symb.Length; i++)
            {
                if (symb[i] == "+")
                {
                    symb[i] = " ";
                }
                newstr += symb[i];
            }
            return newstr;
        }
    }
}