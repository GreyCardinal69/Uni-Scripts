using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Uni_Scripts.Օրթոնորմավորել_Բազիսը
{
    internal class BasisNormalize
    {
        static void Main( string[] args )
        {
            // if the basis is in bad order, the order will be changed.
            double[][] xVectors = new double[][]
            {
                new double[] {1,0,0,0},
                new double[] {0,0,0,-2},
                new double[] {-3,2,0,-3},
                new double[] {0,-4,-4,3},
            };

            bool dependent = AreVectorsDependent( xVectors );

            if ( dependent )
            {
                Console.WriteLine( "These Xn and Xm vectors are dependent, the latter was changed" );
                ModifyDependentArrays( xVectors );
            }
            else
            {
                Console.WriteLine( "The vectors are independent." );
            }

            double[][] reorderedVectors = ReorderBasis( xVectors );

            Console.WriteLine( "Reordered Basis:" );
            foreach ( var vector in reorderedVectors )
            {
                Console.WriteLine( $"[{string.Join( ", ", vector )}]" );
            }

            Console.WriteLine();

            double[][] yVectors = NormalizeVectors( xVectors );

            string[][] yVecstr = ConvertToFraction( yVectors );
            Console.WriteLine();
            for ( int i = 0; i < yVectors.Length; i++ )
            {
                Console.WriteLine( $"y{i + 1}: [{string.Join( ", ", yVecstr[i] )}]" );
            }

            double[][] zVectors = new double[yVectors.Length][];
            for ( int i = 0; i < yVectors.Length; i++ )
            {
                double length = VectorLength( yVectors[i] );
                zVectors[i] = new double[yVectors[i].Length];
                for ( int j = 0; j < yVectors[i].Length; j++ )
                {
                    if ( length == 0 )
                    {
                        zVectors[i][j] = yVectors[i][j];
                    }
                    else
                    {
                        zVectors[i][j] = yVectors[i][j] / length;
                    }
                }
            }

            string[][] zVecstr = ConvertToFraction( zVectors );
            Console.WriteLine();

            for ( int i = 0; i < zVectors.Length; i++ )
            {
                Console.WriteLine( $"z{i + 1}: [{string.Join( ", ", zVecstr[i] )}]" );
            }
        }

        static bool AreVectorsDependent( double[][] vectors )
        {
            if ( vectors.Length < vectors[0].Length )
                return true;

            double[,] matrix = new double[vectors.Length, vectors[0].Length];
            for ( int i = 0; i < vectors.Length; i++ )
            {
                for ( int j = 0; j < vectors[0].Length; j++ )
                {
                    matrix[i, j] = vectors[i][j];
                }
            }

            double determinant = CalculateDeterminant( matrix );

            return Math.Abs( determinant ) < 1e-10;
        }

        static double CalculateDeterminant( double[,] matrix )
        {
            int n = matrix.GetLength( 0 );
            if ( n != matrix.GetLength( 1 ) )
                throw new ArgumentException( "Matrix must be square" );

            if ( n == 1 )
                return matrix[0, 0];

            if ( n == 2 )
                return matrix[0, 0] * matrix[1, 1] - matrix[0, 1] * matrix[1, 0];

            double determinant = 0;
            for ( int j = 0; j < n; j++ )
            {
                determinant += matrix[0, j] * Math.Pow( -1, 0 + j ) * CalculateDeterminant( SubMatrix( matrix, 0, j ) );
            }

            return determinant;
        }

        static double[,] SubMatrix( double[,] matrix, int excludeRow, int excludeColumn )
        {
            int n = matrix.GetLength( 0 );
            int m = matrix.GetLength( 1 );
            double[,] subMatrix = new double[n - 1, m - 1];

            int rowIndex = 0;
            for ( int i = 0; i < n; i++ )
            {
                if ( i == excludeRow )
                    continue;

                int columnIndex = 0;
                for ( int j = 0; j < m; j++ )
                {
                    if ( j == excludeColumn )
                        continue;

                    subMatrix[rowIndex, columnIndex] = matrix[i, j];
                    columnIndex++;
                }
                rowIndex++;
            }

            return subMatrix;
        }

        static double[][] ReorderBasis( double[][] basis )
        {
            var nonZeroCounts = basis.Select( vector => vector.Count( num => Math.Abs( num ) > 1e-10 ) ).ToArray();

            int minNonZeroIndex = 0;
            for ( int i = 1; i < nonZeroCounts.Length; i++ )
            {
                if ( nonZeroCounts[i] < nonZeroCounts[minNonZeroIndex] )
                {
                    minNonZeroIndex = i;
                }
            }

            double[][] reorderedBasis = new double[basis.Length][];
            reorderedBasis[0] = basis[minNonZeroIndex];

            int j = 1;
            for ( int i = 0; i < basis.Length; i++ )
            {
                if ( i != minNonZeroIndex )
                {
                    reorderedBasis[j++] = basis[i].ToArray(); // Copy the array over
                }
            }

            return reorderedBasis;
        }

        static double[][] NormalizeVectors( double[][] xVectors )
        {
            int n = xVectors.Length;
            double[][] yVectors = new double[n][];
            double scalarYY;
            List<string> fractions = new List<string>();
            fractions.Add( "" );
            for ( int i = 0; i < n; i++ )
            {
                yVectors[i] = new double[xVectors[i].Length];
                Array.Copy( xVectors[i], yVectors[i], xVectors[i].Length );

                string fractionXY = "";
                for ( int j = 0; j < i; j++ )
                {
                    double scalarXY = ScalarProduct( xVectors[i], yVectors[j] );
                    scalarYY = ScalarProduct( yVectors[j], yVectors[j] );

                    if ( scalarYY == 00 && scalarXY == 0 )
                    {
                        fractionXY = "0";
                    }
                    else
                    {
                        fractionXY = ToFraction( scalarXY / scalarYY );
                    }
                    for ( int k = 0; k < yVectors[j].Length; k++ )
                    {
                        if ( scalarYY == 00 && scalarXY == 0 )
                        {
                            yVectors[i][k] -= 0;
                        }
                        else
                        {
                            yVectors[i][k] -= ( scalarXY / scalarYY ) * yVectors[j][k];
                        }
                    }
                }
                fractions.Add( fractionXY );
            }

            Console.WriteLine( $"y1 = x1 = [{string.Join( ',', ConvertToFraction( yVectors )[0] )}]" );

            for ( int i = 1; i < n; i++ )
            {
                Console.Write( $"y{i + 1} = x{i + 1} - " );
                int yindex = 1;
                for ( int e = 0; e < i; e++ )
                {
                    Console.Write( $"(x{i + 1},y{yindex})/(y{yindex},y{yindex}) * y{yindex}" );
                    if ( e < i - 1 )
                    {
                        yindex++; Console.Write( " - " );
                    }
                }
                Console.Write( $" = {VectorToString( xVectors[i] )}" );
                yindex = 1;
                for ( int e = 0; e < i; e++ )
                {
                    Console.Write( $" - {fractions[yindex + 1]} * y{yindex}" );
                    if ( e < i - 1 )
                    {
                        yindex++;
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine();
            return yVectors;
        }

        private static string VectorToString( double[] vector )
        {
            StringBuilder sb = new StringBuilder();

            sb.Append( '(' ).Append( ' ' );

            for ( int i = 0; i < vector.Length; i++ )
            {
                sb.Append( vector[i] ).Append( ',' );
            }

            sb.Append( ' ' ).Append( ')' );
            return sb.ToString();
        }

        static double ScalarProduct( double[] vector1, double[] vector2 )
        {
            if ( vector1.Length != vector2.Length )
                throw new ArgumentException( "Vectors must have the same length" );

            double result = 0;
            for ( int i = 0; i < vector1.Length; i++ )
            {
                result += vector1[i] * vector2[i];
            }

            return result;
        }

        static double VectorLength( double[] vector )
        {
            double sumOfSquares = 0;
            foreach ( double component in vector )
            {
                sumOfSquares += component * component;
            }
            return Math.Sqrt( sumOfSquares );
        }

        static string[][] ConvertToFraction( double[][] vectors )
        {
            string[][] fractions = new string[vectors.Length][];

            for ( int i = 0; i < vectors.Length; i++ )
            {
                fractions[i] = new string[vectors[i].Length];
                for ( int j = 0; j < vectors[i].Length; j++ )
                {
                    fractions[i][j] = ToFraction( vectors[i][j] );
                }
            }

            return fractions;
        }

        static string ToFraction( double fraction )
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

            if ( numerator > 0 && denominator < 0 )
            {
                return $"-{numerator}/{Math.Abs( denominator )}";
            }

            return $"{( ( numerator < 0 || denominator < 0 ) ? $"-{Math.Abs( numerator )}/{Math.Abs( denominator )}" : $"{Math.Abs( numerator )}/{Math.Abs( denominator )}" )}";
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

        static bool ModifyDependentArrays( double[][] arrays )
        {
            bool modified = false;

            for ( int i = 0; i < arrays.Length; i++ )
            {
                for ( int j = i + 1; j < arrays.Length; j++ )
                {
                    double[] array1 = arrays[i];
                    double[] array2 = arrays[j];

                    double A = FindA( array1, array2 );

                    if ( A != -1 )
                    {
                        PrintArray( array1 );
                        PrintArray( array2 );
                        Console.WriteLine();

                        ModifyArray( array2 );
                        modified = true;

                        break;
                    }
                }
            }

            return modified;
        }

        static double FindA( double[] x, double[] y )
        {
            if ( x.Length != y.Length )
                throw new ArgumentException( "Arrays must have the same length" );

            for ( int i = 0; i < x.Length; i++ )
            {
                if ( x[i] != 0 && y[i] % x[i] == 0 )
                {
                    double A = y[i] / x[i];
                    bool isValid = true;
                    for ( int j = 0; j < x.Length; j++ )
                    {
                        if ( x[j] * A != y[j] )
                        {
                            isValid = false;
                            break;
                        }
                    }
                    if ( isValid )
                        return A;
                }
            }
            return -1;
        }

        static void ModifyArray( double[] array )
        {
            for ( int i = 0; i < array.Length; i++ )
            {
                if ( array[i] == 0 )
                {
                    array[i] = 1;
                    return;
                }
            }
        }

        static void PrintArray( double[] array )
        {
            Console.Write( "[" );
            for ( int i = 0; i < array.Length; i++ )
            {
                Console.Write( array[i] );
                if ( i < array.Length - 1 )
                {
                    Console.Write( ", " );
                }
            }
            Console.WriteLine( "]" );
        }
    }
}