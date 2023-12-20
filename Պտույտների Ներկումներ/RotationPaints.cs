using System;
using System.Text;
using System.Collections.Generic;

namespace Uni_Scripts.Պտույտների_Ներկումներ
{
    internal class RotationPaints
    {
        public static void Main( string[] args )
        {
            // ==================================================
            //
            //      1․ Ընտրել պատկերը և պահանջը։
            //
            //      QaranistGagat
            //      QaranistKox
            //      QaranistNist
            //      XoranardGagat
            //      XoranardKox
            //      XoranardNist

            Type type = Type.QaranistGagat;

            // x^n*y^m*z^p
            // ==================================================
            //
            //      2․ Ընտրել պտույտները։
            //  
            //  Օրինակ՝ x^3*y^3*z^3
            //  Որը Նշանակում է
            //          x-ի 3 պտույտ։
            //          y-ի 3 պտույտ։
            //          z-ի 3 պտույտ։
            //
            string target = "x^6*y^3*z^3";

            // ==================================================
            //
            //      3․ Սկսել ծրագիրը։
            //  

            string[] split = target.Split( '*' );
            int targExpo1 = split[0][2] - '0', targExpo2 = split[1][2] - '0', targExpo3 = split[2][2] - '0';

            Console.WriteLine( $"Target: x^{targExpo1}y^{targExpo2}z^{targExpo3}\n" );

            StringBuilder result = new StringBuilder();

            switch ( type )
            {
                case Type.XoranardGagat:
                    result.Append( $"1/24( " );
                    Console.WriteLine( "Formula in Cyclic form:" );
                    Console.WriteLine( $"1/24( T1^8 + 6*T4^2 + 8*T1^2*T3^2 + 9*T2^4 )" );
                    Console.WriteLine( "\nFormula in Polynomial form:" );
                    Console.WriteLine( $"1/24( ( x + y + z )^8 + 6( x^4 + y^4 + z^4 )^2 + 8( x + y + z )^2 * ( x^3 + y^3 + z^3 )^2 + 9 ( x^2 + y^2 + z^2 )^4 )" );
                    break;
                case Type.XoranardKox:
                    result.Append( $"1/24( " );
                    Console.WriteLine( "Formula in Cyclic form:" );
                    Console.WriteLine( $"1/24( Т1^12 +3*T2^6 + 6*T4^3 + 6*T1^2*T2^5 + 8*T3^4 )" );
                    Console.WriteLine( "\nFormula in Polynomial form:" );
                    Console.WriteLine( $"1/24( ( x + y + z )^12 + 6*( x^4 + y^4 + z^4 )^3 + 3*( x^2 + y^2 + z^2 )^6 + 6*( x + y + z )^2 * ( x^2 + y^2 + z^2 )^5 + 8*( x^3 + y^3 + z^3 )^4 )" );
                    break;
                case Type.XoranardNist:
                    result.Append( $"1/24( " );
                    Console.WriteLine( "Formula in Cyclic form:" );
                    Console.WriteLine( $"1/24( T1^6 + 3*T1^2*T2^2 + 6*T1^2*T4^1 + 6*T2^3 + 8*T3^2 )" );
                    Console.WriteLine( "\nFormula in Polynomial form:" );
                    Console.WriteLine( $"1/24( ( x + y + z )^6 + 3*( x + y + z )^2*( x^2 + y^2 + z^2)^2 + 6*( x + y + z )^2* ( x^4 + y^4 + z^4 ) + 6*( x^2 + y^2 + z^2 )^3 + 8*( x^3 + y^3 + z^3 )^2 )" );
                    break;
                case Type.QaranistGagat:
                    result.Append( $"1/12( " );
                    Console.WriteLine( "Formula in Cyclic form:" );
                    Console.WriteLine( $"1/12( T1^4 + 3*T2^2 + 8*T1^1*T3^1 )" );
                    Console.WriteLine( "\nFormula in Polynomial form:" );
                    Console.WriteLine( $"1/12( ( x + y + z )^4 + 3*( x^2 + y^2 + z^2 )^2 + 8*( x + y + z)*( x^3 + y^3 + z^3) )" );
                    break;
                case Type.QaranistKox:
                    result.Append( $"1/12( " );
                    Console.WriteLine( "Formula in Cyclic form:" );
                    Console.WriteLine( $"1/12( T1^6 + 8*T3^2 +3*T1^2*T2^2 )" );
                    Console.WriteLine( "\nFormula in Polynomial form:" );
                    Console.WriteLine( $"1/12( ( x + y + z )^6 + 8*( x^3 + y^3 + z^3 )^2 + 3*( x + y + z )^2*( x^2 + y^2 + z^2 )^2 )" );
                    break;
                case Type.QaranistNist:
                    result.Append( $"1/12( " );
                    Console.WriteLine( "Formula in Cyclic form:" );
                    Console.WriteLine( $"1/12( T1^4 + 3*T2^2 + 8*T1^1*T3^1 )" );
                    Console.WriteLine( "\nFormula in Polynomial form:" );
                    Console.WriteLine( $"1/12( ( x + y + z )^4 + 3*( x^2 + y^2 + z^2 )^2 + 8*( x + y + z)*( x^3 + y^3 + z^3) )" );
                    break;
            }

            Console.WriteLine( $"\nC-m/n or for example C-1/2 means: m! / n! * ( m - n )!\n" );

            int i = -1;
            int brake = 0;
            foreach ( Set set in CyclicFormulas[type] )
            {
                i++;
                if ( set.SecondSet == null )
                {
                    int total = set.Expo;

                    if ( set.InnerExpo > targExpo1 || set.InnerExpo > targExpo2 || set.InnerExpo > targExpo3 )
                    {
                        result.Append( $"{set.Mult}*0" );
                        if ( i < CyclicFormulas[type].Length - 1 )
                        {
                            result.Append( $" + " );
                        }
                        continue;
                    }

                    if ( !CanBeAchieved( targExpo1, set.InnerExpo ) || !CanBeAchieved( targExpo2, set.InnerExpo ) || !CanBeAchieved( targExpo3, set.InnerExpo ) )
                    {
                        result.Append( $"{set.Mult}*0" );
                        if ( i < CyclicFormulas[type].Length - 1 )
                        {
                            result.Append( $" + " );
                        }
                        continue;
                    }

                    if ( set.Mult > 1 )
                    {
                        result.Append( $"{set.Mult}" );
                    }

                    int n = 0;
                    int iter = 0;
                    while ( n != targExpo1 )
                    {
                        if ( n < targExpo1 )
                        {
                            n += set.InnerExpo;
                        }
                        iter++;
                        brake++;
                        if ( brake > 15 )
                        {

                            result.Append( "* 0" );
                            break;
                        }
                    }
                    brake = 0;
                    if ( total < iter )
                    {
                        result.Append( "0*" );

                    }
                    else
                    {
                        if ( i != 0 ) result.Append( " * " );
                        result.Append( $"C-{total}/{iter}" );
                    }
                    total -= iter;

                    n = 0;
                    iter = 0;
                    while ( n != targExpo2 )
                    {
                        if ( n < targExpo2 )
                        {
                            n += set.InnerExpo;
                        }
                        iter++;
                        brake++;
                        if ( brake > 15 )
                        {

                            result.Append( "* 0" );
                            break;
                        }
                    }
                    brake = 0;
                    if ( total < iter )
                    {
                        result.Append( "0*" );

                    }
                    else
                    {
                        result.Append( " * " );
                        result.Append( $"C-{total}/{iter}" );
                    }
                    total -= iter;

                    iter = 0;
                    n = 0;
                    while ( n != targExpo3 )
                    {
                        if ( n < targExpo3 )
                        {
                            n += set.InnerExpo;
                        }
                        iter++;
                        brake++;
                        if ( brake > 15 )
                        {

                            result.Append( "* 0" );
                            break;
                        }
                    }
                    brake = 0;
                    if ( total < iter )
                    {
                        result.Append( "0" );

                    }
                    else
                    {
                        result.Append( " * " );
                        result.Append( $"C-{total}/{iter}" );
                    }
                }
                else
                {
                    if (
                        ( set.InnerExpo > targExpo1 || set.InnerExpo > targExpo2 || set.InnerExpo > targExpo3 ||
                        !CanBeAchieved( targExpo1, set.InnerExpo ) ||
                        !CanBeAchieved( targExpo2, set.InnerExpo ) ||
                        !CanBeAchieved( targExpo3, set.InnerExpo ) ||
                        set.SecondSet.InnerExpo > targExpo1 ) && ( set.SecondSet.InnerExpo > targExpo2 || set.SecondSet.InnerExpo > targExpo3 ||
                        !CanBeAchieved( targExpo1, set.SecondSet.InnerExpo ) ||
                        !CanBeAchieved( targExpo2, set.SecondSet.InnerExpo ) ||
                        !CanBeAchieved( targExpo3, set.SecondSet.InnerExpo ) ) 
                    )
                    {
                        result.Append( $"{set.Mult}*0" );
                        if ( i < CyclicFormulas[type].Length - 1 )
                        {
                            result.Append( $" + " );
                        }
                        continue;
                    }

                    int total = set.Expo;
                    int total2 = set.SecondSet.Expo;

                    if ( set.Mult > 1 )
                    {
                        result.Append( $"{set.Mult}" );
                    }

                    int n = 0;
                    int n2 = 0;
                    int iter = 0;
                    int iter2 = 0;

                    while ( n != targExpo1 )
                    {
                        if ( targExpo1 % set.SecondSet.InnerExpo == 0 )
                        {
                            n = targExpo1;
                            iter2 += targExpo1 / set.SecondSet.InnerExpo;
                            break;
                        }
                        if ( n < targExpo1 && total != 0 && ( n + set.InnerExpo + set.SecondSet.InnerExpo <= targExpo1 ) )
                        {
                            n += set.InnerExpo;
                            iter++;
                        }
                        if ( n < targExpo1 && total2 != 0 && ( n + set.SecondSet.InnerExpo <= targExpo1 ) )
                        {
                            n += set.SecondSet.InnerExpo;
                            iter2++;
                        }
                    }
                    if ( iter != 0 )
                    {
                        if ( total < iter )
                        {
                            result.Append( " * 0" );

                        }
                        else
                        {
                            result.Append( " * " );
                            result.Append( $"C-{total}/{iter}" );
                        }

                    }
                    if ( iter2 != 0 )
                    {
                        if ( total2 < iter2 )
                        {
                            result.Append( " * 0" );

                        }
                        else
                        {
                            result.Append( " * " );
                            result.Append( $"C-{total2}/{iter2}" );
                        }
                    }
                    total -= iter;
                    total2 -= iter2;

                    n = 0; n2 = 0; iter = 0; iter2 = 0;
                    while ( n != targExpo2 )
                    {
                        if ( targExpo2 % set.SecondSet.InnerExpo == 0 )
                        {
                            n = targExpo2;
                            iter2 += targExpo2 / set.SecondSet.InnerExpo;
                            break;
                        }
                        if ( n < targExpo2 && total != 0 && ( n + set.InnerExpo + set.SecondSet.InnerExpo <= targExpo2 ) )
                        {
                            n += set.InnerExpo;
                            iter++;
                        }
                        if ( n < targExpo2 && total2 != 0 && ( n + set.SecondSet.InnerExpo <= targExpo2 ) )
                        {
                            n += set.SecondSet.InnerExpo;
                            iter2++;
                        }
                        brake++;
                        if ( brake > 15 )
                        {

                            result.Append( "* 0" );
                            break;
                        }
                    }
                    brake = 0;
                    if ( iter != 0 )
                    {
                        if ( total < iter )
                        {
                            result.Append( " * 0" );

                        }
                        else
                        {
                            result.Append( " * " );
                            result.Append( $"C-{total}/{iter}" );
                        }
                    }
                    if ( iter2 != 0 )
                    {
                        if ( total2 < iter2 )
                        {
                            result.Append( " * 0" );

                        }
                        else
                        {
                            result.Append( " * " );
                            result.Append( $"C-{total2}/{iter2}" );
                        }
                    }
                    total -= iter;
                    total2 -= iter2;

                    n = 0; n2 = 0; iter = 0; iter2 = 0;
                    while ( n != targExpo3 )
                    {
                        if ( targExpo3 % set.SecondSet.InnerExpo == 0 )
                        {
                            n = targExpo3;
                            iter2 += targExpo3 / set.SecondSet.InnerExpo;
                            break;
                        }
                        if ( n < targExpo3 && total != 0 )
                        {
                            n += set.InnerExpo;
                            iter++;
                        }
                        if ( n < targExpo3 && total2 != 0 && ( n + set.SecondSet.InnerExpo <= targExpo3 ) )
                        {
                            n += set.SecondSet.InnerExpo;
                            iter2++;
                        }
                        brake++;
                        if ( brake > 15 )
                        {

                            result.Append( "* 0" );
                            break;
                        }
                    }
                    brake = 0;
                    if ( iter != 0 && total != 0 )
                    {
                        if ( total < iter )
                        {
                            result.Append( " * 0" );

                        }
                        else
                        {
                            result.Append( " * " );
                            result.Append( $"C-{total}/{iter}" );
                        }
                    }
                    if ( iter2 != 0 && total2 != 0 )
                    {
                        if ( total2 < iter2 )
                        {
                            result.Append( " * 0" );

                        }
                        else
                        {
                            result.Append( "* " );
                            result.Append( $"C-{total2}/{iter2}" );
                        }
                    }
                    total -= iter;
                    total2 -= iter2;

                    if ( targExpo1 == targExpo2 && targExpo2 == targExpo3 ) result.Append( "*3" );
                }

                if ( i < CyclicFormulas[type].Length - 1 )
                {
                    result.Append( $" + " );
                }
            }

            result.Append( $" )" );

            Console.WriteLine( result );
        }

        static bool CanBeAchieved( int x, int y )
        {
            if ( x == y )
            {
                return true;
            }

            for ( int i = 2; i <= x / y; i++ )
            {
                if ( i * y == x )
                {
                    return true;
                }
            }
            return false;
        }

        static int Factorial( int n )
        {
            if ( n == 0 || n == 1 )
                return 1;
            return n * Factorial( n - 1 );
        }

        public enum Type
        {
            XoranardGagat,
            XoranardKox,
            XoranardNist,
            QaranistGagat,
            QaranistKox,
            QaranistNist,
        }

        class Set
        {
            public int Mult;
            public int Expo;
            public int InnerExpo;
            public Set SecondSet;
        }

        private static Dictionary<Type, Set[]> CyclicFormulas = new Dictionary<Type, Set[]>()
            {
                {
                    Type.XoranardGagat, new Set[] {
                        new Set() { Mult = 1, Expo = 8, InnerExpo = 1, SecondSet = null },
                        new Set() { Mult = 6, Expo = 2, InnerExpo = 4, SecondSet = null },
                        new Set() { Mult = 8, Expo = 2, InnerExpo = 1, SecondSet = new Set() { Mult = 1, SecondSet = null, Expo = 2, InnerExpo = 3 } },
                        new Set() { Mult = 9, Expo = 4, InnerExpo = 2, SecondSet = null }
                    }
                },
                {

                    Type.XoranardKox, new Set[] {
                        new Set() { Mult = 1, Expo = 12, InnerExpo = 1, SecondSet = null },
                        new Set() { Mult = 3, Expo = 6, InnerExpo = 2, SecondSet = null },
                        new Set() { Mult = 6, Expo = 3, InnerExpo = 4, SecondSet = null },
                        new Set() { Mult = 6, Expo = 2, InnerExpo = 1, SecondSet = new Set() { Mult = 1, InnerExpo = 2, Expo = 5, SecondSet = null } },
                        new Set() { Mult = 8, Expo = 4, InnerExpo = 3, SecondSet = null },
                    }
                },
                {

                    Type.XoranardNist, new Set[] {
                        new Set() { Mult = 1, Expo = 6, InnerExpo = 1, SecondSet = null },
                        new Set() { Mult = 3, Expo = 2, InnerExpo = 1, SecondSet = new Set() { Mult = 1, Expo = 2, InnerExpo = 2 } },
                        new Set() { Mult = 6, Expo = 2, InnerExpo = 1, SecondSet =  new Set() { Mult = 1, Expo = 1, InnerExpo = 4 } },
                        new Set() { Mult = 6, Expo = 3, InnerExpo = 2, SecondSet = null },
                        new Set() { Mult = 8, Expo = 2, InnerExpo = 3, SecondSet = null },
                    }
                },
                {

                    Type.QaranistKox, new Set[] {
                        new Set() { Mult = 1, Expo = 6, InnerExpo = 1, SecondSet = null },
                        new Set() { Mult = 8, Expo = 2, InnerExpo = 3, SecondSet = null },
                        new Set() { Mult = 3, Expo = 2, InnerExpo = 1, SecondSet = new Set() { Mult = 1, Expo = 2, InnerExpo = 2, SecondSet = null } },
                    }
                },
                {
                    Type.QaranistGagat, new Set[] {
                        new Set() { Mult = 1, Expo = 4, InnerExpo = 1, SecondSet = null },
                        new Set() { Mult = 3, Expo = 4, InnerExpo = 2, SecondSet = null },
                        new Set() { Mult = 8, Expo = 1, InnerExpo = 1, SecondSet = new Set() { Mult = 1, Expo = 1, InnerExpo = 3 } },
                    }
                },
                {
                    Type.QaranistNist, new Set[] {
                        new Set() { Mult = 1, Expo = 4, InnerExpo = 1, SecondSet = null },
                        new Set() { Mult = 3, Expo = 4, InnerExpo = 2, SecondSet = null },
                        new Set() { Mult = 8, Expo = 1, InnerExpo = 1, SecondSet = new Set() { Mult = 1, Expo = 1, InnerExpo = 3 } },
                    }
                },
            };
    }
}