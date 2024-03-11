namespace Uni_Scripts.SmithNormalForm
{
    public class Program
    {
        public static void Main( string[] args )
        {
            // 
            //  Լրացնել Մատրիցը, NxN չափի։
            //
            int[,] M = {
                { 2,4,4 },
                { -6,6,12 },
                { 10,4,16 }
            };

            Smith( M );
        }

        public static (int, int) Dims( int[,] M )
        {
            int numRighe = M.GetLength( 0 );
            int numColonne = M.GetLength( 1 );
            return (numRighe, numColonne);
        }

        public static int[] MinAij( int[,] M, int s )
        {
            (int numRighe, int numColonne) = Dims( M );
            int[] ijmin = { s, s };
            int valmin = int.MaxValue;
            for ( int i = s; i < numRighe; i++ )
            {
                for ( int j = s; j < numColonne; j++ )
                {
                    if ( M[i, j] != 0 && Math.Abs( M[i, j] ) <= valmin )
                    {
                        ijmin = new int[] { i, j };
                        valmin = Math.Abs( M[i, j] );
                    }
                }
            }
            return ijmin;
        }

        public static int[,] IdentityMatrix( int n )
        {
            int[,] res = new int[n, n];
            for ( int i = 0; i < n; i++ )
            {
                res[i, i] = 1;
            }
            return res;
        }

        public static string Display( int[,] M )
        {
            string r = "\n";
            for ( int i = 0; i < M.GetLength( 0 ); i++ )
            {
                for ( int j = 0; j < M.GetLength( 1 ); j++ )
                {
                    r += M[i, j].ToString() + " ";
                }
                r += "\n";
            }
            return r + "";
        }

        public static void SwapRows( int[,] M, int i, int j )
        {
            for ( int col = 0; col < M.GetLength( 1 ); col++ )
            {
                (M[j, col], M[i, col]) = (M[i, col], M[j, col]);
            }
        }

        public static void SwapColumns( int[,] M, int i, int j )
        {
            for ( int row = 0; row < M.GetLength( 0 ); row++ )
            {
                (M[row, j], M[row, i]) = (M[row, i], M[row, j]);
            }
        }

        public static void AddToRow( int[,] M, int x, int k, int s )
        {
            (int numRighe, int numColonne) = Dims( M );
            for ( int tmpj = 0; tmpj < numColonne; tmpj++ )
            {
                M[x, tmpj] += k * M[s, tmpj];
            }
        }

        public static void AddToColumn( int[,] M, int x, int k, int s )
        {
            (int numRighe, int numColonne) = Dims( M );
            for ( int tmpj = 0; tmpj < numRighe; tmpj++ )
            {
                M[tmpj, x] += k * M[tmpj, s];
            }
        }

        public static void ChangeSignRow( int[,] M, int x )
        {
            (int numRighe, int numColonne) = Dims( M );
            for ( int tmpj = 0; tmpj < numColonne; tmpj++ )
            {
                M[x, tmpj] = -M[x, tmpj];
            }
        }

        public static void ChangeSignColumn( int[,] M, int x )
        {
            (int numRighe, int numColonne) = Dims( M );
            for ( int tmpj = 0; tmpj < numRighe; tmpj++ )
            {
                M[tmpj, x] = -M[tmpj, x];
            }
        }

        public static bool IsLone( int[,] M, int s )
        {
            (int numRighe, int numColonne) = Dims( M );
            for ( int x = s + 1; x < numColonne; x++ )
            {
                if ( M[s, x] != 0 ) return false;
            }
            for ( int y = s + 1; y < numRighe; y++ )
            {
                if ( M[y, s] != 0 ) return false;
            }
            return true;
        }

        public static (int, int)? GetNextEntry( int[,] M, int s )
        {
            (int numRighe, int numColonne) = Dims( M );
            for ( int x = s + 1; x < numRighe; x++ )
            {
                for ( int y = s + 1; y < numColonne; y++ )
                {
                    if ( M[x, y] % M[s, s] != 0 )
                    {
                        return (x, y);
                    }
                }
            }
            return null;
        }

        public static void Smith( int[,] M )
        {
            (int numRighe, int numColonne) = Dims( M );
            int[,] L = IdentityMatrix( numRighe );
            int[,] R = IdentityMatrix( numColonne );
            int maxs = Math.Min( numRighe, numColonne );
            for ( int s = 0; s < maxs; s++ )
            {
                Console.WriteLine( $"Step {s + 1}/{maxs}:\n" );
                Console.WriteLine( "Original Matrix (M):" + Display( M ) );
                while ( !IsLone( M, s ) )
                {
                    int[] minAij = MinAij( M, s );
                    int i = minAij[0];
                    int j = minAij[1];

                    SwapRows( M, s, i );
                    SwapRows( L, s, i );
                    SwapColumns( M, s, j );
                    SwapColumns( R, s, j );
                    Console.WriteLine( $"Swapped rows and columns to move M[{i}][{j}] to M[{s}][{s}]:" + Display( M ) );

                    for ( int x = s + 1; x < numRighe; x++ )
                    {
                        if ( M[x, s] != 0 )
                        {
                            int k = M[x, s] / M[s, s];
                            AddToRow( M, x, -k, s );
                            AddToRow( L, x, -k, s );
                            Console.WriteLine( $"Added {-k} times row {s} to row {x} to make M[{x}][{s}] = 0:" + Display( M ) );
                        }
                    }

                    for ( int x = s + 1; x < numColonne; x++ )
                    {
                        if ( M[s, x] != 0 )
                        {
                            int k = M[s, x] / M[s, s];
                            AddToColumn( M, x, -k, s );
                            AddToColumn( R, x, -k, s );
                            Console.WriteLine( $"Added {-k} times column {s} to column {x} to make M[{s}][{x}] = 0:" + Display( M ) );
                        }
                    }
                    if ( IsLone( M, s ) )
                    {
                        (int, int)? res = GetNextEntry( M, s );
                        if ( res.HasValue )
                        {
                            (int x, int y) = res.Value;
                            AddToRow( M, s, 1, x );
                            AddToRow( L, s, 1, x );
                            Console.WriteLine( $"Added 1*M[{i}][{j}]  to M[{s}][{x}] to make it equal to 1:" + Display( M ) );
                        }
                        else
                        {
                            if ( M[s, s] < 0 )
                            {
                                ChangeSignRow( M, s );
                                ChangeSignRow( L, s );
                                Console.WriteLine( $"Changed the sign of row {s} to make M[{s}][{s}] positive:" + Display( M ) );
                            }
                        }
                    }
                    Console.WriteLine( "\n" );
                }
                if ( M[s, s] < 0 )
                {
                    ChangeSignRow( M, s );
                    ChangeSignRow( L, s );
                    Console.WriteLine( $"Changed the sign of row {s} to make M[{s}][{s}] positive:" + Display( M ) );
                }
            }
        }
    }
}