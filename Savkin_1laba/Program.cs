using System;
using System.Collections.Generic;

class Program
{
    static void Print_Carriage(int position_of_carriage)
    {
        Console.WriteLine();
        int count = 1;
        while (count < position_of_carriage)
        {
            Console.Write("  ");
            count++;
        }
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("v");
        Console.ResetColor();
    }
    static void Print_Elements_Turing_Mashine(int[] num, int size_of_input_string, int position_of_carriage)
    {
        for (int i = 0; i < size_of_input_string; i++)
        {
            if (i == position_of_carriage - 1)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
            }

            Console.Write($"{num[i]} ");
            Console.ResetColor();

        }
    }

    static bool Turing_Mashine_Running(int[] num, int position_of_carriage, int lenght, char direction,  int al)
    {
        while (true)
        {
            Print_Carriage(position_of_carriage);
            Print_Elements_Turing_Mashine(num, lenght, position_of_carriage);

            if (position_of_carriage == 0 || position_of_carriage == lenght)
            {
                return true;
            }

            if (num[position_of_carriage] == al)
            {
                num[position_of_carriage - 1] = 0;
            }
            else
            {
                num[position_of_carriage - 1] = num[position_of_carriage - 1] + 1;
            }
            switch (direction)
            {
                case 'l':
                    position_of_carriage--;
                    break;
                case 'r':
                    position_of_carriage++;
                    break;
            }


            if (position_of_carriage < 0 || position_of_carriage > lenght)
            {
                return true;
            }
        }
    }
    static void Main()
    {
        string alph;
        int al;
        bool chek;
        do
        {
            Console.WriteLine("Определим размер используемого алфавита:");
            alph = Console.ReadLine();
            al = Convert.ToInt32(alph);
            if (al > 9)
            {
                Console.WriteLine("В алфавит могут входить только числа от 0 до 9");
                chek = true;
            }
            else
            {
                chek = false;
            }
        } while (chek);

        Console.WriteLine($"В алфавит входит числа от 0 до {al}");

        Console.WriteLine("Введите элементы на ленту:");
        string str = Console.ReadLine();

        Console.WriteLine("Введите номер элемента на который будет установлена каретка:");
        int position_of_carriage = Convert.ToInt32(Console.ReadLine());
        if (position_of_carriage < 0 || position_of_carriage > str.Length)
        {
            Console.WriteLine("Неверный номер коретки!");
            return;
        }
        Console.WriteLine("Укажите направление: (l/r)");
        char direction = Convert.ToChar(Console.ReadLine());
        int lenth = str.Length;
        int[] num = new int[lenth]; ;
        for (int i = 0; i < str.Length; i++)
        {
            num[i] = Convert.ToInt32(str[i]) - 48;
            if (num[i] > al)
            {
                Console.WriteLine("Неверный формат строки!!!");
                return;
            }
        }

        Turing_Mashine_Running(num, position_of_carriage, lenth, direction, al);
    }
}