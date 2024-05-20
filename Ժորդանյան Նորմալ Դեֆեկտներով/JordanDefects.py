import numpy as np

def get_rank(matrix):
    return np.linalg.matrix_rank(matrix)

def print_matrix(matrix):
    for row in matrix:
        print("\t".join(map(str, row)))
    print()

def multiply_matrices(a, b):
    return np.dot(a, b)

def add_to_diagonal(matrix, c):
    matrix = matrix.copy()
    np.fill_diagonal(matrix, matrix.diagonal() + c)
    return matrix

def remove_from_diagonal(matrix, c):
    matrix = matrix.copy()
    np.fill_diagonal(matrix, matrix.diagonal() - c)
    return matrix
    
def main():
    N = 4
    # Լրացնել մատրիցը
    # սկսել ծրագիրը
    original_matrix = np.array([
        [   -2,-3,-2,0        ],
        [  0,0,0,0             ],
        [     0,0,0,0       ],
        [   2,-2,-2,-2       ]
    ])
    
    eigenvalues, _ = np.linalg.eig(original_matrix)
    unique_eigenvalues = np.unique(eigenvalues)
    lambdas = unique_eigenvalues.tolist()  # Convert unique_eigenvalues to a list and assign it to lambdas
    
    print("Eigenvalue Expressions:")
    eigen_expressions = []
    for eigen in lambdas:
        eigen_power = np.count_nonzero(eigenvalues == eigen)
        eigen_expression = f"({'λ' if eigen == 0 else f'{eigen} - λ'})^{eigen_power}"
        eigen_expressions.append(eigen_expression)
    print(" * ".join(eigen_expressions))
    
    print("\nThe unique eigenvalues of the matrix are:")
    for i, eigen in enumerate(lambdas, start=1):
        print(f"λ{i} = {eigen}", end=", " if i < len(lambdas) else "")
    print()
    print()
    eigen_blocks_count = {}
    total_cubes = 0

    for index, eigen in enumerate(lambdas, start=1):
        matrix = original_matrix.copy()
        
        print(f"(A - λ{index}E)^1 = ")
        matrix1 = remove_from_diagonal(matrix, eigen)
        print_matrix(matrix1)

        rank1 = get_rank(matrix1)
        alpha1_1 = N - rank1
        print(f"Rank(A-λ{index}E)^1 = {rank1}")
        print(f"α{index}_1 = {alpha1_1}")
        print()

        print(f"(A - λ{index}E)^2 = ")
        matrix2 = multiply_matrices(matrix1, matrix1)
        print_matrix(matrix2)

        rank2 = get_rank(matrix2)
        alpha1_2 = N - rank2
        print(f"Rank(A-λ{index}E)^2 = {rank2}")
        print(f"α{index}_2 = {alpha1_2}")
        print(f"a{index}_0 = 0")
        print()

        print(f"(A - λ{index}E)^3 = ")
        matrix3 = multiply_matrices(matrix2, matrix1)
        print_matrix(matrix3)

        rank3 = get_rank(matrix3)
        alpha1_3 = N - rank3
        print(f"Rank(A-λ{index}E)^3 = {rank3}")
        print(f"α{index}_3 = {alpha1_3}")
        print()

        u1 = 2 * alpha1_1 - alpha1_2
        u2 = 2 * alpha1_2 - alpha1_1 - alpha1_3
        u3 = 0
        alpha1_4 = 0
        
        if u1 == 0 and u2 == 0:
            matrix4 = multiply_matrices(matrix3, matrix1)
            rank4 = get_rank(matrix4)
            alpha1_4 = N - rank4
            print(f"Rank(A-λ{index}E)^4 = {rank4}")
            print(f"α{index}_4 = {alpha1_4}")
            print()
            u3 = 2 * alpha1_3 - alpha1_4 - alpha1_2
            total_cubes += u3 * 3

        total_cubes += u1
        total_cubes += u2 * 2

        if eigen not in eigen_blocks_count:
            eigen_blocks_count[eigen] = (0, 0, 0)

        counts = eigen_blocks_count[eigen]
        eigen_blocks_count[eigen] = (counts[0] + u1, counts[1] + u2, counts[2] + u3)

        print(f"U{index}_1 = 2*{alpha1_1} - {alpha1_2} = {u1}")
        print(f"U{index}_2 = 2*{alpha1_2} - {alpha1_3} - {alpha1_1} = {u2}")
        print(f"U{index}_3 = 2*{alpha1_3} - {alpha1_4} - {alpha1_2} = {u3}")
        
        print(f"λ{index} has {u1} amount of 1-sized blocks and {u2} amount of 2-sized blocks and {u3} amount of 3-sized blocks.")
        print()
        
    print("\nJordan Canonical Form Matrix:")
    jordan_form_matrix = np.zeros((N, N), dtype=int)
    rowIndex = 0
    for eigen_value, (ones_count, twos_count, threes_count) in eigen_blocks_count.items():
        for _ in range(twos_count):
            jordan_form_matrix[rowIndex][rowIndex] = eigen_value
            if rowIndex + 1 < N:
                jordan_form_matrix[rowIndex + 1][rowIndex] = 1
                rowIndex += 1
                jordan_form_matrix[rowIndex][rowIndex] = eigen_value
            rowIndex += 1
    
        for _ in range(ones_count):
            jordan_form_matrix[rowIndex][rowIndex] = eigen_value
            rowIndex += 1
    
        for _ in range(threes_count):
            if rowIndex + 3 < N:
                jordan_form_matrix[rowIndex+1][rowIndex] = 1
                jordan_form_matrix[rowIndex+2][rowIndex+1] = 1
                jordan_form_matrix[rowIndex][rowIndex ] = eigen_value
                jordan_form_matrix[rowIndex + 1][rowIndex + 1] = eigen_value
                jordan_form_matrix[rowIndex + 2][rowIndex + 2] = eigen_value
                rowIndex += 3
    
    print_matrix(jordan_form_matrix)

if __name__ == "__main__":
    main()