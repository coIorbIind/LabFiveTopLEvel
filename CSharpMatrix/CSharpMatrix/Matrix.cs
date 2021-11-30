using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Math;

namespace CSharpMatrix
{
    class Matrix
    {
        double[] row = null;
        double[] col = null;

        public Matrix(int n)
        {
            row = new double[n];
            col = new double[n];
            for (int i = 0; i < n; i++)
            {
                row[i] = 2 * i;
                col[i] = 3 * i;
            }
            col[0] = row[row.Length - 1];
        }

        public Matrix(double[] row_value, double[] col_value)
        {
            if (row_value.Length != col_value.Length)
                throw new ArgumentException("Строка и столбец имею разные размеры");
            if (row_value[row_value.Length - 1] != col_value[0])
                throw new ArgumentException("Последний элемент строки должен совпадать с первым элементом столбца");
            row = new double[row_value.Length];
            col = new double[col_value.Length];
            for (int i = 0; i < row_value.Length; i++)
            {
                row[i] = row_value[i];
                col[i] = col_value[i];
            }
        }

        public double[] solve(double[] free_col)
        {
            int n = row.Length;
            double[] x = new double[n];
            double[] y = new double[n];
            double[] solution = new double[n];


            x[0] = 1 / col[0];
            y[0] = 1 / col[0];
            double F, G, r, s, t;
            for (int k = 1; k < n; k++)
            {
                F = G = 0.0;
                for (int i = 0; i < k; i++)
                {
                    F += row[(n - 1) - (k - i)] * x[i];
                    G += col[i + 1] * y[i];
                }
                r = 1.0 / (1.0 - F * G);
                s = -r * F;
                t = -r * G;

                double[] tempY = new double[n];
                Array.Copy(y, tempY, n);

                y[0] = t * x[0];
                x[0] = r * x[0];
                for (int i = 1; i <= k; i++)
                {
                    y[i] = t * x[i] + r * tempY[i - 1];
                    x[i] = r * x[i] + s * tempY[i - 1];
                }
            }


            for (int i = 0; i < n; i++)
            {
                double[] transpose = new double[n];
                for (int j = 0; j < n; j++)
                {
                    double a = 0.0;
                    for (int k = 0; k < Min(i, j) + 1; k++)
                        a += x[i - k] * y[n - 1 - j + k];

                    double b = 0.0;
                    if (i != 0 && j != 0)
                        for (int k = 0; k < Min(i, j); k++)
                            b += y[i - 1 - k] * x[n - j + k];

                    transpose[j] = a - b;
                }

                solution[i] = 0.0;
                for (int k = 0; k < n; k++)
                    solution[i] += transpose[k] / x[0] * free_col[(n - 1) - k];
            }

            return solution;
        }

        public override string ToString()
        {
            string text_col = string.Empty;
            foreach (double item in col)
            {
                text_col += $"{item} ";
            }
            string text_row = string.Empty;
            foreach (double item in row)
            {
                text_row += $"{item} ";
            }

            return $"Матрица ранга {row.Length} задана:\nСтолбцом: {text_col} \nСтрокой: {text_row}";
        }
    }
}
