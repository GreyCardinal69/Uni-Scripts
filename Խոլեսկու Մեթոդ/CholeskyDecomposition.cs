using System;

namespace Uni_Scripts.Խոլեսկու_Մեթոդ
{
    internal class CholeskyDecomposition
    {
        public static void Main(string[] args)
        {
            // 3x3
            // A = U^T*U
            double[][] A =
            {
                new double[] { 64, 24, -16 },
                new double[] { 24, 58,  -13 },
                new double[] { -16,  -13, 86 }
            };

            Console.WriteLine("A =");
            PrintMatrix(A);

            double[][] U = new double[3][];
            for (int i = 0; i < 3; i++)
            {
                U[i] = new double[3];
            }

            Console.WriteLine("Cholesky Decomposition: A = U^T * U");
            Console.WriteLine("General formulas:");
            Console.WriteLine("  u_ii = sqrt(A_ii - sum_{k=1}->{i-1} (u_ki^2))");
            Console.WriteLine("  u_ij = (A_ij - sum_{k=1}->{i-1} (u_ki * u_kj)) / u_ii");
            Console.WriteLine();

            for (int i = 0; i < 3; i++)
            {
                double sum1 = 0;
                string sum1Details = "";

                for (int k = 0; k < i; k++)
                {
                    sum1 += U[k][i] * U[k][i];
                    sum1Details += (k == 0 ? "" : " + ") + $"({U[k][i]}^2)";
                }

                U[i][i] = Math.Sqrt(A[i][i] - sum1);
                Console.WriteLine($"u_{i + 1}{i + 1} = sqrt({A[i][i]} - {(sum1Details == "" ? "0" : sum1Details)}) = {U[i][i]}");

                for (int j = i + 1; j < 3; j++)
                {
                    double sum2 = 0;
                    string sum2Details = "";

                    for (int k = 0; k < i; k++)
                    {
                        sum2 += U[k][i] * U[k][j];
                        sum2Details += (k == 0 ? "" : " + ") + $"({U[k][i]} * {U[k][j]})";
                    }

                    U[i][j] = (A[i][j] - sum2) / U[i][i];
                    Console.WriteLine($"u_{i + 1}{j + 1} = ({A[i][j]} - {(sum2Details == "" ? "0" : sum2Details)}) / {U[i][i]} = {U[i][j]}");
                }
            }

            Console.WriteLine("\nUpper Triangular Matrix U:");
            PrintMatrix(U);
        }

        static void PrintMatrix(double[][] matrix)
        {
            for (int i = 0; i < matrix.Length; i++)
            {
                Console.Write("| ");
                for (int j = 0; j < matrix[i].Length; j++)
                {
                    Console.Write($"{matrix[i][j],8:F3} ");
                }
                Console.WriteLine("|");
            }
            Console.WriteLine();
        }
    }
}