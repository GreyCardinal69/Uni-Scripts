using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uni_Scripts.Օրթոնորմավորել_Բազիսը
{
    internal class BasisNormalize
    {
        static void Main( string[] args )
        {
            double[][] xVectors = new double[][]
            {
            new double[] {0,0,0,1},
            new double[] {0,0,-1,3},
            new double[] {0,3,3,1},
            new double[] {4,2,4,4}
            };

            double[][] yVectors = NormalizeVectors( xVectors );

            string[][] yVecstr = ConvertToFraction( yVectors );

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
                    zVectors[i][j] = yVectors[i][j] / length;
                }
            }

            string[][] zVecstr = ConvertToFraction( zVectors );
            Console.WriteLine();

            for ( int i = 0; i < zVectors.Length; i++ )
            {
                Console.WriteLine( $"z{i + 1}: [{string.Join( ", ", zVecstr[i] )}]" );
            }
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
                    fractionXY = ToFraction( scalarXY / scalarYY );
                    for ( int k = 0; k < yVectors[j].Length; k++ )
                    {
                        yVectors[i][k] -= ( scalarXY / scalarYY ) * yVectors[j][k];
                    }
                }
                fractions.Add( fractionXY );
            }

            Console.WriteLine( $"y1 = x1 = [{string.Join( ',', ConvertToFraction( yVectors )[0] )}]" );
            string fracs = "";
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
                if ( !string.IsNullOrEmpty( fractions[i + 1] ) )
                {
                    fracs += fractions[i + 1];
                    if ( i < n - 1 ) fracs += " - ";
                    Console.WriteLine( $" = x{i + 1} - {fracs} * y{yindex}".Replace( "-  *", "*" ) );
                }
            }

            Console.WriteLine();
            return yVectors;
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
    }
}