using System;

namespace MatricesDemo
{
  // Інтерфейс для доступу до базових операцій матриць
  public interface IMatrix
  {
    void InputFromConsole();
    void FillRandom(int minValue = -50, int maxValue = 50);
    int MinElement();
    void Print();
  }

  // Додано абстрактний базовий клас з одним спільним Random
  public abstract class Matrix : IMatrix
  {
    // Використовуємо спільний Random (Runtime-dependent); у .NET 6+ можна використовувати Random.Shared
    protected static readonly Random _rnd = Random.Shared;

    public abstract void InputFromConsole();
    public abstract void FillRandom(int minValue = -50, int maxValue = 50);
    public abstract int MinElement();
    public abstract void Print();

    // Допоміжний метод для консольного вводу цілих чисел
    protected int ReadInt(string prompt)
    {
      while (true)
      {
        Console.Write(prompt);
        string? s = Console.ReadLine();
        if (int.TryParse(s, out int val))
          return val;
        Console.WriteLine("Невірне значення. Спробуйте знову.");
      }
    }
  }

  public class TwoDMatrix : Matrix
  {
    private int[,] _a;
    private readonly int _rows = 3;
    private readonly int _cols = 3;

    public TwoDMatrix()
    {
      _a = new int[_rows, _cols];
      Console.WriteLine("TwoDMatrix: default constructor called");
    }

    public TwoDMatrix(bool fillRandom) : this()
    {
      if (fillRandom) FillRandom();
      Console.WriteLine($"TwoDMatrix: constructed (fillRandom={fillRandom})");
    }

    public override void InputFromConsole()
    {
      Console.WriteLine("Введіть елементи двовимірної матриці 3x3 (цілі числа):");
      for (int i = 0; i < _a.GetLength(0); i++)
      {
        for (int j = 0; j < _a.GetLength(1); j++)
        {
          _a[i, j] = ReadInt($"A[{i},{j}] = ");
        }
      }
    }

    public override void FillRandom(int minValue = -50, int maxValue = 50)
    {
      if (minValue > maxValue) throw new ArgumentException("minValue має бути <= maxValue.");
      for (int i = 0; i < _a.GetLength(0); i++)
        for (int j = 0; j < _a.GetLength(1); j++)
          _a[i, j] = _rnd.Next(minValue, maxValue + 1);
    }

    public override int MinElement()
    {
      if (_a == null || _a.GetLength(0) == 0 || _a.GetLength(1) == 0)
        throw new InvalidOperationException("Масив порожній або неініціалізований.");

      int min = _a[0, 0];
      for (int i = 0; i < _a.GetLength(0); i++)
        for (int j = 0; j < _a.GetLength(1); j++)
          if (_a[i, j] < min) min = _a[i, j];
      return min;
    }

    public override void Print()
    {
      Console.WriteLine("Двовимірна матриця 3x3:");
      for (int i = 0; i < _a.GetLength(0); i++)
      {
        for (int j = 0; j < _a.GetLength(1); j++)
          Console.Write($"{_a[i, j],6}");
        Console.WriteLine();
      }
    }
  }

  public class ThreeDMatrix : Matrix
  {
    private int[,,] _b;
    private readonly int _x = 3;
    private readonly int _y = 3;
    private readonly int _z = 3;

    public ThreeDMatrix()
    {
      _b = new int[_x, _y, _z];
      Console.WriteLine("ThreeDMatrix: default constructor called");
    }

    public ThreeDMatrix(bool fillRandom) : this()
    {
      if (fillRandom) FillRandom();
      Console.WriteLine($"ThreeDMatrix: constructed (fillRandom={fillRandom})");
    }

    public override void InputFromConsole()
    {
      Console.WriteLine("Введіть елементи тривимірної матриці 3x3x3 (цілі числа):");
      for (int k = 0; k < _b.GetLength(2); k++)
      {
        Console.WriteLine($"Слой {k}:");
        for (int i = 0; i < _b.GetLength(0); i++)
        {
          for (int j = 0; j < _b.GetLength(1); j++)
          {
            _b[i, j, k] = ReadInt($"B[{i},{j},{k}] = ");
          }
        }
      }
    }


    public override void FillRandom(int minValue = -50, int maxValue = 50)
    {
      if (minValue > maxValue) throw new ArgumentException("minValue має бути <= maxValue.");
      for (int k = 0; k < _b.GetLength(2); k++)
        for (int i = 0; i < _b.GetLength(0); i++)
          for (int j = 0; j < _b.GetLength(1); j++)
            _b[i, j, k] = _rnd.Next(minValue, maxValue + 1);
    }

    public override int MinElement()
    {
      if (_b == null || _b.GetLength(0) == 0 || _b.GetLength(1) == 0 || _b.GetLength(2) == 0)
        throw new InvalidOperationException("Масив порожній або неініціалізований.");

      int min = _b[0, 0, 0];
      for (int k = 0; k < _b.GetLength(2); k++)
        for (int i = 0; i < _b.GetLength(0); i++)
          for (int j = 0; j < _b.GetLength(1); j++)
            if (_b[i, j, k] < min) min = _b[i, j, k];
      return min;
    }


    public override void Print()
    {
      Console.WriteLine("Тривимірна матриця 3x3x3 (по слоях):");
      for (int k = 0; k < _b.GetLength(2); k++)
      {
        Console.WriteLine($"Слой {k}:");
        for (int i = 0; i < _b.GetLength(0); i++)
        {
          for (int j = 0; j < _b.GetLength(1); j++)
            Console.Write($"{_b[i, j, k],6}");
          Console.WriteLine();
        }
      }
    }
  }

  static class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Демонстрація класів двовимірної та тривимірної матриць (3x3, 3x3x3)");
      TwoDMatrix m2 = new TwoDMatrix();
      m2.FillRandom();
      m2.Print();
      Console.WriteLine($"Мінімальний елемент двовимірної матриці: {m2.MinElement()}");
      Console.WriteLine();

      Matrix m3 = new ThreeDMatrix(); // використаємо поліморфізм через базовий тип
      m3.FillRandom(); // викличеться перевизначений метод
      m3.Print(); // перевизначений Print
      Console.WriteLine($"Мінімальний елемент тривимірної матриці: {m3.MinElement()}");
      Console.WriteLine();

  // --- Демонстрація використання інтерфейсу IMatrix ---
  IMatrix im2 = new TwoDMatrix(true);    // конструктор, що одразу заповнює випадково
  IMatrix im3i = new ThreeDMatrix(true);

  Console.WriteLine("\n--- Виклик через інтерфейс IMatrix для двовимірної матриці ---");
  im2.Print();
  Console.WriteLine($"Мінімум через інтерфейс (2D): {im2.MinElement()}");

  Console.WriteLine("\n--- Виклик через інтерфейс IMatrix для тривимірної матриці ---");
  im3i.Print();
  Console.WriteLine($"Мінімум через інтерфейс (3D): {im3i.MinElement()}");

      Console.WriteLine();

      Console.WriteLine("Приклад вводу з клавіатури для двовимірної матриці:");
      var user2 = new TwoDMatrix();
      user2.InputFromConsole();
      user2.Print();
      Console.WriteLine($"Мінімум (користувацька матриця 2D): {user2.MinElement()}");
      Console.WriteLine();

      Console.WriteLine("Приклад вводу з клавіатури для тривимірної матриці:");
      var user3 = new ThreeDMatrix();
      user3.InputFromConsole();
      user3.Print();
      Console.WriteLine($"Мінімум (користувацька матриця 3D): {user3.MinElement()}");
    }
  }
}