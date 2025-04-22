using System;
using System.Threading.Tasks;

public class Matrix
{
    public int[,] Data { get; }
    public int Rows => Data.GetLength(0);
    public int Cols => Data.GetLength(1);

    public Matrix(int rows, int cols, bool randomFill = false)
    {
        Data = new int[rows, cols];
        if (randomFill)
        {
            var random = new Random();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Data[i, j] = random.Next(1, 10); 
                }
            }
        }
    }

    public static Matrix Multiply(Matrix a, Matrix b)
    {
        if (a.Cols != b.Rows)
            throw new ArgumentException("Количество столбцов первой матрицы должно равняться количеству строк второй");

        var result = new Matrix(a.Rows, b.Cols);

        for (int i = 0; i < a.Rows; i++)
        {
            for (int j = 0; j < b.Cols; j++)
            {
                int sum = 0;
                for (int k = 0; k < a.Cols; k++)
                {
                    sum += a.Data[i, k] * b.Data[k, j];
                }
                result.Data[i, j] = sum;
            }
        }

        return result;
    }

    public static async Task<Matrix> MultiplyAsync(Matrix a, Matrix b)
    {
        return await Task.Run(() => Multiply(a, b));
    }

    public void Print(string title = null)
    {
        if (!string.IsNullOrEmpty(title))
            Console.WriteLine(title);

        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
            {
                Console.Write(Data[i, j].ToString().PadLeft(3) + " ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Асинхронное умножение матриц");
        Console.WriteLine("----------------------------\n");

        Matrix a = new Matrix(2, 3, randomFill: true);
        Matrix b = new Matrix(3, 2, randomFill: true);

        a.Print("Матрица A (2x3):");
        b.Print("Матрица B (3x2):");

        Console.WriteLine("Умножаем матрицы...\n");
        Matrix result = await Matrix.MultiplyAsync(a, b);

        result.Print("Результат умножения (2x2):");
    }
}