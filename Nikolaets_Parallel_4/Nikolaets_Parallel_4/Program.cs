using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nikolaets_Parallel_4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int[] permutation = { 1, 2, 3, 4 }; // Перестановка

            List<int[]> results = new List<int[]>(); // Список для збереження результатів

            Parallel.ForEach(permutation, (element) =>
            {
                int[] result = MultiplyPermutation(permutation, element);
                results.Add(result);
            });

            Console.WriteLine("Результати паралельного множення послідовності перестановок:");

            foreach (int[] result in results)
            {
                PrintPermutation(result);
            }
            Console.ReadLine();
        }
        static int[] MultiplyPermutation(int[] permutation, int factor)
        {
            int length = permutation.Length;
            int[] result = new int[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = permutation[i] * factor;
            }

            return result;
        }

        static void PrintPermutation(int[] permutation)
        {
            foreach (int element in permutation)
            {
                Console.Write($"{element} ");
            }

            Console.WriteLine();
        }
    }
}
