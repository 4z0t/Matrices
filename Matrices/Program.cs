using System;

namespace Matrices
{
    class Program
    {
        static void Main(string[] args)
        {
            double[] ar = new double[3] { 1, 2, 3 };
            Circulant c = new (ar);
            Console.WriteLine(c);

        }
    }
}
