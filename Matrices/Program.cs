using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Matrices
{
    class Program
    {
        const string MatrixDll = "C:\\Users\\4z0t\\source\\repos\\Matrices\\Debug\\matrixDLL.dll";
        [DllImport(MatrixDll)]
        static extern double Time(int n, int repeat);
        [DllImport(MatrixDll)]
        static unsafe extern void Solve(int n, double[] max_1, double[] max_2, double* max_3);
        static string path = "time.time";



        static void Main(string[] args)
        {

            TimeList t = new TimeList();


            if (File.Exists(path))
            {
                t.Load(path);
            }
            else
            {
                File.Create(path).Close();
            }

            int repeats = 5000;
            int size = 100;
            double csTime = TimeCsharp(size, repeats);
            double cppTime = Time(size, repeats);
            t.Add(new(size, repeats, csTime, cppTime));
            Console.WriteLine(t);
            t.Save(path);



        }
        static double TimeCsharp(int n, int k)
        {
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            Circulant matrix = new Circulant(n);
            double[] right = new double[n];

            for (int i = 0; i < n; i++)
                right[i] = i * i;

            for (int i = 0; i < k; i++)
                matrix.Solve(right);
            sw.Stop();
            return sw.Elapsed.TotalSeconds;
        }


    }
}

