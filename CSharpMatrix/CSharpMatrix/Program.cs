using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace CSharpMatrix
{
    class Program
    {
        static double cs_solve(int n, int count)
        {
            Matrix matrix = new Matrix(n);
            double[] free_col = new double[n];
            for (int i = 0; i < n; i++)
                free_col[i] = i;

            double[] solution = new double[n];

            Stopwatch sw = new Stopwatch();

            sw.Start();

            for (int i = 0; i < count; i++)
            {
                solution = matrix.solve(free_col);
            }

            sw.Stop();

            return sw.Elapsed.Seconds;

        }

        [DllImport("attemptDll.dll")]
        public static extern double solve_default(int n, int count);// экспорт функции для решения уравнения со сгенерированной матрицей


        [DllImport("attemptDll.dll")]
        public static extern void solve_matrix(int n, double[] row, double[] col, double[] free_col, double[] solution);// экспорт функции для решения уравнения с переданной матрицей


        static void Main(string[] args)
        {
            Console.WriteLine("Проверка работоспособности на матрице ранга 3");

            double[] row = { 7, 8, 9 };
            double[] col = { 9, 10, 11 };
            double[] free_col = { 99, 100, 101 };
            double[] cpp_solution = new double[row.Length];


            solve_matrix(row.Length, row, col, free_col, cpp_solution);
            string cpp_text = String.Empty;
            foreach (double item in cpp_solution)
            {
                cpp_text += $"{item} ";
            }


            Matrix matrix = new Matrix(row, col);
            Console.WriteLine("Решение для матрицы:");
            Console.WriteLine("------------------------");
            Console.WriteLine(matrix);
            double[] cs_solution = matrix.solve(free_col);
            string cs_text = String.Empty;
            foreach (double item in cs_solution)
            {
                cs_text += $"{item} ";
            }


            Console.WriteLine("------------------------");
            Console.WriteLine($"Решение С++ {cpp_text}");
            Console.WriteLine($"Решение С# {cs_text}");
            Console.WriteLine("---------------------------------------------------------------------");

            string menu = "-------------------------------\n" +
                          "Протестировать программу .....1\n" +
                          "Выйти ........................0\n" +
                          "-------------------------------";


            TimeList timeList = new TimeList();
            Console.WriteLine("Введите название файла:");
            string fileName = Console.ReadLine();
            if (timeList.Load(fileName))
            {
                Console.WriteLine("Файл успешно считан");
                Console.WriteLine("Получены данные:");
                Console.WriteLine(timeList);
            }
            else
                Console.WriteLine("Файл не найден");

            int decision = 8, n = 0, count = 0;

            do
            {
                Console.WriteLine(menu);

                while (true)
                {
                    if (Int32.TryParse(Console.ReadLine(), out decision) && (decision == 0 || decision == 1))
                        break;
                    else
                        Console.WriteLine("Некорректные, данные попробуйте снова");
                }
                if (decision == 0)
                    break;
                Console.WriteLine("Введите размерность матрицы");
                while (true)
                {
                    if (Int32.TryParse(Console.ReadLine(), out n) && (n > 1))
                        break;
                    else
                        Console.WriteLine("Некорректные данные, попробуйте снова");
                }

                Console.WriteLine("Введите число повторений");
                while (true)
                {
                    if (Int32.TryParse(Console.ReadLine(), out count) && (count > 0))
                        break;
                    else
                        Console.WriteLine("Некорректные данные, попробуйте снова");
                }

                double cpp_time = solve_default(n, count);
                double cs_time = cs_solve(n, count);
                TimeItem item = new TimeItem(n, count, cpp_time, cs_time);
                timeList.Add(item);
                Console.WriteLine("Получены данные:");
                Console.WriteLine(item);

            } while (decision != 0);



            Console.WriteLine(timeList);
            timeList.Save(fileName);
        }
    }
}
