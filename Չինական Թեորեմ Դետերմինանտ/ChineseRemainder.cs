using System;

namespace Uni_Scripts.Չինական_Թեորեմ_Դետերմինանտ
{
    internal class ChineseRemainder
    {
        public static void Main( string[] args )
        {
            // ==================================================
            //
            //      1․ Լրացնել դետերմինանտը։
            //
            //
            int[] matrix = new int[]
            {
                865, 204, 684,
                576,  136, 456,
                864, 204, 685
            };

            Console.WriteLine( "Original Matrix:" );
            PrintMatrixOneDimensional( matrix );
            Console.WriteLine();

            // ==================================================
            //
            //      2․ Գրել վերին սահմանը։
            //          Օրինակ՝ 0 < d < 137
            //
            //
            int range = 140;

            // ==================================================
            //
            //      3․ Սկսել ծրագիրը։
            //
            //

            int num1 = 1;
            int num2 = 2;

            int rem1 = 0;
            int rem2 = 0;

            while ( true )
            {
                if ( CoprimeBool( num1, num2 ) && num1 * num2 > range )
                {
                    break;
                }
                else
                {
                    num1++;
                    num2++;
                }
            }

            Console.WriteLine( $"Chosen coprimes are: {num1} and {num2}" );
            Console.WriteLine();
            int[] remNum1Matrix;
            int lower = 0;

            remNum1Matrix = new int[]
            {
                    LowerX( matrix[0] % num1, num1, 7), LowerX( matrix[1] % num1, num1, 7), LowerX( matrix[2] % num1, num1, 7),
                    LowerX( matrix[3] % num1, num1, 7), LowerX( matrix[4] % num1, num1, 7), LowerX( matrix[5] % num1, num1, 7),
                    LowerX( matrix[6] % num1, num1, 7), LowerX( matrix[7] % num1, num1, 7), LowerX( matrix[8] % num1, num1, 7)
            };

            PrintMatrix( remNum1Matrix );
            rem1 = GetDeterminant( remNum1Matrix );
            Console.WriteLine( $"=> {rem1} => {rem1 % num1} mod{num1}" );
            Console.WriteLine();

            lower = 0;
            int[] remNum2Matrix;

            remNum2Matrix = new int[]
            {
                    LowerX( matrix[0] % num2, num2, 7), LowerX( matrix[1] % num2, num2, 7), LowerX( matrix[2] % num2, num2, 7),
                    LowerX( matrix[3] % num2, num2, 7), LowerX( matrix[4] % num2, num2, 7), LowerX( matrix[5] % num2, num2, 7),
                    LowerX( matrix[6] % num2, num2, 7), LowerX( matrix[7] % num2, num2, 7), LowerX( matrix[8] % num2, num2, 7)
            };

            PrintMatrix( remNum2Matrix );
            rem2 = GetDeterminant( remNum2Matrix );
            Console.WriteLine( $"=> {rem2} => {rem2 % num2} mod{num2}" );

            Console.WriteLine();

            Console.WriteLine( $"d ≡ {rem1 % num1} mod{num1}" );
            Console.WriteLine( $"d ≡ {rem2 % num2} mod{num2}" );
            Console.WriteLine();
            Console.WriteLine( $"{num2} = 1*{num1}+1" );
            Console.WriteLine( $"1 = {num2}-{num1}" );
            Console.WriteLine();
            int final = ( ( rem1 % num1 ) * num2 ) - ( ( rem2 % num2 ) * num1 );
            Console.WriteLine( $"d = {rem1 % num1}*{num2}-{rem2 % num2}*{num1} => {(( rem1 % num1 ) * num2 )} - {( ( rem2 % num2 ) * num1 )} = {final}" );
            Console.WriteLine();

            if ( final < 0 )
            {
                Console.WriteLine($"{final} + {num1}*{num2} => {final + (num1*num2)}");
            }

            Console.WriteLine( "Proof:" );
            Console.WriteLine( $"{num1}*{num2}={num1 * num2}" );
            Console.WriteLine( $"{num1 * num2}-{Math.Abs( final )} = {( num1 * num2 ) - Math.Abs( final )}" );
        }

        static void PrintMatrixOneDimensional( int[] matrix )
        {
            if ( matrix.Length != 9 )
            {
                Console.WriteLine( "Invalid matrix size. Expected 9 elements." );
                return;
            }

            for ( int i = 0; i < 3; i++ )
            {
                for ( int j = 0; j < 3; j++ )
                {
                    Console.Write( matrix[i * 3 + j] + " " );
                }
                Console.Write( "\n" );
            }
        }

        static int LowerX( int a, int num, int c ) => a >= c ? a - num : a;

        static int GetDeterminant( int[] matrix )
        {
            int a = matrix[0], b = matrix[1], c = matrix[2];
            int d = matrix[3], e = matrix[4], f = matrix[5];
            int g = matrix[6], h = matrix[7], i = matrix[8];

            return a * e * i + b * f * g + c * d * h - c * e * g - b * d * i - a * f * h;
        }

        static void PrintMatrix( int[] matrix )
        {
            int index = 0;
            for ( int i = 0; i < 3; i++ )
            {
                for ( int j = 0; j < 3; j++ )
                {
                    Console.Write( matrix[index++] + " " );
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        static int Gcd( int a, int b )
        {
            if ( a == 0 || b == 0 )
                return 0;

            if ( a == b )
                return a;

            if ( a > b )
                return Gcd( a - b, b );

            return Gcd( a, b - a );
        }

        static bool CoprimeBool( int a, int b ) => Gcd( a, b ) == 1;
    }
}