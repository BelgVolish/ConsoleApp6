// ReSharper disable StringLiteralTypo
namespace ConsoleApp6;

static class Program
{
    private static bool _isTaskPerformed;
    private static TaskType _currentTask;
    private static Repository _repository = null!;
    private static List<Worker>? _lastWorkersList;

    static void Main()
    {
        _isTaskPerformed = false;
        _currentTask = TaskType.None;
        _repository = new Repository();
        _lastWorkersList = new List<Worker>();
        DoApp();
    }

    private static void WriteWorkersFormatted(List<Worker> workers, string emptyListAnswer = "По вышему запросу ничего не найдено", OrdersType ordersType = OrdersType.Id)
    {
        _lastWorkersList = workers;

        if (workers.Count == 0)
        {
            Console.WriteLine(emptyListAnswer);    
        }
        else
        {
            List<Worker> workers2 = ordersType switch
            {
                OrdersType.Id => workers.OrderBy(x => x.Id).ToList(),
                OrdersType.Name => workers.OrderBy(x => x.Name).ToList(),
                OrdersType.BirthDate => workers.OrderBy(x => x.BirthDate).ToList(),
                OrdersType.AddDate => workers.OrderBy(x => x.AddTime).ToList(),
                OrdersType.Height => workers.OrderBy(x => x.Height).ToList(),
                OrdersType.LivePlace => workers.OrderBy(x => x.BirthPlace).ToList(),
                OrdersType.Years => workers.OrderBy(x => x.Years).ToList(),
                _ => throw new ArgumentOutOfRangeException(nameof(ordersType), ordersType, null)
            };

            Console.WriteLine("Id\t    ФИО\t  Возраст\t  Дата рождения\t  Место рождения\t   Рост\t    Дата добавления в систему");
            foreach (var worker in workers2)
            {
                Console.WriteLine("{0}\t    {1}\t  {2}\t    {3}\t    {4}\t    {5}\t    {6}", worker.Id, worker.Name, worker.Years,
                    worker.BirthDate.Date, worker.BirthPlace, worker.Height, worker.AddTime);
            }
        }
    }
    

    private static void GetWorkerById()
    {
        Console.WriteLine("Введите нужный ID");
        if (!int.TryParse(Console.ReadLine(), out int result))
        {
            Console.WriteLine("Не удалось распознать Id");
        }
        else
        {
            if (_repository.TryGetWorkerById(result, out Worker worker))
            {
                List<Worker> list = new List<Worker> { worker };
                WriteWorkersFormatted(list);
            }
            else
            {
                Console.WriteLine("По вышему запросу ничего не найдено");
            }
        }
    }

    private static void ChooseAction(TaskType taskType)
    {
        switch (taskType)
        {
            case TaskType.CloseApp:
                _repository.WriteFile();
                Environment.Exit(0);
                break;
            case TaskType.SeeAll:
                SeeAll();
                break;
            case TaskType.GetById:
                GetWorkerById();
                break;
            case TaskType.CreateWorker:
                _repository.AddWorker();
                break;
            case TaskType.DeleteWorker:
                _repository.DeleteWorker();
                break;
            case TaskType.SeeAllCreatedInTwoDates:
                break;
            case TaskType.None:
            default:
                Console.WriteLine("Неизвестная команда");
                break;
        }
    }

    private static void Order(List<Worker> workers)
    {
        Console.WriteLine("По какому критерию фильтровать?");
        if (!Enum.TryParse(Console.ReadLine(), true, out OrdersType ordersType))
        {
            Console.WriteLine("Не удалось распознать действие");
            DoApp();
        }
        else
        {
           WriteWorkersFormatted(workers,"По вышему запросу ничего не найдено", ordersType);
        }
    }

    private static void SeeAll() => WriteWorkersFormatted(_repository.GetAllWorkers(), "В системе нет записанных работников");

    private static void DoApp()
    {
        while (true)
        {
            if (!_isTaskPerformed)
            {
                Console.WriteLine("\nВведите команду");
                if (!Enum.TryParse(Console.ReadLine(), true, out TaskType taskType))
                {
                    Console.WriteLine("Не удалось распознать команду");
                    DoApp();
                }

                ChooseAction(taskType);
                
                _isTaskPerformed = true;

                if (taskType != TaskType.CloseApp)
                {
                    continue;
                }

                break;
            }

            Console.WriteLine("Введите действие с полученными данными");
            if (!Enum.TryParse(Console.ReadLine(),true, out ActionsType actionsType))
            {
                Console.WriteLine("Не удалось распознать действие");
                _isTaskPerformed = false;
            }
            else
            {
                switch (actionsType)
                {
                    case ActionsType.Redo:
                        ChooseAction(_currentTask);
                        break;
                    case ActionsType.Order:
                        if (_lastWorkersList != null) Order(_lastWorkersList);
                        break;
                    case ActionsType.Exit:
                        _currentTask = TaskType.None;
                        _isTaskPerformed = false;
                        DoApp();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}

public enum TaskType
{
    CloseApp,
    SeeAll,
    GetById,
    CreateWorker,
    DeleteWorker,
    SeeAllCreatedInTwoDates,
    None
}

public enum ActionsType
{
    Redo,
    Order,
    Exit
}

public enum OrdersType
{
    Id,
    Name,
    BirthDate,
    AddDate,
    Height,
    LivePlace,
    Years
}