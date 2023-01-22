using System;
namespace DependencyGraph.CLI
{
	public static class ArrayHelper
	{
        public static string[,] To2D(this string[][] input)
        {
            if(input.Length == 0)
            {
                return new string[,] { };
            }
            try
            {
                int row = input.Length;
                int col = input.GroupBy(row => row.Length).Single().Key;

                var result = new string[row, col];
                for (int i = 0; i < row; ++i)
                    for (int j = 0; j < col; ++j)
                        result[i, j] = input[i][j];

                return result;
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("Givenn Jagged array cannot be converted to 2D array");
            }
        }
    }
}

