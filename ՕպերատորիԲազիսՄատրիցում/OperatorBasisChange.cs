using System;

namespace Uni_Scripts.ՕպերատորիԲազիսՄատրիցում
{
    internal class OperatorBasisChange
    {
        static void Main( string[] args )
        {
            /*
             * Օրինակ
             *  Գծային օպերատորի մատրիցը e1,e2,e3 բազիսում ունի հետևյալ տեսքը։
             *  
             *      | |
             *      | |
             *      | |
             *     \\ //
             *      \ /
             *       |              */
            double[,] A = {
                { -1, 1, 3 },
                { 4, -2, -1 },
                { -2, 3, 4 }
            };

            /*
             * Օրինակ
             * Որոշել օպերատորի մատրիցը e1, -e1 + e2, -e3 բազիսում
             *  // Լրացնել մատրիցը վերևում տրված բազիսից ներքևում գրվածի նման․
             *  
             *  e1 = { 1,0,0 }
             *  -e1 + e2 = { -1, 1, 0 }
             *  -e3 = { 0,0, -1 }
             *      | |
             *      | |
             *      | |
             *     \\ //
             *      \ /
             *       |              */
            double[,] T =
            {
                { 1,0,0 },
                { -1,1,0 },
                { 0,0,-1}
            };

            //
            //
            //         Սկսել Ծրագիրը
            //
            //

            ChangeOperatorBasis( A, T );
        }

        private static void ChangeOperatorBasis( double[,] A, double[,] T )
        {
            Console.WriteLine( "A=" );
            PrintMatrix( A );
            Console.WriteLine();

            Console.WriteLine( "T=" );
            PrintMatrix( T );
            Console.WriteLine();

            GuassianEngine.GetMatrixInverseGaussian( T, 'T' );
            double[,] inverseT = GuassianEngine.Result;

            Console.WriteLine( "T*A*T^-1 = " );
            PrintMatrices( T, A, inverseT );
            Console.WriteLine();

            double[,]? TA = MultiplyMatrices( T, A );

            PrintMatrices( TA, inverseT );
            Console.WriteLine();

            var TAiT = MultiplyMatrices( TA, inverseT );

            PrintMatrix( TAiT );
        }

        private static void PrintMatrices( double[,] matrix1, double[,] matrix2, double[,] matrix3 )
        {
            int rows = matrix1.GetLength( 0 );
            int cols = matrix1.GetLength( 1 );

            for ( int i = 0; i < rows; i++ )
            {
                for ( int j = 0; j < cols; j++ )
                {
                    Console.Write( matrix1[i, j] + "\t" );
                }
                Console.Write( "\t" );

                for ( int j = 0; j < cols; j++ )
                {
                    Console.Write( matrix2[i, j] + "\t" );
                }
                Console.Write( "\t" );

                for ( int j = 0; j < cols; j++ )
                {
                    Console.Write( matrix3[i, j] + "\t" );
                }
                Console.WriteLine();
            }
        }
        private static void PrintMatrices( double[,] matrix1, double[,] matrix2 )
        {
            int rows = matrix1.GetLength( 0 );
            int cols = matrix1.GetLength( 1 );

            for ( int i = 0; i < rows; i++ )
            {
                for ( int j = 0; j < cols; j++ )
                {
                    Console.Write( matrix1[i, j] + "\t" );
                }
                Console.Write( "\t" );

                for ( int j = 0; j < cols; j++ )
                {
                    Console.Write( matrix2[i, j] + "\t" );
                }
                Console.WriteLine();
            }
        }

        private static double[,] MultiplyMatrices( double[,] matrix1, double[,] matrix2 )
        {
            int rows1 = matrix1.GetLength( 0 );
            int cols1 = matrix1.GetLength( 1 );
            int rows2 = matrix2.GetLength( 0 );
            int cols2 = matrix2.GetLength( 1 );

            if ( cols1 != rows2 )
            {
                throw new ArgumentException( "Number of columns of the first matrix must be equal to the number of rows of the second matrix." );
            }

            double[,] result = new double[rows1, cols2];

            for ( int i = 0; i < rows1; i++ )
            {
                for ( int j = 0; j < cols2; j++ )
                {
                    double sum = 0;
                    for ( int k = 0; k < cols1; k++ )
                    {
                        sum += matrix1[i, k] * matrix2[k, j];
                    }
                    result[i, j] = sum;
                }
            }

            return result;
        }

        private static void PrintMatrix( double[,] matrix )
        {
            int rows = matrix.GetLength( 0 );
            int cols = matrix.GetLength( 1 );

            for ( int i = 0; i < rows; i++ )
            {
                for ( int j = 0; j < cols; j++ )
                {
                    Console.Write( matrix[i, j] + "\t" );
                }
                Console.WriteLine();
            }
        }
    }

    public class GuassianEngine
    {
        private static string[] _lastIdentities;
        private static char _matr;

        public static double[,] Result;

        public static void GetMatrixInverseGaussian( double[,] matrix, char matr )
        {
            _matr = matr;
            int n = matrix.GetLength( 0 );

            double[,] augmentedMatrix = new double[n, 2 * n];
            string[] identityMatrix = new string[n];
            _lastIdentities = new string[n];

            for ( int i = 0; i < n; i++ )
            {
                identityMatrix[i] = $"B{i + 1}";
                for ( int j = 0; j < n; j++ )
                {
                    augmentedMatrix[i, j] = matrix[i, j];
                }
                augmentedMatrix[i, i + n] = 1;
            }

            PrintMatrix( augmentedMatrix, identityMatrix, n, 2 * n, "Initial Augmented Matrix" );

            double[,] previousMatrix = new double[n, 2 * n];
            Array.Copy( augmentedMatrix, previousMatrix, augmentedMatrix.Length );

            int step = 1;
            for ( int i = 0; i < n; i++ )
            {
                double divisor = augmentedMatrix[i, i];

                for ( int j = 0; j < 2 * n; j++ )
                {
                    augmentedMatrix[i, j] /= divisor;
                }

                for ( int j = 0; j < n; j++ )
                {
                    if ( j != i && augmentedMatrix[j, i] != 0 )
                    {
                        double multiplier = -augmentedMatrix[j, i];
                        for ( int k = 0; k < 2 * n; k++ )
                        {
                            augmentedMatrix[j, k] += multiplier * augmentedMatrix[i, k];
                        }
                        if ( multiplier == 1 )
                        {
                            identityMatrix[j] += $" + {identityMatrix[i]}";
                        }
                        else
                        {
                            identityMatrix[j] += $" + {multiplier} * {identityMatrix[i]}";
                        }
                    }
                }
                if ( !MatrixEquals( previousMatrix, augmentedMatrix ) )
                {
                    Console.WriteLine( $"Step {step}:" );
                    step++;
                    PrintMatrix( augmentedMatrix, identityMatrix, n, 2 * n );
                    Array.Copy( augmentedMatrix, previousMatrix, augmentedMatrix.Length );
                }
            }

            double[,] inverseMatrix = new double[n, n];
            for ( int i = 0; i < n; i++ )
            {
                for ( int j = 0; j < n; j++ )
                {
                    inverseMatrix[i, j] = augmentedMatrix[i, j + n];
                }
                Console.WriteLine( $"X{i + 1} = {_lastIdentities[i]}" );
            }

            Console.WriteLine( $"\n{_matr}^-1 = \n" );

            Result = inverseMatrix;

            for ( int i = 0; i < n; i++ )
            {
                for ( int j = 0; j < n; j++ )
                {
                    Console.Write( FractionFromDecimal( inverseMatrix[i, j] ) + "\t" );
                }
                Console.WriteLine( '\n' );
            }
        }

        static void PrintMatrix( double[,] matrix, string[] identityMatrix, int rows, int cols, string message = "" )
        {
            if ( !string.IsNullOrEmpty( message ) )
                Console.WriteLine( message );

            for ( int i = 0; i < rows; i++ )
            {
                for ( int j = 0; j < cols; j++ )
                {
                    if ( j < cols / 2 )
                    {
                        if ( IsWholeNumber( matrix[i, j] ) )
                        {
                            Console.Write( $"{matrix[i, j]}\t" );
                        }
                        else
                        {
                            Console.Write( FractionFromDecimal( matrix[i, j] ) + "\t" );
                        }
                    }
                    else
                    {
                        if ( j == cols / 2 )
                        {
                            string identityTerm = identityMatrix[i];
                            string[] terms = identityTerm.Split( new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries );
                            string formattedIdentity = "";
                            foreach ( string term in terms )
                            {
                                string formattedTerm = term.Trim();
                                if ( formattedTerm.StartsWith( '-' ) )
                                {
                                    formattedIdentity += $"{formattedTerm} ";
                                }
                                else if ( formattedIdentity == "" )
                                {
                                    formattedIdentity += $"{formattedTerm} ";
                                }
                                else
                                {
                                    formattedIdentity += $"+ {formattedTerm} ";
                                }
                            }
                            Console.Write( formattedIdentity + "\t" );
                            _lastIdentities[i] = formattedIdentity;
                        }
                        else
                        {
                            Console.Write( " \t" );
                        }
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        static bool IsWholeNumber( double number ) => number == Math.Floor( number );

        static string FractionFromDecimal( double fraction )
        {
            if ( fraction == 0 )
                return "0";

            int wholePart = ( int ) fraction;
            double fractionalPart = fraction - wholePart;

            double precision = 1.0e-9;

            int numerator = 1;
            int denominator = 1;
            double fractionValue = numerator / ( double ) denominator;

            while ( Math.Abs( fractionValue - fractionalPart ) > precision )
            {
                if ( fractionValue < fractionalPart )
                {
                    numerator++;
                }
                else
                {
                    denominator++;
                    numerator = ( int ) Math.Round( fractionalPart * denominator );
                }
                fractionValue = numerator / ( double ) denominator;
            }

            int gcd = GCD( numerator, denominator );
            numerator /= gcd;
            denominator /= gcd;

            numerator += wholePart * denominator;

            if ( denominator == 1 ) return numerator.ToString();

            return $"{numerator}/{denominator}";
        }

        static int GCD( int a, int b )
        {
            while ( b != 0 )
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        static bool MatrixEquals( double[,] a, double[,] b )
        {
            for ( int i = 0; i < a.GetLength( 0 ); i++ )
            {
                for ( int j = 0; j < a.GetLength( 1 ); j++ )
                {
                    if ( Math.Abs( a[i, j] - b[i, j] ) > double.Epsilon )
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}