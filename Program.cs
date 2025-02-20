using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SFyCSm009;

internal class Program
{
    /// <summary>
    /// Объявление события выполняющего сортировку списка после выбор варианта сортировки пользователем
    /// </summary>
    public static event Action<List<string>, string>? SortRequested;

    /// <summary>
    /// Процедура обработчик события ввода варианта сортировки пользователя
    /// Выполняет сортировку переданного списка в зависимости от выбранного варианата сортировки.
    /// </summary>
    /// <param name="names">Список строк который нужно отсортировать</param>
    /// <param name="sortType">Вариант сортировки ("1" от А до Я, "2" от Я до А)</param>
    private static void OnSortRequested(List<string> names, string sortType)
    {
        if (sortType == "1")
            names.Sort((a, b) => string.Compare(a, b, StringComparison.Ordinal));
        else if (sortType == "2")
            names.Sort((a, b) => string.Compare(b, a, StringComparison.Ordinal));
    }

    /// <summary>
    /// Процедура реализующая основной алгоритм работы программы по 2й части задания (сортировка).
    /// </summary>
    static void PerformSorting()
    {
        List<string> names = new List<string> { "Борис", "Дмитрий", "Григорий", "Андрей", "Владимир" };

        SortRequested += OnSortRequested;

        Console.WriteLine("Выберите вариант сортировки: 1 А-Я или 2 Я-А:");

        try
        {
            string sortType = InputValue();

            SortRequested?.Invoke(names, sortType);

            Console.WriteLine("\nОтсортированный список:");

            foreach (var name in names)
                Console.WriteLine(name);
        }
        catch (Exception exception)
        {
            Console.WriteLine("Не корректный ввод: {0}", exception.Message);
        }
    }

    /// <summary>
    /// Класс исключения InvalidInputException.
    /// </summary>
    public class InvalidInputException : Exception
    {
        public InvalidInputException(string message = "Недопустимое значение, введите 1 или 2.") : base(message) { }
    }

    /// <summary>
    /// Метод вызывающий добавленное исключение InvalidInputException.
    /// </summary>
    static string InputValue()
    {
        Console.Write("\nВведите 1 или 2: ");

        string? value = Console.ReadLine();

        if (value != "1" && value != "2")
            throw new InvalidInputException();

        return value;
    }

    /// <summary>
    /// Метод вызывающий исключение DivideByZeroException.
    /// </summary>
    static void DivideByZero()
    {
        int value = 1;

        value = value / (value - value);
    }

    /// <summary>
    /// Метод вызывающий исключение ArgumentNullException.
    /// </summary>
    static void ArgumentNull()
    {
        int.Parse(null);
    }

    /// <summary>
    /// Метод вызывающий исключение IndexOutOfRangeException.
    /// </summary>
    static void IndexOutOfRange()
    {
        string[]? value = ["1", "2"];

        value[2] = "3";
    }

    /// <summary>
    /// Метод вызывающий исключение FormatException.
    /// </summary>
    static void Format()
    {
        decimal price = 169.32m;

        Console.WriteLine("The cost is {0:Q2}.", price);
    }

    /// <summary>
    /// Процедура реализующая основной алгоритм работы программы по 1й части задания (исключения).
    /// </summary>
    static void TestException()
    {
        // Массив исключений для последовательной обработки
        Exception[] exceptions = new Exception[]
        {
            new InvalidInputException("Не корректный ввод"),
            new DivideByZeroException("Деление на 0"),
            new ArgumentNullException("", "Значение Null"),
            new IndexOutOfRangeException("Ошибка индекса"),
            new FormatException("Ошибка форматрирования"),
        };

        // Цикл выполняющий проверку ислючений
        foreach (var ex in exceptions)
            try
            {
                if (ex is InvalidInputException)
                    InputValue();
                else if (ex is DivideByZeroException)
                    DivideByZero();
                else if (ex is ArgumentNullException)
                    ArgumentNull();
                else if (ex is IndexOutOfRangeException)
                    IndexOutOfRange();
                else if (ex is FormatException)
                    Format();
            }
            catch (Exception exception)
            {
                Console.WriteLine("Проверка исключения \"{0}\": {1}", ex.Message, exception.Message);
            }
    }

    /// <summary>
    /// Главная точка входа приложения
    /// </summary>
    /// <param name="args">Аргументы командной строки при запуске приложения.</param> 
    static void Main(string[] args)
    {
        Console.WriteLine("\n1 часть. Проверка исключений.");

        TestException();

        Console.WriteLine("\n2 часть. Сортировка.");

        PerformSorting();
    }
}