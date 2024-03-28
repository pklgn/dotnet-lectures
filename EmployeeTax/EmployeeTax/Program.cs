namespace EmployeeTax;

internal class Program
{
    private static List<(string Employee, decimal Tax)> _employees = new List<(string, decimal)>
    {
        ("Алексей Смирнов", 1500.30m),
        ("Андрей Иванов", 1700.80m),
        ("Дмитрий Кузнецов", 1950.25m),
        ("Екатерина Соколова", 2750.65m),
        ("Иван Попов", 2500.50m),
        ("Мария Лебедева", 3100.55m),
        ("Михаил Новиков", 2200.00m),
        ("Наталья Морозова", 3000.90m),
        ("Ольга Петрова", 3200.75m),
        ("Светлана Волкова", 2800.45m)
    };

    private static void Main(string[] args)
    {
        SortEmployees();
        Console.WriteLine("Введите пару 'Работник-Налог' или 'exit' для выхода:");

        while (true)
        {
            string? input = Console.ReadLine();
            if (input.ToLower() == "exit")
            {
                break;
            }

            if (input.ToLower() == "list")
            {
                PrintEmployees();
                continue;
            }

            if (input.ToLower() == "save")
            {
                SaveEmployees();
                continue;
            }

            string[] parts = input.Split(':');
            if (parts.Length != 2)
            {
                Console.WriteLine("Неверный формат ввода. Попробуйте снова.");
                continue;
            }

            string employee = parts[0].Trim();
            if (decimal.TryParse(parts[1].Trim(), out decimal tax))
            {
                AddEmployee(employee, tax);
            }
            else
            {
                Console.WriteLine("Невозможно преобразовать налог в число. Попробуйте снова.");
            }
        }
    }

    private static void AddEmployee(string employee, decimal tax)
    {
        int position = _employees.FindIndex(e => e.Employee[0] > employee[0]);
        if (position == -1) _employees.Add((employee, tax));
        else _employees.Insert(position, (employee, tax));
    }

    private static void PrintEmployees()
    {
        foreach ((string employee, decimal tax) in _employees)
        {
            Console.WriteLine($"{employee} - {tax}");
        }
    }

    private static void SaveEmployees()
    {
        SortEmployees();
        // TODO: дополнительная логика по сохранению
    }

    private static void SortEmployees()
    {
        _employees = _employees.OrderBy(e => e.Employee).ToList();
    }
}
