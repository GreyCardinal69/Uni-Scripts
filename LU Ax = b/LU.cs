using System;

namespace Uni_Scripts.LU
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //  Լուծել Ax = b
            // 3x3 Matrix
            int[][] A =
            {
                new int[] { 2, -6, -2 },
                new int[] { -2, 4, 1 },
                new int[] { 4, -4, -1 }
            };

            // Ax = b
            double[] b =
                {   -6   ,
                    4   ,
                    2
                };

            Console.WriteLine("A = L*U");
            Console.WriteLine($"| {A[0][0],4}  {A[0][1],4}  {A[0][2],4}  |        | {"l11",4}  {0,4}  {0,4}  |        | {1,4}  {"u12",4}  {"u13",4}  |");
            Console.WriteLine($"| {A[1][0],4}  {A[1][1],4}  {A[1][2],4}  |    =   | {"l21",4}  {"l22",4}  {0,4}  |   *    | {0,4}  {1,4}  {"u23",4}  |");
            Console.WriteLine($"| {A[2][0],4}  {A[2][1],4}  {A[2][2],4}  |        | {"l31",4}  {"l32",4}  {"l33",4}  |        | {0,4}  {0,4}  {1,4}  |");

            Console.WriteLine($"\nA = \n" +
                $"| l11     l11*u12             l11*u13                   |\n" +
                $"| l21     l21*u12 + l22       l21*u13 + l22*u23         |\n" +
                $"| l31     l31*u12 + l32       l31*u13 + l32*u23 + l33   |"
            );

            double[][] L = new double[3][];
            double[][] U = new double[3][];

            for (int i = 0; i < 3; i++)
            {
                L[i] = new double[3];
                U[i] = new double[3];
            }

            for (int i = 0; i < 3; i++)
            {
                U[i][i] = 1;
            }

            Console.WriteLine();
            L[0][0] = A[0][0];
            Console.WriteLine($"l11 = {A[0][0]}");
            U[0][1] = A[0][1] / L[0][0];
            Console.WriteLine($"u12 = {A[0][1]} / {L[0][0]} = {U[0][1]}");
            U[0][2] = A[0][2] / L[0][0];
            Console.WriteLine($"u13 = {A[0][2]} / {L[0][0]} = {U[0][2]}");

            L[1][0] = A[1][0];
            Console.WriteLine($"l21 = {A[1][0]}");
            L[1][1] = A[1][1] - L[1][0] * U[0][1];
            Console.WriteLine($"l22 = {A[1][1]} - ({L[1][0]} * {U[0][1]}) = {L[1][1]}");
            U[1][2] = (A[1][2] - L[1][0] * U[0][2]) / L[1][1];
            Console.WriteLine($"u22 = ({A[1][2]} - ({L[1][0]} * {U[0][2]})) / {L[1][1]} = {U[1][2]}");

            L[2][0] = A[2][0];
            Console.WriteLine($"l31 = {A[2][0]}");
            L[2][1] = A[2][1] - L[2][0] * U[0][1];
            Console.WriteLine($"l32 = {A[2][1]} - ({L[2][0]} * {U[0][1]}) = {L[2][1]}");
            L[2][2] = A[2][2] - (L[2][0] * U[0][2] + L[2][1] * U[1][2]);
            Console.WriteLine($"l33 = {A[2][2]} - ({L[2][0]} * {U[0][2]} + {L[2][1]} * {U[1][2]}) = {L[2][2]}");

            Console.WriteLine("\nL =");
            PrintMatrix(L);

            Console.WriteLine("U =");
            PrintMatrix(U);

            Console.WriteLine("\nSolving Ly = b:");
            double[] y = new double[3];
            y[0] = b[0] / L[0][0];
            Console.WriteLine($"y1 = {b[0]} / {L[0][0]} = {y[0]}");
            y[1] = (b[1] - L[1][0] * y[0]) / L[1][1];
            Console.WriteLine($"y2 = ({b[1]} - {L[1][0]} * {y[0]}) / {L[1][1]} = {y[1]}");
            y[2] = (b[2] - (L[2][0] * y[0] + L[2][1] * y[1])) / L[2][2];
            Console.WriteLine($"y3 = ({b[2]} - ({L[2][0]} * {y[0]} + {L[2][1]} * {y[1]})) / {L[2][2]} = {y[2]}");

            Console.WriteLine("\nSolution vector y:");
            PrintVector(y);

            Console.WriteLine("\nSolving Ux = y:");
            double[] x = new double[3];
            x[2] = y[2];
            Console.WriteLine($"x3 = {y[2]}");
            x[1] = y[1] - U[1][2] * x[2];
            Console.WriteLine($"x2 = {y[1]} - {U[1][2]} * {x[2]} = {x[1]}");
            x[0] = y[0] - (U[0][1] * x[1] + U[0][2] * x[2]);
            Console.WriteLine($"x1 = {y[0]} - ({U[0][1]} * {x[1]} + {U[0][2]} * {x[2]}) = {x[0]}");

            Console.WriteLine("\nSolution vector x:");
            PrintVector(x);
        }

        static void PrintMatrix(double[][] matrix)
        {
            int rows = matrix.Length;
            int cols = matrix[0].Length;
            for (int i = 0; i < rows; i++)
            {
                Console.Write("| ");
                for (int j = 0; j < cols; j++)
                {
                    Console.Write($"{matrix[i][j],8:F3} ");
                }
                Console.WriteLine("|");
            }
            Console.WriteLine();
        }

        static void PrintVector(double[] vector)
        {
            Console.Write("[");
            for (int i = 0; i < vector.Length; i++)
            {
                Console.Write($" {vector[i],8:F3} ");
            }
            Console.WriteLine("]");
        }
    }
}