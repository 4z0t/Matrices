using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Matrices
{
    /// <summary>
    /// represents Circulant matrix for lab6
    /// </summary>
    class Circulant
    {
        private double[] data;
        public Circulant(int n)
        {
            data = new double[n];
        }

        public Circulant(double[] temp)
        {

            data = new double[temp.Length];
            for (int i = 0; i < temp.Length; i++)
                data[i] = temp[i];
        }

        /// <summary>
        /// solves Circulant matrix with given coef
        /// </summary>
        /// <param name="coef"> given coef for solving</param>
        /// <returns>solution vector</returns>
        public double[] Solve(double[] coef)
        {
            int n = data.Length;
            double[] x = new double[n];
            double[] y = new double[n];
            double[,] matx1 = new double[n, n];
            double[,] maty1 = new double[n, n];
            double[,] matx2 = new double[n, n];
            double[,] maty2 = new double[n, n];
            double[,] mat1 = new double[n, n];
            double[,] mat2 = new double[n, n];
            double[,] invert = new double[n, n];
            double[] result = new double[n];
            x[0] = 1.0 / data[0];
            y[0] = x[0];
            for (int k = 1; k < n; k++)
            {
                double Fk = 0;
                double Gk = 0;
                double[] xk = new double[k];
                double[] yk = new double[k];
                for (int i = 0; i < k; i++)
                {
                    Fk += data[k - i] * x[i];
                    Gk += data[i + 1] * y[i];
                }
                double rk = 1.0 / (1.0 - Fk * Gk);
                double sk = -rk * Fk;
                double tk = -rk * Gk;

                for (int i = 0; i < k; i++)
                {
                    xk[i] = x[i];
                    yk[i] = y[i];
                }
                x[0] = xk[0] * rk + 0 * sk;
                y[0] = xk[0] * tk + 0 * rk;
                for (int i = 1; i < k; i++)
                {
                    x[i] = xk[i] * rk + yk[i - 1] * sk;
                    y[i] = xk[i] * tk + yk[i - 1] * rk;
                }
                x[k] = 0 * rk + yk[k - 1] * sk;
                y[k] = 0 * tk + yk[k - 1] * rk;
            }

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    matx1[i, j] = (i >= j) ? x[i - j] : 0;
                    maty1[i, j] = (j >= i) ? y[n - 1 - j + i] : 0;
                    matx2[i, j] = (i > j) ? y[i - 1 - j] : 0;
                    maty2[i, j] = (j > i) ? x[n - 1 - j + 1 + i] : 0;
                }

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                {
                    mat1[i, j] = 0;
                    mat2[i, j] = 0;
                    for (int k = 0; k < n; k++)
                    {
                        mat1[i, j] += matx1[i, k] * maty1[k, j];
                        mat2[i, j] += matx2[i, k] * maty2[k, j];
                    }
                }

            var rev_x = 1.0 / x[0];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    invert[i, j] = rev_x * (mat1[i, j] - mat2[i, j]);

            for (int i = 0; i < n; i++)
            {
                result[i] = 0;
                for (int j = 0; j < n; j++)
                    result[i] += invert[i, j] * coef[j];
            }
            return result;
        }

        public override string ToString()
        {
            StringBuilder result = new();
            var n = data.Length;
            var curIndex = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    result.Append(data[curIndex]).Append('\t');
                    curIndex++;
                    if (curIndex == n && j != n - 1) curIndex = 0;
                }
                curIndex--;
                result.Append('\n');
            }
            return result.ToString();
        }

    }
}
