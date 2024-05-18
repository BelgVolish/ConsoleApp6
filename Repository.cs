using System.Text;

namespace ConsoleApp6;

public class Repository
{
    private const string FileName = @"C:\Users\Public\Documents\Workers.txt";
    private readonly Dictionary<int, Worker> _workers;

    public List<Worker> GetAllWorkers()
    {
        return _workers.Values.ToList();
    }

    public void AddWorker()
    {
        Console.WriteLine("Введите ФИО работника");
        string? name = Console.ReadLine(); 
        
        Console.WriteLine("Введите рост работника");
        if (!int.TryParse(Console.ReadLine(), out int height))
        {
            Console.WriteLine("Не удалось считать рост");
            return;
        }

        Console.WriteLine("Введите место рождения");
        string? birthPlace = Console.ReadLine();
        
        Console.WriteLine("Введите дату рождения работника");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime dateTime))
        {
            Console.WriteLine("Не удалось считаь дату рождения работника");
            return;
        }

        int id = 0;

        if (_workers.Count != 0)
        {
            id = _workers.Last().Key + 1;
        }

        Worker worker = new Worker(id, name!, height, dateTime, birthPlace!);
        _workers.Add(worker.Id ,worker); 
        Console.WriteLine("Работник успешно добавлен");
        WriteFile();
    }

    public bool TryGetWorkerById(int id, out Worker worker)
    {
        if (_workers.TryGetValue(id, out worker))
        {
            return true;
        }

        worker = new Worker();
        return false;
    }

    public void DeleteWorker()
    {
        Console.WriteLine("Введите ID работника которого нужно удалить");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Не удалось распознать ID");
            return;
        }
        
        _workers.Remove(id);
        Console.WriteLine("Работник успешно удалён");
        WriteFile();
    }

    public void WriteFile()
    {
        File.WriteAllText(FileName,string.Empty);
        
        using StreamWriter streamWriter = new StreamWriter(FileName, true, Encoding.Unicode);
        foreach (var var in _workers)
        {
            streamWriter.WriteLine(var.Value.ConvertToString());
        }
        streamWriter.Close();
    }

    private void ReadFile()
    {
        if (!File.Exists(FileName))
        {
            var fileStream = File.Create(FileName);
            fileStream.Close();
        }
        
        using StreamReader streamReader = new StreamReader(FileName, Encoding.Unicode);
        while (!streamReader.EndOfStream)
        {
            string? text = streamReader.ReadLine();
            if (text == null) continue;
            
            Worker worker = Worker.ConvertFromString(text);
            _workers.Add(worker.Id, worker);
        }
            
        streamReader.Close();
        
        File.WriteAllText(FileName,string.Empty);
    }

    public Repository()
    {
        _workers = new Dictionary<int, Worker>();
        ReadFile();
    }
}